﻿namespace ScreenRecognition.Api.Models.DbModels;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? NickName { get; set; }

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? CountryId { get; set; }

    public DateTime? Birthday { get; set; }

    public virtual Country? Country { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<History> Histories { get; } = new List<History>();

    public virtual Role? Role { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<Setting> Settings { get; } = new List<Setting>();
}
