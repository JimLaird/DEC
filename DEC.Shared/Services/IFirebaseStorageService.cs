using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;

namespace DEC.Shared.Services
{
    public interface IFirebaseStorageService
    {
        /// <summary>
        /// Uploads a file to the server and returns the URL of the uploaded file.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="fileStream"/> is positioned at the beginning of the
        /// stream before calling this method. The caller is responsible for disposing the <paramref name="fileStream"/>
        /// after the upload is complete.</remarks>
        /// <param name="fileStream">The stream containing the file data to be uploaded. Must not be null.</param>
        /// <param name="fileName">The name of the file, including its extension. Must not be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the URL of the uploaded file.</returns>
        Task<string> UploadFile(Stream fileStream, string fileName);
       
    }
}
