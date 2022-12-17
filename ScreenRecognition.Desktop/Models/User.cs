using System;
using System.Collections.Generic;

namespace ScreenRecognition.Desktop.Models;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? CountryId { get; set; }

    public DateTime? Birthday { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Role Role { get; set; } = null!;
}
