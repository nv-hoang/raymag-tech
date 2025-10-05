using System.ComponentModel.DataAnnotations.Schema;

namespace RefineCMS.Models;

[Table("TermMeta")]
public class TermMeta : BaseModel
{
    [Column("ObjectId")]
    public required int ObjectId { get; set; }

    [Column("MetaKey")]
    public required string MetaKey { get; set; }

    [Column("MetaValue")]
    public string MetaValue { get; set; } = string.Empty;
}