using TimeAttendanceApp.Infrostructure.Errors;

namespace TimeAttendanceApp.Services.FileProccessingService
{
    public class FileProcessingService:IFileProcessingService
    {
        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw ServiceException.BadRequest("File is empty or null");
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
