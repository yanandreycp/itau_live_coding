# LiveCoding - Sistema de Pedidos

API REST para gerenciamento de pedidos com cálculo de preços baseado no tipo de pedido (Standard, Express, Subscription).

---

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/orders` | Criar pedido |
| `GET` | `/orders/{id}` | Obter pedido |
| `PUT` | `/orders/{orderId}/items/{itemId}` | Alterar quantidade de item |
| `DELETE` | `/orders/{orderId}/items/{itemId}` | Remover item do pedido |

---

## Glossário de Campos

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `id` | `guid` | Identificador único do pedido |
| `type` | `enum string` | `"Standard"`, `"Express"` ou `"Subscription"` (aceita também `0`, `1`, `2` no input) |
| `initialPrice` | `decimal` | Soma dos preços originais dos produtos (sem ajuste de taxa) |
| `effectivePrice` | `decimal` | Preço final após aplicar taxa do tipo de pedido |
| `createdAt` | `datetime` | Data/hora de criação |
| `updatedAt` | `datetime` ou `null` | Data/hora da última alteração |
| `name` | `string` | Nome do produto |
| `price` | `decimal` | Preço unitário original (apenas no input) |
| `quantity` | `int` | Quantidade do produto |
| `originalUnitPrice` | `decimal` | Preço unitário original |
| `effectiveUnitPrice` | `decimal` | Preço unitário após ajuste de taxa |
| `rate` | `decimal` | Taxa aplicada: `0.0` (Standard), `0.15` (Express), `-0.10` (Subscription) |
| `delta` | `decimal` | Diferença entre preço original e efetivo (`originalUnitPrice * quantity * rate`) |
| `totalPrice` | `decimal` | Preço total do item (`effectiveUnitPrice * quantity`) |
| `orderId` | `guid` | ID do pedido (apenas em parâmetro de rota) |
| `itemId` | `guid` | ID do produto dentro do pedido (apenas em parâmetro de rota) |

---

## POST /orders

Cria um novo pedido. O tipo do pedido define a taxa aplicada sobre todos os produtos.

**Input:**
```json
{
  "products": [
    { "name": "Teclado Mecânico", "quantity": 2, "price": 250.00 },
    { "name": "Mouse Gamer", "quantity": 1, "price": 180.00 }
  ],
  "type": "Express"
}
```

**Output (200):**
```json
{
  "id": "a1b2c3d4-...",
  "products": [
    {
      "id": "p1e2f3a4-...",
      "name": "Teclado Mecânico",
      "quantity": 2,
      "originalUnitPrice": 250.00,
      "effectiveUnitPrice": 287.50,
      "rate": 0.15,
      "delta": 75.00,
      "totalPrice": 575.00
    },
    {
      "id": "p5e6f7a8-...",
      "name": "Mouse Gamer",
      "quantity": 1,
      "originalUnitPrice": 180.00,
      "effectiveUnitPrice": 207.00,
      "rate": 0.15,
      "delta": 27.00,
      "totalPrice": 207.00
    }
  ],
  "type": "Express",
  "initialPrice": 680.00,
  "effectivePrice": 782.00,
  "createdAt": "2026-05-21T22:00:00Z"
}
```

**Output (400):** Quando a validação falha (ex: produtos vazio, nome inválido, quantidade ou preço <= 0).
```json
["Products list cannot be null or empty"]
```

---

## GET /orders/{id}

Obtém os dados de um pedido existente.

**Output (200):** Mesma estrutura do POST, com o campo adicional `updatedAt`.
```json
{
  "id": "a1b2c3d4-...",
  "products": [ ... ],
  "type": "Express",
  "initialPrice": 680.00,
  "effectivePrice": 782.00,
  "createdAt": "2026-05-21T22:00:00Z",
  "updatedAt": null
}
```

**Output (204):** Pedido não encontrado.

---

## PUT /orders/{orderId}/items/{itemId}

Altera a quantidade de um produto existente no pedido. Os preços são recalculados automaticamente.

**Input:**
```json
{
  "quantity": 5
}
```

**Output (200):** Pedido atualizado com os novos valores.
```json
{
  "id": "a1b2c3d4-...",
  "products": [ ... ],
  "type": "Express",
  "initialPrice": 1430.00,
  "effectivePrice": 1644.50,
  "createdAt": "2026-05-21T22:00:00Z",
  "updatedAt": "2026-05-21T22:05:00Z"
}
```

**Output (400):** Quantidade inválida (<= 0).

---

## DELETE /orders/{orderId}/items/{itemId}

Remove um produto do pedido. Os preços são recalculados automaticamente.

**Output (200):** Pedido sem o produto removido.
```json
{
  "id": "a1b2c3d4-...",
  "products": [
    {
      "id": "p5e6f7a8-...",
      "name": "Mouse Gamer",
      "quantity": 1,
      "originalUnitPrice": 180.00,
      "effectiveUnitPrice": 207.00,
      "rate": 0.15,
      "delta": 27.00,
      "totalPrice": 207.00
    }
  ],
  "type": "Express",
  "initialPrice": 207.00,
  "effectivePrice": 207.00,
  "createdAt": "2026-05-21T22:00:00Z",
  "updatedAt": "2026-05-21T22:10:00Z"
}
```

---

## Tipos de Pedido (`EOrderType`)

| Input (string) | Input (int) | Efeito |
|----------------|-------------|--------|
| `"Standard"` | `0` | `rate = 0.0` — preço sem alteração |
| `"Express"` | `1` | `rate = 0.15` — acréscimo de 15% |
| `"Subscription"` | `2` | `rate = -0.10` — desconto de 10% |

**Fórmula:** `effectivePrice = initialPrice * (1 + rate)`

---

## Estrutura do Projeto

```
src/
├── LiveCoding.Domain/          # Entidades (Order, Product) e enums (EOrderType)
├── LiveCoding.Application/     # Use cases, services, validações (FluentValidation), DTOs
├── LiveCoding.Infrastructure/  # EF Core (SQLite in-memory), repositórios
└── LiveCoding.WebApi/          # Controllers, Program.cs, DI, Swagger
tests/
├── LiveCoding.UnitTests/       # Testes unitários com xUnit + Moq
└── LiveCoding.FunctionalTests/ # Testes funcionais com WebApplicationFactory
```

---

## Testes

### Unitários (21 testes — xUnit + Moq)

Testam cada camada isoladamente com dependências mockadas.

| Camada | Arquivo | Cenários |
|--------|---------|----------|
| Controllers | `OrderControllerTests.cs` | Retorno Ok, BadRequest e exceção |
| Services | `OrderServiceTests.cs` | CRUD completo, 3 tipos de pedido, erro e exceção |
| Repositories | `OrderRepositoryTests.cs` | Get e Save com SQLite real |
| Use Cases | `*UseCaseTests.cs` | Validação falha e serviço com sucesso |

### Funcionais (14 testes — xUnit + WebApplicationFactory)

Sobem a aplicação completa em memória com SQLite real. Sem mock, cada teste cria dados via requisições HTTP e valida as respostas.

| # | Teste | Fluxo |
|---|-------|-------|
| 1 | `CreateOrder_WithStandardType` | POST Standard → valida `rate=0`, `initialPrice == effectivePrice` |
| 2 | `CreateOrder_WithExpressType` | POST Express → valida `rate=0.15`, delta e `effectivePrice` |
| 3 | `CreateOrder_WithSubscriptionType` | POST Subscription → valida `rate=-0.10`, delta e `effectivePrice` |
| 4 | `CreateOrder_WithMultipleProducts` | POST Express com 3 produtos → valida soma |
| 5 | `CreateOrder_WithEmptyProducts` | POST sem produtos → 400 BadRequest |
| 6 | `GetOrder_WithExistingId` | POST → GET → valida dados consistentes |
| 7 | `GetOrder_WithNonExistentId` | GET aleatório → 204 NoContent |
| 8 | `ChangeProductQuantity_OnExistingProduct` | POST → PUT → valida qtd alterada e preço recalculado |
| 9 | `ChangeProductQuantity_WithInvalidQuantity` | POST → PUT qtd=0 → 400 BadRequest |
| 10 | `RemoveOrderProduct_OnExistingProduct` | POST com 2 itens → DELETE 1 → valida remoção e recálculo |
| 11 | `RemoveOrderProduct_WithNonExistentOrder` | DELETE aleatório → 204 NoContent |
| 12 | `FullFlow_Standard` | POST → PUT → DELETE → GET → consistência completa |
| 13 | `FullFlow_Express` | POST Express → PUT → valida markup de 15% |
| 14 | `FullFlow_Subscription` | POST Subscription → PUT → valida desconto de 10% |

**Total: 35 testes (21 unitários + 14 funcionais)**

---

## Como Executar

### Rodar a API

```bash
dotnet run --project src/LiveCoding.WebApi
```

A API sobe em `http://localhost:5000`. Acesse `http://localhost:5000/swagger` para testar os endpoints via Swagger UI.

### Rodar todos os testes

```bash
dotnet test
```

### Rodar apenas os testes unitários

```bash
dotnet test tests/LiveCoding.UnitTests
```

### Rodar apenas os testes funcionais

```bash
dotnet test tests/LiveCoding.FunctionalTests
```