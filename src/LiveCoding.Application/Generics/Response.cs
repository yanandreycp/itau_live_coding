namespace LiveCoding.Application.Generics
{
    public class Response<T>
    {
        public Response()
        {

        }

        public Response(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public Response(bool isSuccess, T content)
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
    }
}