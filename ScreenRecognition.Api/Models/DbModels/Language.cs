using System;
using System.Collections.Generic;

namespace ScreenRecognition.Api.Models.DbModels;

public partial class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? TranslatorAlias { get; set; }

    public string? Ocralias { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<History> HistoryInputLanguages { get; } = new List<History>();

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<History> HistoryOutputLanguages { get; } = new List<History>();

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<Setting> SettingInputLanguages { get; } = new List<Setting>();

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<Setting> SettingOutputLanguages { get; } = new List<Setting>();
}
