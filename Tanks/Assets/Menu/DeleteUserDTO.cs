using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUserDTO : MonoBehaviour
{
    public string LoginName { get; set; }
    public string Password { get; set; }

    public DeleteUserDTO(string loginName, string password)
    {
        LoginName = loginName;
        Password = password;
    }
}
