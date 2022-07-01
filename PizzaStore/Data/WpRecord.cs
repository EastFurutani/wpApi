using System.ComponentModel.DataAnnotations.Schema;

[Table("WPDapper")]
public record Wp(int id, string wordName, string wordType);