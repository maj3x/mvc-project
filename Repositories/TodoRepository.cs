using TaskManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Repositories
{
    public class TodoRepository : GenericRepository<Todo>
    {
        private readonly AppDbContext _context;

        public TodoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Todo>> GetUserTodosAsync(string userId)
        {
            return await _context.Todos
                .Include(t => t.User)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<Todo> GetTodoWithUserByIdAsync(int id)
        {
            return await _context.Todos
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }
    }
}
