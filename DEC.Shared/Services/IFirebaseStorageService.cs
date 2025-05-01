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
        Task<string> UploadFile(Stream fileStream, string fileName);
       
    }
}
