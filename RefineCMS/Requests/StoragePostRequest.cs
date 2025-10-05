using System.Runtime.Serialization;

namespace RefineCMS.Requests;

public class StoragePostItem
{
    public string Type { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}

[DataContract]
public class StoragePostRequest
{
    [DataMember]
    public string Name { get; set; } = string.Empty;

    [DataMember]
    public string Item { get; set; } = string.Empty;

    [DataMember]
    public List<StoragePostItem> Items { get; set; } = [];
}
