namespace FMS_Collection.Core.Common
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; } = default;
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";

        public ServiceResponse<T> Response(bool Success, string Message, T Data = default)
        {
            this.Data = Data;
            this.Success = Success;
            this.Message = Message;

            return this;
        }
    }
}
