using System;
using System.Collections.Generic;

namespace ScreenRecognition.Api.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
