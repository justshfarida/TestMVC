namespace TestMVC
{
    public class ResponseModel<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
    }
    
}
