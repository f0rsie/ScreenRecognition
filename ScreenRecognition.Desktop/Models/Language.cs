using System;
using System.Collections.Generic;

namespace ScreenRecognition.Desktop.Models;

public partial class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? TranslatorAlias { get; set; }

    public string? Ocralias { get; set; }

    public virtual ICollection<History> HistoryInputLanguages { get; } = new List<History>();

    public virtual ICollection<History> HistoryOutputLanguages { get; } = new List<History>();

    public virtual ICollection<Setting> SettingInputLanguages { get; } = new List<Setting>();

    public virtual ICollection<Setting> SettingOutputLanguages { get; } = new List<Setting>();
}
