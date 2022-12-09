using System;
using System.Collections.Generic;

namespace ScreenRecognition.Api.Models;

public partial class Translator
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<History> Histories { get; } = new List<History>();

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<Setting> Settings { get; } = new List<Setting>();
}
