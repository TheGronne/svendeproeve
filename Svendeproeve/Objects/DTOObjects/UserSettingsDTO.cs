namespace Svendeproeve.Objects.DTOObjects
{
    public class UserSettingsDTO
    {
        public string Username { get; set; }
        public string NewLoginName { get; set; }
        public string NewPassword { get; set; }
        public string OldLoginName { get; set; }
        public string OldPassword { get; set; }

        public UserSettingsDTO(string username, string newLoginName, string newPassword, string oldLoginName, string oldPassword)
        {
            Username = username;
            NewLoginName = newLoginName;
            NewPassword = newPassword;
            OldLoginName = oldLoginName;
            OldPassword = oldPassword;
        }
    }
}
