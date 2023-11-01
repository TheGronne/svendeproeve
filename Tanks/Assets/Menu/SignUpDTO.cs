
public class SignUpDTO
{
    public string Username;
    public string LoginName;
    public string Password;

    public SignUpDTO(string username, string loginName, string password)
    {
        Username = username;
        LoginName = loginName;
        Password = password;
    }
}
