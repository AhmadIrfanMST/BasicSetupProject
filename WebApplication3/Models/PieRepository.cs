using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly MyDbContext _context;
        public PieRepository(MyDbContext cont)
        {
            _context = cont;
        }
        public IEnumerable<Pie> AllPies
        {
            get
            {
                return _context.pies.Include(c => c.Category);
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return _context.pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        public Pie? GetPieById(int pieId)
        {
            return _context.pies.FirstOrDefault(p => p.PieId == pieId);
        }
    }
}
