using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSettingsDTO
{
    public string Username;
    public string NewLoginName;
    public string NewPassword;
    public string OldLoginName;
    public string OldPassword;

    public ChangeSettingsDTO(string username, string newLoginName, string newPassword, string oldLoginName, string oldPassword)
    {
        Username = username;
        NewLoginName = newLoginName;
        NewPassword = newPassword;
        OldLoginName = oldLoginName;
        OldPassword = oldPassword;
    }
}
