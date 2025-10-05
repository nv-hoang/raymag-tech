using System.Runtime.Serialization;

namespace RefineCMS.Requests;

[DataContract]
public class StorageUploadRequest
{
    [DataMember]
    public string Name { get; set; } = string.Empty;

    [DataMember]
    public string Type { get; set; } = string.Empty;

    [DataMember]
    public IFormFile? File { get; set; }
}
