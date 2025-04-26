

using uniquead_App.Models;

namespace uniquead_App.Services.AppRepo
{
    public interface IAppService
    {
        Task<List<OrderItem>> GetOrdersAsync();
        Task<OrderItem> EditApplication(OrderItem model);
    }
}