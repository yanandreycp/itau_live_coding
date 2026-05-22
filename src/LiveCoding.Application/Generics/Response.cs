namespace LiveCoding.Application.Generics
{
    public class Response<T>
    {
        public Response()
        {
            Errors = new List<string>();
        }

        public Response(bool isSuccess) : this()
        {
            IsSuccess = isSuccess;
        }

        public Response(bool isSuccess, T content) : this()
        {
            IsSuccess = isSuccess;
            Content = content;
        }

        public bool IsSuccess { get; set; }
        public T Content { get; set; }
        public List<string> Errors { get; set; }

        public Response<T> AddError(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                Errors.Add(error);
            }
            return this;
        }

        public Response<T> AddErrors(List<string> errors)
        {
            if (errors?.Count > 0)
            {
                Errors.AddRange(errors);
            }
            return this;
        }
    }
}