namespace TimeAttendanceApp.Services.FileProccessingService
{
    public interface IFileProcessingService
    {
        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
    }
}
