using Supabase;
using uniquead_App.Models;

namespace uniquead_App.Services.AppRepo
{
    public class AppService : IAppService
    {
        private readonly Supabase.Client _supabaseClient;
        public AppService()
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };
            _supabaseClient = new Supabase.Client("https://fdrgzitfconvsemozgkv.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZkcmd6aXRmY29udnNlbW96Z2t2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQxODE4MTcsImV4cCI6MjA1OTc1NzgxN30.uWwnlIdw7WJHNFkiHVtECVKCecrfvnMmZwzKNtpieM4", options);

        }

        public async Task<List<OrderItem>> GetOrdersAsync()
        {
            List<OrderItem> orders = new List<OrderItem>();

            var response = await _supabaseClient.From<OrderItem>().Where(i => i.Status == "Pending").Get();

            if(response != null)
            {
                orders = response.Models;
            }

            return orders;
        }

        public async Task<OrderItem> EditApplication(OrderItem model)
        {
            var response = await _supabaseClient.From<OrderItem>().Where(i => i.Id == model.Id).Update(model);
            return response.Model;
        }

        
    }
}
