using System;

namespace TaskManagementSystem.Models
{
    public class Assignment : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string AssignedById { get; set; }
        public virtual AppUser AssignedBy { get; set; }
        public string AssignedToId { get; set; }
        public virtual AppUser AssignedTo { get; set; }
        public AssignmentStatus Status { get; set; }
    }

    public enum AssignmentStatus
    {
        Pending,
        InProgress,
        Completed,
        Late
    }
}
