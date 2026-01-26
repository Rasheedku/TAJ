using Taj.BuildingCostApp.Models;
using Taj.BuildingCostApp.Utilities;

namespace Taj.BuildingCostApp.Services;

public class UserService
{
    private readonly JsonStorageService _storage;
    public UsersData UsersData { get; private set; } = new();

    public UserService(JsonStorageService storage)
    {
        _storage = storage;
    }

    public async Task InitializeAsync()
    {
        UsersData = await _storage.LoadAsync("users.json", CreateDefaultUsers);
        if (UsersData.Users.Count == 0)
        {
            UsersData = CreateDefaultUsers();
            await SaveAsync();
        }
    }

    public Task SaveAsync() => _storage.SaveAsync("users.json", UsersData);

    public UserRecord? Authenticate(string username, string password)
    {
        var user = UsersData.Users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.IsActive);
        if (user == null)
        {
            return null;
        }

        return PasswordHasher.Verify(password, user.PasswordHash, user.PasswordSalt) ? user : null;
    }

    public UserRecord CreateUser(string username, string password, UserRole role)
    {
        var (hash, salt) = PasswordHasher.HashPassword(password);
        var record = new UserRecord
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = role,
            IsActive = true
        };
        UsersData.Users.Add(record);
        return record;
    }

    public void UpdatePassword(UserRecord user, string password)
    {
        var (hash, salt) = PasswordHasher.HashPassword(password);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;
    }

    public void ToggleActive(UserRecord user)
    {
        user.IsActive = !user.IsActive;
    }

    private static UsersData CreateDefaultUsers()
    {
        var (hash, salt) = PasswordHasher.HashPassword("temp1234");
        return new UsersData
        {
            Version = 1,
            Users = new List<UserRecord>
            {
                new()
                {
                    Username = "admin",
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = UserRole.Admin,
                    IsActive = true
                }
            }
        };
    }
}
