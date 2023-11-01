using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    //Canvases
    [SerializeField]
    private GameObject MainCanvas, SignInCanvas, SignUpCanvas, BeforeConnectCanvas, ChangeSettingsCanvas, DeleteUserCanvas, LobbyCanvas;

    //SignIn
    [SerializeField]
    private InputField SignInLoginName, SignInPassword;
    [SerializeField]
    private Text SignInError;

    //SignUp
    [SerializeField]
    private InputField SignUpUsername, SignUpLoginName, SignUpPassword;
    [SerializeField]
    private Text SignUpSuccess, SignUpError;

    //Change settings
    [SerializeField]
    private TMP_InputField NewUsername, NewLoginName, NewPassword, OldLoginName, OldPassword;
    [SerializeField]
    private TMP_Text ChangeSettingsStatus;

    //Delete user
    [SerializeField]
    private TMP_InputField DeleteLoginName, DeletePassword;
    [SerializeField]
    private TMP_Text DeleteError;

    //Lobby
    [SerializeField]
    private LobbyHandler Lobby;

    //API
    [SerializeField]
    private API Api;
    [SerializeField]
    private WebsocketAPI WsApi;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        SignInCanvas.SetActive(false);
        SignUpCanvas.SetActive(false);
        BeforeConnectCanvas.SetActive(false);
        ChangeSettingsCanvas.SetActive(false);
        DeleteUserCanvas.SetActive(false);
        LobbyCanvas.SetActive(false);

        MainCanvas.SetActive(true);
        WsApi.OnHandshake += OnHandshake;
    }

    public void OpenSignInMenu()
    {
        MainCanvas.SetActive(false);
        SignUpCanvas.SetActive(false);
        DeleteUserCanvas.SetActive(false);
        ChangeSettingsCanvas.SetActive(false);
        SignInCanvas.SetActive(true);
    }

    public void OpenSignUpMenu()
    {
        MainCanvas.SetActive(false);
        SignInCanvas.SetActive(false);
        SignUpCanvas.SetActive(true);
    }

    public void OpenMainMenu()
    {
        SignUpCanvas.SetActive(false);
        SignInCanvas.SetActive(false);
        MainCanvas.SetActive(true);
    }

    public void OpenBeforeConnectMenu()
    {
        SignInCanvas.SetActive(false);
        ChangeSettingsCanvas.SetActive(false);
        LobbyCanvas.SetActive(false);
        BeforeConnectCanvas.SetActive(true);
    }

    public void OpenChangeSettingsMenu()
    {
        BeforeConnectCanvas.SetActive(false);
        DeleteUserCanvas.SetActive(false);
        ChangeSettingsCanvas.SetActive(true);
    }

    public void OpenDeleteMenu()
    {
        ChangeSettingsCanvas.SetActive(false);
        DeleteUserCanvas.SetActive(true);
    }

    public async void OpenLobbyMenu()
    {
        BeforeConnectCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
        await WsApi.InitAsync(player);
    }

    //SignIn
    public void SignIn()
    {
        Api.SignIn(SignInLoginName.text, SignInPassword.text, OnSignInSuccess, OnSignInFailure);
    }

    public void OnSignInSuccess(string result)
    {
        player = Player.CreateFromJSON(result);
        OpenBeforeConnectMenu();
    }

    public void OnSignInFailure(UnityWebRequest.Result result)
    {
        SignInError.text = "Something went wrong. Please try again later.";
    }

    //SignUp
    public void SignUp()
    {
        var signUpDto = new SignUpDTO(SignUpUsername.text, SignUpLoginName.text, SignUpPassword.text);
        Api.SignUp(signUpDto, OnSignUpSuccess, OnSignUpFailure);
    }

    public void OnSignUpSuccess(string result)
    {
        SignUpSuccess.text = "User created.";
        OpenSignInMenu();
    }

    public void OnSignUpFailure(string result)
    {
        if (result.Contains("Conflict"))
            SignUpError.text = "Username already used";
        else
            SignUpError.text = "Something went wrong. Please try again later.";
    }
    public void ChangeSettings()
    {
        var dto = new ChangeSettingsDTO(NewUsername.text, NewLoginName.text, NewPassword.text, OldLoginName.text, OldPassword.text);
        Api.ChangeSettings(player.id, dto, OnChangeSuccess, OnChangeFailure);
    }

    public void OnChangeSuccess(UnityWebRequest.Result result)
    {
        OpenSignInMenu();
    }

    public void OnChangeFailure(string result)
    {
        ChangeSettingsStatus.text = "Something went wrong. Please verify your credentials or try again later";
    }

    public void DeleteUser()
    {
        var dto = new DeleteUserDTO(DeleteLoginName.text, DeletePassword.text);
        Api.DeleteUser(dto, OnDeleteSuccess, OnDeleteFailure);
    }

    public void OnDeleteSuccess(UnityWebRequest.Result result)
    {
        OpenSignInMenu();
    }

    public void OnDeleteFailure(string result)
    {
        DeleteError.text = "Something went wrong. Please validate your credentials or try again later.";
    }

    public void OnHandshake(string playersJson)
    {
        var players = Player.CreateListFromJSON(playersJson);
        LobbyCanvas.SetActive(true);
    }
}
