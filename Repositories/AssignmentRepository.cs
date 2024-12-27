using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment>
    {
        private readonly AppDbContext _context;

        public AssignmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsWithDetailsAsync()
        {
            return await _context.Assignments
                .Include(a => a.Category)
                .Include(a => a.AssignedBy)
                .Include(a => a.AssignedTo)
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetUserAssignmentsAsync(string userId)
        {
            return await _context.Assignments
                .Include(a => a.Category)
                .Include(a => a.AssignedBy)
                .Where(a => a.AssignedToId == userId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<Assignment> GetAssignmentWithDetailsAsync(int id)
        {
            return await _context.Assignments
                .Include(a => a.Category)
                .Include(a => a.AssignedBy)
                .Include(a => a.AssignedTo)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }
    }
}
