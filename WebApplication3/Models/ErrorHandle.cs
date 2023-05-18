using System.Text.Json;

namespace WebApplication3.Models
{
    public class ErrorHandle
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    };
}
