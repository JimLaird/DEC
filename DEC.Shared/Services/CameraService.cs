using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;


namespace DEC.Shared.Services
{
    public class CameraService
    {
        public async Task<FileResult?> CapturePhotoAsync()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var result = await MediaPicker.Default.CapturePhotoAsync();
                if (result is null)
                {
                    throw new InvalidOperationException("CapturePhotoAsync returned null.");
                }
                return result;
            }
            throw new NotSupportedException("Camera capture is not supported on this device.");
        }

        public async Task<FileResult?> PickPhotoAsync()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var result = await MediaPicker.Default.PickPhotoAsync();
                if (result is null)
                {
                    throw new InvalidOperationException("PickPhotoAsync returned null.");
                }
                return result;
            }
            throw new NotSupportedException("Photo picking is not supported on this device.");
        }
    }
}
