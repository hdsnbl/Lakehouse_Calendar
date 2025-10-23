namespace LakeHouseCalendarWebsite.Classes
{
    public interface IUser
    {
        string Username { get; set; }
        string Password { get; set; }
        bool Admin { get; set; }
        string Email { get; set; }
    }
}