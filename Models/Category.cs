using System.Collections.Generic;

namespace TaskManagementSystem.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }

    }
}
