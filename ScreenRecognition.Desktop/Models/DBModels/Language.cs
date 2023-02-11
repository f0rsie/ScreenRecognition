namespace ScreenRecognition.Desktop.Models.DbModels;

public partial class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? TranslatorAlias { get; set; }

    public string? Ocralias { get; set; }
}
