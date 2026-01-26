namespace Taj.BuildingCostApp.Models;

public enum UserRole
{
    Admin,
    User
}

public class UserRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public bool IsActive { get; set; } = true;
}

public class UsersData
{
    public int Version { get; set; } = 1;
    public List<UserRecord> Users { get; set; } = new();
}
