using System;
using System.Collections.Generic;

namespace ScreenRecognition.Api.Models;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<History> Histories { get; } = new List<History>();

    public virtual Role Role { get; set; } = null!;

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<Setting> Settings { get; } = new List<Setting>();
}
