using System.ComponentModel.DataAnnotations.Schema;

namespace RefineCMS.Models;

[Table("TermRelationships")]
public class TermRelationship : BaseModel
{
    [Column("ObjectId")]
    public int ObjectId { get; set; }

    [Column("TermId")]
    public int TermId { get; set; }

    [Column("Order")]
    public int TermOrder { get; set; } = 0;
}