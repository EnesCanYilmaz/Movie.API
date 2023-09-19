namespace MovieAPI.FileRename;

public interface IFileService
{
    Task<string> FileRenameAsync(string path, string fileName, bool first = true);
    Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files);
    Task DeleteAsync(string path, string fileName);
    List<string> GetFiles(string path);
    Task<bool> CopyFileAsync(string path, IFormFile file);
    bool HasFile(string path, string fileName);
}