using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEC.Shared.Services;

namespace DEC.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(Constants.FirebaseStorageBucket).Child("uploads").Child(fileName)
                .PutAsync(fileStream, cancellation.Token);

            try
            {
                string downloadUrl = await task;
                return downloadUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                throw;
            }
        }

    }
}
