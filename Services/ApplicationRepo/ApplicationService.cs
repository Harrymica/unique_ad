using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Supabase;
using Supabase.Gotrue;
using System;
using uniquead_App.Models;

namespace uniquead_App.Services.ApplicationRepo
{
    public class ApplicationService : IApplicationService
    {
        private readonly Supabase.Client _supabaseClient;
        public ApplicationService()
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };
            _supabaseClient = new Supabase.Client("https://fdrgzitfconvsemozgkv.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZkcmd6aXRmY29udnNlbW96Z2t2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQxODE4MTcsImV4cCI6MjA1OTc1NzgxN30.uWwnlIdw7WJHNFkiHVtECVKCecrfvnMmZwzKNtpieM4", options);
        }

        public async Task<ApplicationModel> SubmitApplication(ApplicationModel application, IBrowserFile file)
        {
            application.status = "pending";
            if (file != null)
            {
               
                var bucket = _supabaseClient.Storage.From("cv-uploads");
                var imageResult = await UploadFile(file);
                application.Cv = bucket.GetPublicUrl(imageResult);
            }

            // Insert into Supabase table
            var response = await _supabaseClient.From<ApplicationModel>().Insert(application);
            return response.Model;
        } 

        public async Task<ApplicationModel> EditApplication(ApplicationModel model)
        {
            var response = await _supabaseClient.From<ApplicationModel>().Where(i => i.Id == model.Id).Update(model);
            return response.Model;
        }

        public async Task<feedBackModel> EditFeedBack(feedBackModel model)
        {
            var response = await _supabaseClient.From<feedBackModel>().Where(i => i.Id == model.Id).Update(model);
            return response.Model;
        }
        public async Task<ApplicationModel> Deletepplication(ApplicationModel model)
        {

            await _supabaseClient.InitializeAsync();
            ApplicationModel  remod = new ApplicationModel();
            if (model != null)
            {
                //TimeSpan timeSpan = new TimeSpan();
                //var fileName = Path.GetFileName(new Uri(model.Cv).AbsolutePath);
                var bucket =  _supabaseClient.Storage.From("cv-uploads");
                //string path = "https://fdrgzitfconvsemozgkv.supabase.co/storage/v1/object/public/cv-uploads//660c667f-4b99-4565-8e34-ae5727b590ee.pdf";

                //await _supabaseClient.Storage.From("cv-uploads").Remove(model.Cv);
                //var result2 =  _supabaseClient.Storage.From("cv-uploads").Remove(model.Cv);

                try
                {
                    var result2 = await _supabaseClient.Storage.From("cv-uploads").Remove(new List<string> { model.Cv });
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                /*try
                {

                    //var delResp = await bucket.Remove(new List<string> { model.Cv });
                    result.WaitAsync(TimeSpan.FromSeconds(12));
                    if (result.IsCompleted)
                    {

                    Console.WriteLine($"Deleted successfully");
                    }
                    else
                    {
                        Console.WriteLine($"{result.Exception.Message}");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file: {ex.Message}, {result.Exception.Message}");
                }*/

               /* if (result2.IsCompletedSuccessfully)
                {
                    var response = await _supabaseClient.From<ApplicationModel>().Where(i => i.Id == model.Id).Delete(model);
                    remod = response.Model;
                }*/
                var relativePath = new Uri(model.Cv).AbsolutePath.Replace("/storage/v1/object/public/cv-uploads/", "");

                /*try
                {
                    var result = await bucket.Remove(new List<string> { relativePath });

                    if (result.Count > 0)
                    {
                        Console.WriteLine("File deleted successfully.");

                        // Now delete the record from the database
                        var response = await _supabaseClient.From<ApplicationModel>()
                            .Where(i => i.Id == model.Id)
                            .Delete(model);

                        remod = response.Model;
                    }
                    else
                    {
                        Console.WriteLine("File was not deleted. Possibly not found or no permission.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file: {ex.Message}");
                }*/

                //var relativePath = new Uri(model.Cv).AbsolutePath.Replace("/storage/v1/object/public/cv-uploads/", "");
                //await bucket.Remove(relativePath);

            }


            return remod;
        }
        public async Task<List<feedBackModel>> DeleteFeedBack(feedBackModel model)
        {
            List<feedBackModel> feedBack = new List<feedBackModel>();
            if (model != null)
            {
                var response = await _supabaseClient.From<feedBackModel>().Where(i => i.Id == model.Id).Delete(model);
                feedBack = response.Models;
            }
            return feedBack;
        }

        public async Task<string> GetFile(string file)
        {
            var bucket =  _supabaseClient.Storage.From("cv-uploads");
            var fileName = Path.GetFileName(new Uri(file).AbsolutePath);
            var fileUrl =  bucket.GetPublicUrl(fileName);

            return fileUrl.ToString();
        }
        public async Task<List<ApplicationModel>> getApplicationList()
        {
            var response = await _supabaseClient.From<ApplicationModel>().Get();

            if (response.Model == null)
            {
                return null;
            }

            return response.Models;
        }

        public async Task<feedBackModel> Support_feedBack(feedBackModel feedBack)
        { 
                var response = await _supabaseClient.From<feedBackModel>().Insert(feedBack);
                return response.Model;
        }
        public async Task<List<feedBackModel>> getFeedBackList()
        {
            var response = await _supabaseClient.From<feedBackModel>().Get();

            if (response.Model == null)
            {
                return null;
            }

            return response.Models;
        }
        public async Task<string> UploadFile(IBrowserFile file)
        {
            string bucketName;
           
            bucketName = "cv-uploads";

            try
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";

                using var memoryStream = new MemoryStream();
                await file.OpenReadStream(maxAllowedSize: 10485760).CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                var result = await _supabaseClient.Storage
                    .From(bucketName)
                    .Upload(fileBytes, fileName);

                if (result == null)
                {
                    throw new Exception("Error uploading image: Upload result is null");
                }

                Console.WriteLine($"Image uploaded successfully. File name: {fileName}");

                // Get the public URL of the uploaded file
                var publicUrl = _supabaseClient.Storage
                    .From(bucketName)
                    .GetPublicUrl(fileName);

                Console.WriteLine($"Public URL: {publicUrl}");
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Users>> GetUsersAsync()
        {
            var response = await _supabaseClient.From<Users>().Get();

            if (response.Model == null)
            {
                return null;
            }

            return response.Models;
        }

        public async Task<Categories> AddCategory(Categories category, IBrowserFile image)
        {
            if (image != null)
            {
                string imageFileName = await UploadFileToCategoryBucket(image);
                category.ImageUrl = _supabaseClient.Storage.From("category").GetPublicUrl(imageFileName);
            }

            var response = await _supabaseClient.From<Categories>().Insert(category);
            return response.Model;
        }

        public async Task<Categories> EditCategory(Categories category, IBrowserFile? newImage = null)
        {
            if (newImage != null)
            {
                string newFileName = await UploadFileToCategoryBucket(newImage);
                category.ImageUrl = _supabaseClient.Storage.From("category").GetPublicUrl(newFileName);
            }

            var response = await _supabaseClient
                .From<Categories>()
                .Where(x => x.Id == category.Id)
                .Update(category);

            return response.Model;
        }

        /* public async Task<bool> DeleteCategory(Categories category)
        {
            try
            {
                var imageDeleted = true;

                if (!string.IsNullOrWhiteSpace(category.ImageUrl))
                {
                    var relativePath = new Uri(category.ImageUrl).AbsolutePath.Replace("/storage/v1/object/sign/category/", "");
                    try
                    {
                        var response = await _supabaseClient.Storage.From("category").Remove(new List<string> { relativePath });
                        imageDeleted = response != null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Image deletion failed: {ex.Message}");
                        imageDeleted = false;
                    }
                }

                if (imageDeleted)
                {
                    var result = await _supabaseClient.From<Categories>().Where(x => x.Id == category.Id).Delete(category);
                    return result != null;
                }
                else
                {
                    Console.WriteLine("Image was not deleted. Category will not be deleted.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false;
            }
        } */

        /*public async Task<bool> DeleteCategory(Categories category)
        {
            try
            {
                var imageDeleted = true;

                // category.ImageUrl now contains only the file name (e.g., "abc.jpg")
                if (!string.IsNullOrWhiteSpace(category.ImageUrl))
                {
                    try
                    {
                        var response = await _supabaseClient.Storage
                            .From("category")
                            .Remove(new List<string> { category.ImageUrl });

                        imageDeleted = response != null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Image deletion failed: {ex.Message}");
                        imageDeleted = false;
                    }
                }

                if (imageDeleted)
                {
                    var result = await _supabaseClient
                        .From<Categories>()
                        .Where(x => x.Id == category.Id)
                        .Delete(category);

                    return result != null;
                }
                else
                {
                    Console.WriteLine("Image was not deleted. Category will not be deleted.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false;
            }
        } */


        public async Task<bool> DeleteCategory(Categories category)
        {
            var listofCAt = await GetCategoryList();
            if(listofCAt != null && category != null)
            {

             var imageUrl = listofCAt.Where(i => i.Id == category.Id).FirstOrDefault().ImageUrl;

            }
            try
            {
                if (!string.IsNullOrWhiteSpace(category.ImageUrl))
                {
                    var relativePath = new Uri(category.ImageUrl).AbsolutePath
                        .Replace("/storage/v1/object/public/category/", "")
                        .Replace("/storage/v1/object/sign/category/", "");
                    
                    try
                    {

                        var response = await _supabaseClient.Storage.From("category").Remove(new List<string> { relativePath });
                        if (response != null && response.Count > 0)
                        {
                            await _supabaseClient.From<Categories>().Where(x => x.Id == category.Id).Delete(category);
                        }
                        else
                        {
                            Console.WriteLine(response);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false;
            }
        }

        public async Task<string> UploadFileToCategoryBucket(IBrowserFile file)
        {
            string bucketName = "category";

            try
            {
                //string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
                var imagePath = Path.Combine("category_images", file.Name);

                using var memoryStream = new MemoryStream();
                await file.OpenReadStream(maxAllowedSize: 10485760).CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                var result = await _supabaseClient.Storage
                    .From(bucketName)
                    .Upload(fileBytes, imagePath);

                if (result == null)
                    throw new Exception("Upload failed: result is null");

                Console.WriteLine($"Uploaded category image: {imagePath}");
                return imagePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload error: {ex.Message}");
                throw;
            }
        }

        /*public async Task<List<Categories>> GetCategoryList()
        {
            var response = await _supabaseClient.From<Categories>().Get();

            if (response.Models == null || response.Models.Count == 0)
                return new List<Categories>();

            var categories = response.Models;

            foreach (var category in categories)
            {
                if (!string.IsNullOrWhiteSpace(category.ImageUrl))
                {
                    try
                    {
                        // Generate a signed URL valid for 1 hour (3600 seconds)
                        var signedUrlResponse = await _supabaseClient.Storage
                            .From("category")
                            .CreateSignedUrl(category.ImageUrl, 3600);

                        if (signedUrlResponse != null)
                        {
                            category.ImageUrl = signedUrlResponse;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to generate signed URL: {ex.Message}");
                        category.ImageUrl = null;
                    }
                }
            }

            return categories;
        }*/


        public async Task<List<Categories>> GetCategoryList()
        {
            var response = await _supabaseClient.From<Categories>().Get();

            if (response.Model == null)
                return new List<Categories>();

            return response.Models;
        }



    }
}
