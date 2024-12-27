namespace TaskManagementSystem.Models
{
    public class Todo : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
