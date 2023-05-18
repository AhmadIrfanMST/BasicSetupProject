namespace WebApplication3.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDbContext _context;
        public CategoryRepository(MyDbContext cont)
        {
            _context = cont;
        }

        public IEnumerable<Category> AllCategories
        {
            get
            {
                return _context.categories;
            }
        }
    }
}
