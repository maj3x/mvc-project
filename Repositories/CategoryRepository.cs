using TaskManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithAssignmentsAsync()
        {
            return await _context.Categories
                .Include(c => c.Assignments)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

    }
}
