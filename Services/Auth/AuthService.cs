using Supabase;
using Supabase.Gotrue;
using uniquead_App.Models;
using BCrypt.Net;
using uniquead_App.Services.authState;
using System.Text.Json;

namespace uniquead_App.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly Supabase.Client _supabaseClient;

        public event Action? AuthStateChanged;

        // public User? User => _supabaseClient.Auth.CurrentUser;
        public AuthService()
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };
            _supabaseClient = new Supabase.Client("https://fdrgzitfconvsemozgkv.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZkcmd6aXRmY29udnNlbW96Z2t2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQxODE4MTcsImV4cCI6MjA1OTc1NzgxN30.uWwnlIdw7WJHNFkiHVtECVKCecrfvnMmZwzKNtpieM4", options);

        }

        public async Task<Session?> SignUpAsync(RegisterModel model)
        {
            try
            {
                var signUpResponse = await _supabaseClient.Auth.SignUp(model.Email, model.password);
                var userId = signUpResponse.User.Id;
                if(userId != null)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.password);

                    // Step 2: Insert additional profile data into `profiles` table
                    var profileData = new RegisterModel
                    {
                        Id = Guid.NewGuid(),  // Foreign key from auth.users  Guid.Parse(userId)
                        Name = model.Name,
                        PhoneNumber = model.PhoneNumber,
                        District = model.District,
                        Address = model.Address,
                        Profession = model.Profession,
                        Date = model.Date,
                        Email = model.Email,
                        password = hashedPassword,
                        userId = userId

                    };

                    var response = await _supabaseClient.From<RegisterModel>().Insert(profileData);
                   // return response != null;
                }
                return signUpResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignUp Error: {ex.Message}");
                return null;
            }
        }

        public async Task<RegisterResponse?> SignUpAsyncWithResponse(RegisterModel model)
        {
            var registersession = new RegisterResponse();
            try
            {
                var signUpResponse = await _supabaseClient.Auth.SignUp(model.Email, model.password);
                var userId = signUpResponse.User.Id;
                if (userId != null)
                {

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.password);

                    // Step 2: Insert additional profile data into `profiles` table
                    var profileData = new RegisterModel
                    {
                        Id = Guid.NewGuid(),  // Foreign key from auth.users  Guid.Parse(userId)
                        Name = model.Name,
                        PhoneNumber = model.PhoneNumber,
                        District = model.District,
                        Address = model.Address,
                        Profession = model.Profession,
                        Date = model.Date,
                        Email = model.Email,
                        password = hashedPassword,
                        userId = userId

                    };

                    var response = await _supabaseClient.From<RegisterModel>().Insert(profileData);
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignIn Error: {ex.Message}");
                registersession.Message = ex.Message;

                try
                {
                    using var doc = JsonDocument.Parse(ex.Message);
                    if (doc.RootElement.TryGetProperty("msg", out var msgElement))
                    {
                        registersession.Message = msgElement.GetString();
                    }
                }
                catch
                {
                    // If not valid JSON, keep the original exception message
                }
            }
            return registersession;
        }

        public async Task<Session?> SignInAsync(Login login)
        {
            try
            {
                var session = await _supabaseClient.Auth.SignIn(login.Email, login.Password);
                
                Console.WriteLine("Succefully login");
                return session;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignIn Error: {ex.Message}");
                return null;
            }
        }

        public async Task<LoginResponse?> SignInAsyncWithResponse(Login login)
        {
                var loginSession = new LoginResponse();
            try
            {
                loginSession.session = await _supabaseClient.Auth.SignIn(login.Email, login.Password);
                NotifyAuthStateChanged();

                Console.WriteLine("Succesfully login");
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignIn Error: {ex.Message}");
                loginSession.Message = ex.Message;

                try
                {
                    using var doc = JsonDocument.Parse(ex.Message);
                    if (doc.RootElement.TryGetProperty("msg", out var msgElement))
                    {
                        loginSession.Message = msgElement.GetString();
                    }
                }
                catch
                {
                    // If not valid JSON, keep the original exception message
                }
            }
                 return loginSession;
        }

        public async Task SignOutAsync()
        {
            try
            {
                await _supabaseClient.Auth.SignOut();
                //await _supabaseClient.Auth.RetrieveSessionAsync();
                NotifyAuthStateChanged();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignOut Error: {ex.Message}");
            }
        }

        public async Task<User?> GetCurrentUser()
        {
            var currentUserSession =  _supabaseClient.Auth.CurrentUser;
            return currentUserSession;
        }

        public async Task<RegisterModel?> GetAdmin()
        {
            var admin = new RegisterModel();
            var user = _supabaseClient.Auth.CurrentUser;
            
            if(user != null)
            {
            var getAdmin = await _supabaseClient.From<RegisterModel>().Where(i => i.Role == "admin" && i.Email == user.Email).Get();
                admin = getAdmin.Model;
            }
            return admin;
            
        }



        public void NotifyAuthStateChanged()
        {
            AuthStateChanged?.Invoke();
        }

    }
}
