using ECommerceAPI.Infrastructure.Operations;

namespace MovieAPI.FileRename;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task DeleteAsync(string path, string fileName)
        => File.Delete($"{path}/{fileName}");

    public List<string> GetFiles(string path)
    {
        DirectoryInfo directory = new(path);
        return directory.GetFiles().Select(f => f.Name).ToList();
    }
    public async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool HasFile(string path, string fileName)
        => File.Exists($"{path}\\{fileName}");

    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
    {
        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        List<(string fileName, string pathOrContainerName)> datas = new();
        foreach (var file in files)
        {
            var fileNewName = await FileRenameAsync(uploadPath, file.FileName, HasFile(uploadPath,file.FileName));
            
            await CopyFileAsync($"{uploadPath}/{fileNewName}", file);
            datas.Add((fileNewName, $"{path}/{file.FileName}"));
        }

        return datas;
    }
    
    public async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
    {
        var newFileName = await Task.Run<string>(async () =>
        {
            var extension = Path.GetExtension(fileName);
            var newFileName = string.Empty;
            if (first)
            {
                var oldName = Path.GetFileNameWithoutExtension(fileName);
                newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
            }
            else
            {
                newFileName = fileName;
                var indexNo1 = newFileName.IndexOf('-');
                if (indexNo1 == -1)
                    newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                else
                {
                    var lastIndex = 0;
                    while (true)
                    {
                        lastIndex = indexNo1;
                        indexNo1 = newFileName.IndexOf("-", indexNo1 + 1);
                        if (indexNo1 != -1) continue;
                        indexNo1 = lastIndex;
                        break;
                    }


                    var indexNo2 = newFileName.IndexOf(".", StringComparison.Ordinal);
                    var fileNo = newFileName.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);

                    if (int.TryParse(fileNo, out int _fileNo))
                    {
                        _fileNo++;
                        newFileName = newFileName.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                            .Insert(indexNo1 + 1, _fileNo.ToString());
                    }
                    else
                    {
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    }
                }
            }
            if (File.Exists($"{path}\\{newFileName}"))
                return await FileRenameAsync(path, newFileName, false);
            else
                return newFileName;
        });
        return newFileName;
    }
}

 
