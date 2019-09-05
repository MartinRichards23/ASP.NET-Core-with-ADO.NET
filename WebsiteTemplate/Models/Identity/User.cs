using System;

namespace WebsiteTemplate.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public virtual bool LockoutEnabled { get; set; } = true;
        public virtual DateTimeOffset? LockoutEnd { get; set; }
        public virtual int AccessFailedCount { get; set; }

        public UserConfig Config { get; set; }

        public override string ToString()
        {
            return UserName ?? Email;
        }

        public static User MakeDefault(string email)
        {
            User user = new User()
            {
                Email = email,
                UserName = email,
            };

            return user;
        }
    }

    public class UserInfo
    {
        public User User { get; set; }
        public int LoginCount { get; set; }
    }
}
