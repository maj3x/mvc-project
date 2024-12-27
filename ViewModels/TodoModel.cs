namespace TaskManagementSystem.ViewModels
{
    public class TodoModel : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
