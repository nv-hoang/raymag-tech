using System.ComponentModel.DataAnnotations.Schema;

namespace RefineCMS.Models;

[Table("Posts")]
public class Post : BaseModel
{
    [Column("PostAuthor")]
    public int PostAuthor { get; set; } = 0;

    [Column("PostContent")]
    public string PostContent { get; set; } = string.Empty;

    [Column("PostTitle")]
    public string PostTitle { get; set; } = string.Empty;

    [Column("PostExcerpt")]
    public string PostExcerpt { get; set; } = string.Empty;

    [Column("PostStatus")]
    public string PostStatus { get; set; } = "publish";

    [Column("PostName")]
    public string PostName { get; set; } = string.Empty;

    [Column("ParentId")]
    public int ParentId { get; set; } = 0;

    [Column("Order")]
    public int Order { get; set; } = 0;

    [Column("Pinned")]
    public int Pinned { get; set; } = 0;

    [Column("PostType")]
    public string PostType { get; set; } = "post";

    [Column("PostTemplate")]
    public string PostTemplate { get; set; } = string.Empty;
}
