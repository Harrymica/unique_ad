using Microsoft.AspNetCore.Components.Forms;
using uniquead_App.Models;

namespace uniquead_App.Services.ApplicationRepo
{
    public interface IApplicationService
    {
        Task<feedBackModel> Support_feedBack(feedBackModel feedBack);
        Task<ApplicationModel> SubmitApplication(ApplicationModel application, IBrowserFile file);
        Task<List<feedBackModel>> getFeedBackList();
        Task<List<ApplicationModel>> getApplicationList();
        Task<ApplicationModel> EditApplication(ApplicationModel model);
        Task<feedBackModel> EditFeedBack(feedBackModel model);

       Task<ApplicationModel> Deletepplication(ApplicationModel model);
        Task<List<feedBackModel>> DeleteFeedBack(feedBackModel model);
        Task<string> GetFile(string file);
        Task<List<Users>> GetUsersAsync();
        Task<bool> DeleteCategory(Categories category);
        Task<Categories> AddCategory(Categories category, IBrowserFile image);
        Task<Categories> EditCategory(Categories category, IBrowserFile? newImage = null);
        Task<List<Categories>> GetCategoryList();
    }
}