using System.ComponentModel.DataAnnotations.Schema;

namespace RefineCMS.Models;

[Table("Options")]
public class Option : BaseModel
{
    [Column("OptionType")]
    public string OptionType { get; set; } = string.Empty;

    [Column("OptionName")]
    public string OptionName { get; set; } = string.Empty;

    [Column("OptionValue")]
    public string OptionValue { get; set; } = string.Empty;

    [Column("AutoLoad")]
    public bool AutoLoad { get; set; } = false;
}
