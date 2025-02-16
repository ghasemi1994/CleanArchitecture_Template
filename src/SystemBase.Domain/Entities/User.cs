using SystemBase.Framework;

namespace SystemBase.Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }

    private User()
    {

    }

    public User(string userName, 
        string passwordHash, 
        string phoneNumber, 
        string email)
    {
        UserName = userName;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        Email = email;
    }

}
