using System.IO.Pipelines;

namespace WebApplication3.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; } = String.Empty;
        public string? Description { get; set; }

        public List<Pie>? Pies { get; set; }
    }
}
