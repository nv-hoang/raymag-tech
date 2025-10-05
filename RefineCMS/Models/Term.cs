using System.ComponentModel.DataAnnotations.Schema;

namespace RefineCMS.Models;

[Table("Terms")]
public class Term : BaseModel
{
    [Column("Name")]
    public string Name { get; set; } = string.Empty;

    [Column("Slug")]
    public string Slug { get; set; } = string.Empty;

    [Column("Description")]
    public string Description { get; set; } = string.Empty;

    [Column("ParentId")]
    public int ParentId { get; set; } = 0;

    [Column("Order")]
    public int Order { get; set; } = 0;

    [Column("TaxonomyId")]
    public int TaxonomyId { get; set; } = 0;

    [Column("Status")]
    public int Status { get; set; } = 1;
}