public class ActivityLog : BaseEntity
{
    public string UserId { get; set; }
    public string Action { get; set; }
    public string Description { get; set; }
    public virtual AppUser User { get; set; }
}
