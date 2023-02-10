using System;
using System.Collections.Generic;

namespace ScreenRecognition.Desktop.Models.DBModels;

public partial class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? TranslatorAlias { get; set; }

    public string? Ocralias { get; set; }
}
