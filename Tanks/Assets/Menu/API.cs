using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class API : MonoBehaviour
{
    public void SignIn(string loginName, string password, Action<string> onSuccess, Action<UnityWebRequest.Result> onFailure)
    {
        StartCoroutine(Get("user/signIn", $"loginName={loginName}&password={password}", onSuccess, onFailure));
    }

    public void SignUp(SignUpDTO signUp, Action<string> onSuccess, Action<string> onFailure)
    {
        StartCoroutine(Post(signUp, "user/signUp", onSuccess, onFailure));
    }

    public void ChangeSettings(int id, ChangeSettingsDTO settings, Action<UnityWebRequest.Result> onSuccess, Action<string> onFailure)
    {
        StartCoroutine(Put("user", $"id={id}&username={settings.Username}&newLoginName={settings.NewLoginName}&newPassword={settings.NewPassword}&oldLoginName={settings.OldLoginName}&oldPassword={settings.OldPassword}", onSuccess, onFailure));
    }

    public void DeleteUser(DeleteUserDTO user, Action<UnityWebRequest.Result> onSuccess, Action<string> onFailure)
    {
        StartCoroutine(Delete("user", $"loginName={user.LoginName}&password={user.Password}", onSuccess, onFailure));
    }

    private IEnumerator Post(object payload, string url, Action<string> onSuccess, Action<string> onFailure)
    {
        UnityWebRequest www = UnityWebRequest.Post($"https://localhost:7019/api/{url}", JsonUtility.ToJson(payload), "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            onFailure(www.error);
        }
        else
        {
            onSuccess(www.downloadHandler.text);
        }
    }

    private IEnumerator Get(string url, string parameters, Action<string> onSuccess, Action<UnityWebRequest.Result> onFailure)
    {
        UnityWebRequest www = UnityWebRequest.Get($"https://localhost:7019/api/{url}?{parameters}");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            onFailure(www.result);
        }
        else
        {
            onSuccess(www.downloadHandler.text);
        }
    }

    private IEnumerator Put(string url, string parameters, Action<UnityWebRequest.Result> onSuccess, Action<string> onFailure)
    {
        UnityWebRequest www = UnityWebRequest.Put($"https://localhost:7019/api/{url}?{parameters}", "");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            onFailure(www.error);
        }
        else
        {
            onSuccess(www.result);
        }
    }

    private IEnumerator Delete(string url, string parameters, Action<UnityWebRequest.Result> onSuccess, Action<string> onFailure)
    {
        UnityWebRequest www = UnityWebRequest.Delete($"https://localhost:7019/api/{url}?{parameters}");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            onFailure(www.error);
        }
        else
        {
            onSuccess(www.result);
        }
    }
}
