using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RefineCMS.Requests;

[DataContract]
public class FormUIRequest
{
    [DataMember]
    [Required]
    public required string FormData { get; set; }

    [DataMember]
    [Required]
    public required string SubmitAction { get; set; }

    public JObject GetData()
    {
        return JObject.Parse(FormData);
    }
}
