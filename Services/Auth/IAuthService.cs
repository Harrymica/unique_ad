using Supabase.Gotrue;
using uniquead_App.Models;

namespace uniquead_App.Services.Auth
{
    public interface IAuthService
    {
        public Task<Session?> SignUpAsync(RegisterModel model);
        Task<RegisterResponse?> SignUpAsyncWithResponse(RegisterModel model);

        public Task<Session?> SignInAsync(Login login);
        Task<LoginResponse?> SignInAsyncWithResponse(Login login);
        public Task SignOutAsync();
        //public Task<Session?> GetSessionAsync();
        public Task<User?> GetCurrentUser();
        Task<RegisterModel?> GetAdmin();
        ////public Task SaveSessionAsync(Session session);
    }
}

