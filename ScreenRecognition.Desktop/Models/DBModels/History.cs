using System;
using System.Collections.Generic;

namespace ScreenRecognition.Desktop.Models.DBModels;

public partial class History
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public byte[]? Screenshot { get; set; }

    public int? SelectedOcrid { get; set; }

    public string? RecognizedText { get; set; }

    public double? RecognizedTextAccuracy { get; set; }

    public int? SelectedTranslatorId { get; set; }

    public int? InputLanguageId { get; set; }

    public int? OutputLanguageId { get; set; }

    public string? Translate { get; set; }

    public virtual Language? InputLanguage { get; set; }

    public virtual Language? OutputLanguage { get; set; }

    public virtual Ocr? SelectedOcr { get; set; }

    public virtual Translator? SelectedTranslator { get; set; }

    public virtual User User { get; set; } = null!;
}
