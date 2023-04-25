namespace Project.ViewModels
{
    public class UserViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public IList<string>? UserRoles { get; set; }
    }
}
