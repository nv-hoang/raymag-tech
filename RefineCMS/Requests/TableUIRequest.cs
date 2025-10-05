using System.Runtime.Serialization;

namespace RefineCMS.Requests;

[DataContract]
public class TableUIRequest
{
    [DataMember]
    public int Skip { get; set; } = 0;

    [DataMember]
    public int Limit { get; set; } = 20;

    [DataMember]
    public string Search { get; set; } = string.Empty;

    [DataMember]
    public List<int> DeleteIds { get; set; } = [];

    [DataMember]
    public List<int> Ids { get; set; } = [];

    [DataMember]
    public Dictionary<string, string> filter { get; set; } = [];

    [DataMember]
    public string Order { get; set; } = string.Empty;
}
