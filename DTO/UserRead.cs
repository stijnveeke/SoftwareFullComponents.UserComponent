namespace SoftwareFullComponent.UserComponent.DTO
{
    public class UserRead
    {
        public string user_id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public bool email_verified { get; set; }
        public bool blocked { get; set; }
    }
}