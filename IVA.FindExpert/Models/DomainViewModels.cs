public class PhoneValidateRequest
{
    public string Name { get; set; }
    public string Phone { get; set; }
}

public class LoginRequest
{
    public string Phone { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int UserType { get; set; }
}

public class UserModel
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public string Phone { get; set; }
    public long UserProfileId { get; set; }
}