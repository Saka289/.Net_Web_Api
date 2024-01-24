namespace DOTNET_RPG.Models
{
    public class ServiceRespone<T>
    {
        public T? Data { get; set; }

        public bool Success { get; set; } = true;

        public string Message { get; set; } = string.Empty;
    }
}