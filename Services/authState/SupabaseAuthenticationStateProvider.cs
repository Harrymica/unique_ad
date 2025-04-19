using Microsoft.AspNetCore.Components.Authorization;
using Supabase.Interfaces;
using Supabase;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace uniquead_App.Services.authState
{
    public class SupabaseAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly IJSRuntime _jsRuntime;

        public SupabaseAuthenticationStateProvider()
        {
            

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };
            _supabaseClient = new Supabase.Client("https://fdrgzitfconvsemozgkv.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZkcmd6aXRmY29udnNlbW96Z2t2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQxODE4MTcsImV4cCI6MjA1OTc1NzgxN30.uWwnlIdw7WJHNFkiHVtECVKCecrfvnMmZwzKNtpieM4", options);

        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {


            var url = "https://fdrgzitfconvsemozgkv.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZkcmd6aXRmY29udnNlbW96Z2t2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQxODE4MTcsImV4cCI6MjA1OTc1NzgxN30.uWwnlIdw7WJHNFkiHVtECVKCecrfvnMmZwzKNtpieM4";
            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = false
            };
            var supabase = new Supabase.Client(url, key, options);
            await supabase.InitializeAsync();

            await supabase.Auth.RetrieveSessionAsync();
            var session = supabase.Auth.CurrentSession;//_supabaseClient.Auth.CurrentSession;

           
            
           // var user = session?.User;
            ClaimsIdentity identity;

            if (session != null && session.User != null)
            {
                var user = session.User;
                identity = new ClaimsIdentity(new[]
                {
                 new Claim(ClaimTypes.Name, user.Email),
                 new Claim(ClaimTypes.NameIdentifier, user.Id)
             }, "supabase");
            }
            else
            {
                identity = new ClaimsIdentity();
            }


            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationState(principal);
        }

        public void NotifyAuthenticationStateChanged() =>
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
