using System.Runtime.Serialization;

namespace RefineCMS.Requests;

[DataContract]
public class StorageQueryRequest
{
    [DataMember]
    public string Q { get; set; } = "index";

    [DataMember]
    public string Adapter { get; set; } = "local";

    [DataMember]
    public string Path { get; set; } = "local://";

    [DataMember]
    public string Filter { get; set; } = string.Empty;
}
