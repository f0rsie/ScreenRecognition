namespace ScreenRecognition.Api.Models.DbModels;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
