using Supabase.Gotrue;

namespace uniquead_App.Models
{
    public class RegisterResponse
    {
        public string Message { get; set; }
        public Session session { get; set; }
    }

    public class UserActivity
    {
        public Users User { get; set; }
        public string DotClass { get; set; }
        public int DaysAgo { get; set; }
    }
}
