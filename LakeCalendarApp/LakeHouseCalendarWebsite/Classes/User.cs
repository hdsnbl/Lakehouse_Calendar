namespace LakeHouseCalendarWebsite.Classes
{
    public class User : IUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
        public string Email { get; set; }

        public User(string username, string password) 
        {
            Username = username;
            Password = password;
        }
        public User()
        {
            Username = "";
            Password = "";
            Admin = false;
        }

    }
}