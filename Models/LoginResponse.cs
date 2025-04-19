using Supabase.Gotrue;

namespace uniquead_App.Models
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public Session session { get; set; }
    }
}
