using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    //Canvases
    [SerializeField]
    private GameObject MainCanvas, SignInCanvas, SignUpCanvas, BeforeConnectCanvas, ChangeSettingsCanvas, DeleteUserCanvas, LobbyCanvas, StatsCanvas;

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
    private List<TMP_Text> PlayerNames, PlayerStatuses;
    [SerializeField]
    private List<Button> PlayerReadyButtons;
    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private TMP_Text WinningPlayer;

    //MatchStats
    [SerializeField]
    private GameObject MatchStatContent, MatchPrefab;

    //API
    [SerializeField]
    private API Api;

    private bool ShouldStart = false;

    // Start is called before the first frame update
    void Start()
    {
        SignInCanvas.SetActive(false);
        SignUpCanvas.SetActive(false);
        BeforeConnectCanvas.SetActive(false);
        ChangeSettingsCanvas.SetActive(false);
        DeleteUserCanvas.SetActive(false);
        LobbyCanvas.SetActive(false);
        StatsCanvas.SetActive(false);

        MainCanvas.SetActive(true);
        WebsocketAPI.OnHandshake += OnHandshake;
        WebsocketAPI.OnReady += OnReadyUp;
        WebsocketAPI.OnStart += OnStart;
        WebsocketAPI.OnDisconnect += OnDisconnect;

        if (LobbyHandler.players.Count > 0)
        {
            OpenLobbyMenu();
            if (!string.IsNullOrEmpty(LobbyHandler.lastWinningPlayer.username))
            {
                WinningPlayer.text = "WINNER: " + LobbyHandler.lastWinningPlayer.username;
                LobbyHandler.lastWinningPlayer = new Player();
            }
        }
    }

    private void Update()
    {
        if (LobbyCanvas.activeSelf)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    PlayerNames[i].text = LobbyHandler.players[i].username;
                    if (LobbyHandler.players[i].id == LobbyHandler.localPlayer.id)
                        PlayerReadyButtons[i].gameObject.SetActive(true);

                    if (LobbyHandler.players[i].isready)
                    {
                        PlayerStatuses[i].text = "Ready";
                        PlayerStatuses[i].color = new Color(0, 255, 0);
                        PlayerReadyButtons[i].GetComponentInChildren<TMP_Text>().text = "Unready";
                        PlayerReadyButtons[i].GetComponent<Image>().color = new Color(255, 0, 0);
                    }
                    else
                    {
                        PlayerStatuses[i].text = "Not Ready";
                        PlayerStatuses[i].color = new Color(255, 0, 0);
                        PlayerReadyButtons[i].GetComponentInChildren<TMP_Text>().text = "Ready up";
                        PlayerReadyButtons[i].GetComponent<Image>().color = new Color(0, 255, 0);
                    }
                }
                catch
                {
                    PlayerNames[i].text = "Player Name";
                    PlayerStatuses[i].text = "Not Ready";
                    PlayerStatuses[i].color = new Color(255, 0, 0);
                    PlayerReadyButtons[i].gameObject.SetActive(false);
                    continue;
                }
            }

            if (LobbyHandler.IsEveryoneReady())
                StartButton.gameObject.SetActive(true);
            else
                StartButton.gameObject.SetActive(false);

            if (ShouldStart)
                SceneManager.LoadScene("InGame");
        }
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
        StatsCanvas.SetActive(false);
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
        MainCanvas.SetActive(false);
        SignUpCanvas.SetActive(false);
        DeleteUserCanvas.SetActive(false);
        ChangeSettingsCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
        if (LobbyHandler.players.Count <= 0)
            await WebsocketAPI.InitAsync(LobbyHandler.localPlayer);
    }

    public void OpenStatsMenu()
    {
        BeforeConnectCanvas.SetActive(false);
        MainCanvas.SetActive(false);
        SignUpCanvas.SetActive(false);
        DeleteUserCanvas.SetActive(false);
        ChangeSettingsCanvas.SetActive(false);
        LobbyCanvas.SetActive(false);
        StatsCanvas.SetActive(true);
        GetMatchStats();
    }

    //SignIn
    public void SignIn()
    {
        Api.SignIn(SignInLoginName.text, SignInPassword.text, OnSignInSuccess, OnSignInFailure);
    }

    public void OnSignInSuccess(string result)
    {
        LobbyHandler.localPlayer = Player.CreateFromJSON(result);
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
        Api.ChangeSettings(LobbyHandler.localPlayer.id, dto, OnChangeSuccess, OnChangeFailure);
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

    public async void ReadyUp()
    {
        await WebsocketAPI.SendReady(!LobbyHandler.localPlayer.isready);
        LobbyHandler.localPlayer.isready = !LobbyHandler.localPlayer.isready;
    }

    public void OnReadyUp(int playerId, bool ready)
    {
        LobbyHandler.ChangeReadyState(playerId, ready);
    }

    public async void StartGame()
    {
        await WebsocketAPI.StartGame();
    }

    public void OnStart()
    {
        ShouldStart = true;
    }

    public async void DisonnectFromLobby()
    {
        OpenBeforeConnectMenu();
        await WebsocketAPI.Disconnect();
        LobbyHandler.players.Clear();
    }

    public void OnDisconnect(int id)
    {
        LobbyHandler.RemovePlayer(id);
    }

    public void OnHandshake(string playersJson)
    {
        var players = Player.CreateListFromJSON(playersJson);
        LobbyHandler.players = players;
        LobbyCanvas.SetActive(true);
    }

    public void GetMatchStats()
    {
        Api.GetMatchStats(LobbyHandler.localPlayer.id, OnGetMatchStatsSuccess, OnGetMatchStatsFailure);
    }

    public void OnGetMatchStatsSuccess(string resultJson)
    {
        var stats = MatchStat.CreateListFromJSON(resultJson);
        for (int i = 0; i < stats.Count; i++)
        {
            var match = stats[i];
            var prab = Instantiate(MatchPrefab);
            prab.transform.SetParent(MatchStatContent.transform, false);
            prab.transform.position = new Vector2(prab.transform.position.x, prab.transform.position.y - (i * 25));

            for (int j = 0; j < match.playernames.Count; j++)
            {
                prab.transform.Find("Player" + (j + 1)).GetComponent<TMP_Text>().text = match.playernames[j];
            }
            prab.transform.Find("Kills").GetComponent<TMP_Text>().text = match.kills.ToString();
            prab.transform.Find("Deaths").GetComponent<TMP_Text>().text = match.deaths.ToString();
            prab.transform.Find("Winner").GetComponent<TMP_Text>().text = match.winner;
        }
        
    }

    public void OnGetMatchStatsFailure(UnityWebRequest.Result result)
    {

    }
}
