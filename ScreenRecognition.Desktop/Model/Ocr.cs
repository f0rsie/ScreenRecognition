using System;
using System.Collections.Generic;

namespace ScreenRecognition.Desktop.Models;

public partial class Ocr
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<History> Histories { get; } = new List<History>();

    public virtual ICollection<Setting> Settings { get; } = new List<Setting>();
}
