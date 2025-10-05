using Microsoft.AspNetCore.StaticFiles;
using RefineCMS.Common;
using RefineCMS.Requests;

namespace RefineCMS.Providers;

public class StorageProvider(CFG _cfg, Output _output)
{
    public object List(string virtualPath, string filter)
    {
        var (storage, relativePath, fullPath) = ParseVirtualPath(virtualPath);
        var items = new List<object>();

        if (Directory.Exists(fullPath))
        {
            var urlPrefix = $"{_cfg.WwwRoot}/uploads";

            // Directories
            foreach (var dir in Directory.GetDirectories(fullPath))
            {
                var dirInfo = new DirectoryInfo(dir);
                var info = new
                {
                    basename = dirInfo.Name,
                    extension = "",
                    extra_metadata = Array.Empty<string>(),
                    last_modified = ToUnixTimestamp(dirInfo.LastWriteTimeUtc),
                    path = $"{storage}://{Path.Combine(relativePath, dirInfo.Name).Replace("\\", "/")}",
                    storage,
                    type = "dir",
                    visibility = "public"
                };

                if (string.IsNullOrEmpty(filter) || info.basename.Contains(filter) || info.path.Contains(filter))
                {
                    items.Add(info);
                }
            }

            // Files
            foreach (var file in Directory.GetFiles(fullPath))
            {
                var fileInfo = new FileInfo(file);
                new FileExtensionContentTypeProvider().TryGetContentType(fileInfo.Name, out var mimeType);

                var info = new
                {
                    basename = fileInfo.Name,
                    extension = fileInfo.Extension.TrimStart('.'),
                    extra_metadata = Array.Empty<string>(),
                    file_size = fileInfo.Length,
                    last_modified = ToUnixTimestamp(fileInfo.LastWriteTimeUtc),
                    mime_type = mimeType ?? "application/octet-stream",
                    path = $"{storage}://{Path.Combine(relativePath, fileInfo.Name).Replace("\\", "/")}",
                    storage,
                    type = "file",
                    url = $"{urlPrefix}/{Path.Combine(relativePath, fileInfo.Name).Replace("\\", "/")}",
                    visibility = "public"
                };

                if (string.IsNullOrEmpty(filter) || info.basename.Contains(filter) || info.path.Contains(filter))
                {
                    items.Add(info);
                }
            }
        }

        return new
        {
            adapter = storage,
            dirname = virtualPath,
            files = items,
            storages = new[] { storage }
        };
    }

    public (bool status, string message) CreateFolder(string virtualPath, string name)
    {
        var (_, _, fullPath) = ParseVirtualPath(virtualPath);

        fullPath = Path.Combine(fullPath, name.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

        if (Directory.Exists(fullPath)) return (false, _output.Trans("Folder already exists."));

        try
        {
            Directory.CreateDirectory(fullPath);
            return (true, "");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public (bool status, string message, string path) Upload(string virtualPath, IFormFile file)
    {
        var (_, relativePath, fullPath) = ParseVirtualPath(virtualPath);

        if (!Directory.Exists(fullPath)) return (false, _output.Trans("Folder does not exists"), "");

        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine(fullPath, fileName);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message, "");
        }

        var path = $"{_cfg.WwwRoot}/uploads/{Path.Combine(relativePath, fileName)}";

        return (true, "", path);
    }

    public (bool status, string message) Delete(List<StoragePostItem> Items)
    {
        try
        {
            foreach (var item in Items)
            {
                if ("dir".Equals(item.Type))
                {
                    var (_, _, fullPath) = ParseVirtualPath(item.Path);
                    if (!Directory.Exists(fullPath)) return (false, _output.Trans("Folder does not exists"));

                    Directory.Delete(fullPath, recursive: true);
                }
                else if ("file".Equals(item.Type))
                {
                    var (_, _, fullPath) = ParseVirtualPath(item.Path);
                    if (!File.Exists(fullPath)) return (false, _output.Trans("File does not exists"));

                    File.Delete(fullPath);
                }
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }

        return (true, "");
    }

    public (bool status, string message) Move(List<StoragePostItem> Items, string folderPath)
    {
        var (_, _, targetPath) = ParseVirtualPath(folderPath);

        if (!Directory.Exists(targetPath)) return (false, _output.Trans("Folder does not exists"));

        try
        {
            foreach (var item in Items)
            {
                if ("dir".Equals(item.Type))
                {
                    var (_, _, fullPath) = ParseVirtualPath(item.Path);
                    if (!Directory.Exists(fullPath)) return (false, _output.Trans("Folder does not exists"));

                    var destPath = Path.Combine(targetPath, Path.GetFileName(fullPath));
                    Directory.Move(fullPath, destPath);
                }
                else if ("file".Equals(item.Type))
                {
                    var (_, _, fullPath) = ParseVirtualPath(item.Path);
                    if (!File.Exists(fullPath)) return (false, _output.Trans("File does not exists"));

                    var destPath = Path.Combine(targetPath, Path.GetFileName(fullPath));
                    File.Move(fullPath, destPath);
                }
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }

        return (true, "");
    }

    public (bool status, string message) Rename(string path, string name)
    {
        var (_, _, fullPath) = ParseVirtualPath(path);
        var parentDir = Path.GetDirectoryName(fullPath);
        var newPath = Path.Combine(parentDir!, name);

        try
        {
            if (Directory.Exists(fullPath)) Directory.Move(fullPath, newPath);
            else if (File.Exists(fullPath)) File.Move(fullPath, newPath);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }

        return (true, "");
    }

    protected (string storage, string relativePath, string fullPath) ParseVirtualPath(string virtualPath)
    {
        var parts = virtualPath.Split(["://"], 2, StringSplitOptions.None);

        var storage = string.IsNullOrEmpty(parts[0]) ? "local" : parts[0];
        var relativePath = parts.Length > 1 ? parts[1] : "";
        var fullPath = Path.Combine(_cfg.UploadDir.TrimEnd('/'), relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

        return (storage, relativePath, fullPath);
    }

    protected long ToUnixTimestamp(DateTime dt)
    {
        return new DateTimeOffset(dt).ToUnixTimeSeconds();
    }
}
