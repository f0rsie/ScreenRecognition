using System;
using System.Collections.Generic;

namespace ScreenRecognition.Api.Models;

public partial class Setting
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public int? SelectedOcrid { get; set; }

    public int? SelectedTranslatorId { get; set; }

    public string? TranslatorApiKey { get; set; }

    public int? InputLanguageId { get; set; }

    public int? OutputLanguageId { get; set; }

    public string? ResultColor { get; set; }

    public virtual Language? InputLanguage { get; set; }

    public virtual Language? OutputLanguage { get; set; }

    public virtual Ocr? SelectedOcr { get; set; }

    public virtual Translator? SelectedTranslator { get; set; }

    public virtual User? User { get; set; } = null!;
}
