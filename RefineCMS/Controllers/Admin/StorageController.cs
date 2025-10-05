using Microsoft.AspNetCore.Mvc;
using RefineCMS.Attributes;
using RefineCMS.Common;
using RefineCMS.Providers;
using RefineCMS.Requests;

namespace RefineCMS.Controllers.Admin;

[AdminController("storage")]
[Auth]
public class StorageController(AppFactory _appFactory) : BaseController(_appFactory)
{
    protected readonly StorageProvider _storageProvider = _appFactory.Create<StorageProvider>();

    [HttpGet]
    public IActionResult List([FromQuery] StorageQueryRequest query)
    {
        return Json(_storageProvider.List(query.Path, query.Filter));
    }

    [HttpPost("newfolder")]
    public IActionResult Newfolder([FromQuery] StorageQueryRequest query, [FromBody] StoragePostRequest request)
    {
        if (!string.IsNullOrEmpty(request.Name))
        {
            var (status, message) = _storageProvider.CreateFolder(query.Path, request.Name);
            if (status == false)
            {
                return BadRequest(new { status = 400, message });
            }
        }

        return List(query);
    }

    [HttpPost("upload")]
    public IActionResult Upload([FromQuery] StorageQueryRequest query, [FromForm] StorageUploadRequest? upload)
    {
        if (!(upload == null || upload.File == null || upload.File.Length == 0))
        {
            var (status, message, _) = _storageProvider.Upload(query.Path, upload!.File!);
            if (status == false)
            {
                return BadRequest(new { status = 400, message });
            }
        }

        return List(query);
    }

    [HttpPost("editor-upload")]
    public IActionResult EditorUpload([FromQuery] StorageQueryRequest query, [FromForm] StorageUploadRequest? upload)
    {
        if (!(upload == null || upload.File == null || upload.File.Length == 0))
        {
            var (status, message, path) = _storageProvider.Upload(query.Path, upload!.File!);
            if (status == true)
            {
                return Json(new { path });
            }
        }

        return Json(new { path = "" });
    }

    [HttpPost("delete")]
    public IActionResult Delete([FromQuery] StorageQueryRequest query, [FromBody] StoragePostRequest request)
    {
        var (status, message) = _storageProvider.Delete(request.Items);
        if (status == false)
        {
            return BadRequest(new { status = 400, message });
        }
        return List(query);
    }

    [HttpPost("move")]
    public IActionResult Move([FromQuery] StorageQueryRequest query, [FromBody] StoragePostRequest request)
    {
        var (status, message) = _storageProvider.Move(request.Items, request.Item);
        if (status == false)
        {
            return BadRequest(new { status = 400, message });
        }
        return List(query);
    }

    [HttpPost("rename")]
    public IActionResult Rename([FromQuery] StorageQueryRequest query, [FromBody] StoragePostRequest request)
    {
        var (status, message) = _storageProvider.Rename(request.Item, request.Name);
        if (status == false)
        {
            return BadRequest(new { status = 400, message });
        }
        return List(query);
    }
}
