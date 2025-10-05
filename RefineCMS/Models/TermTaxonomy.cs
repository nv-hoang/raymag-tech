using System.ComponentModel.DataAnnotations.Schema;

namespace RefineCMS.Models;

[Table("TermTaxonomy")]
public class TermTaxonomy : BaseModel
{
    [Column("Name")]
    public string Name { get; set; } = string.Empty;
    [Column("PluralName")]
    public string PluralName { get; set; } = string.Empty;

    [Column("Slug")]
    public string Slug { get; set; } = string.Empty;

    [Column("Description")]
    public string Description { get; set; } = string.Empty;

    [Column("Order")]
    public int Order { get; set; } = 0;

    [Column("PostType")]
    public string PostType { get; set; } = string.Empty;

    [Column("Hierarchical")]
    public int Hierarchical { get; set; } = 0;

    [Column("Public")]
    public int Public { get; set; } = 0;

    [Column("Status")]
    public int Status { get; set; } = 1;
}