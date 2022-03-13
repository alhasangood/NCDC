using Common;

namespace Portal.Services;

public class FileService

{
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> UplaodFileAsync(string directoryPath, string fileName, IFormFile file)
    {
        try
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, $"{fileName}.{file.ContentType.Split("/").Last()}");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }


            using var ms = new MemoryStream();

            file.CopyTo(ms);

            using var stream = File.Create(filePath);


            var fileBytes = ms.ToArray();

            string fileString = Convert.ToBase64String(fileBytes);

            fileString = Security.EncryptString(fileString, Security.MasterKey);

            fileBytes = Convert.FromBase64String(fileString);

            await stream.WriteAsync(fileBytes);



            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
            return false;
        }
    }

    public async Task<byte[]?> ReadFileAsync(string directoryPath, string fileName)
    {
        try
        {
            var filePath = Path.Combine(directoryPath, fileName);

            if (!File.Exists(filePath))
            {
                return null;
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath);

            string fileString = Convert.ToBase64String(fileBytes);

            fileString = Security.DecryptString(fileString, Security.MasterKey);

            fileBytes = Convert.FromBase64String(fileString);

            return fileBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
            return null;
        }
    }
}