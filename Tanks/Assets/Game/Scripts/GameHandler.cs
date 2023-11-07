using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> playerTanks;

    private List<GamePlayer> players = new List<GamePlayer>();

    private bool roundInProgess;

    // Start is called before the first frame update
    void Start()
    {
        PopulatePlayers();

        WebsocketAPI.OnRotation += OnRotation;
        WebsocketAPI.OnPlayerPosition += OnPlayerPosition;
        WebsocketAPI.OnMousePosition += OnMousePosition;

        StartRound();
    }

    // Update is called once per frame
    async void Update()
    {
        if(ShouldStartNewRound())
        {
            EndRound();
            StartRound();
        }

        //Send state to other players
        if (roundInProgess)
        {
            var localPlayer = players[LobbyHandler.GetPlayerIndex(LobbyHandler.localPlayer.id)];
            await WebsocketAPI.SendRotation(localPlayer.gameObject.GetComponent<LocalPlayerMovement>().rotationAngle);
            await WebsocketAPI.SendPlayerPosition(localPlayer.gameObject.transform.position.x, localPlayer.gameObject.transform.position.y);
            await WebsocketAPI.SendMousePosition(localPlayer.gameObject.GetComponent<LocalPlayerMovement>().mouseAngle);

            //Set state of other players
            foreach (var item in players)
            {
                if (!LobbyHandler.IsLocalPlayer(item.id))
                {
                    item.gameObject.GetComponent<OnlinePlayer>().SetRotation(item.rotation);
                    item.gameObject.GetComponent<OnlinePlayer>().SetPlayerPosition(item.positionX, item.positionY);
                    item.gameObject.GetComponent<OnlinePlayer>().SetMousePosition(item.mouseAngle);
                }
            }
        }
    }

    private void OnRotation(int playerId, float rotation)
    {
        var index = LobbyHandler.GetPlayerIndex(playerId);
        players[index].rotation = rotation;
    }

    private void OnPlayerPosition(int playerId, float posX, float posY)
    {
        var index = LobbyHandler.GetPlayerIndex(playerId);
        players[index].positionX = posX;
        players[index].positionY = posY;
    }

    private void OnMousePosition(int playerId, float angle)
    {
        var index = LobbyHandler.GetPlayerIndex(playerId);
        players[index].mouseAngle = angle;
    }

    private void StartRound()
    {
        SetPlayerPositions();
        SetPlayerActive();
        roundInProgess = true;
    }

    private void EndRound()
    {
        roundInProgess = false;
        var winningPlayer = players.Find(p => p.alive == true);
        winningPlayer.wins += 1;

        if (winningPlayer.wins >= 3)
            EndGame();
    }

    private void SetPlayerPositions()
    {
        players[0].positionX = -7f;
        players[0].positionY = 2.75f;
        players[0].gameObject.transform.position = new Vector2(players[0].positionX, players[0].positionY);

        players[1].positionX = 7f;
        players[1].positionY = -2.75f;
        players[1].gameObject.transform.position = new Vector2(players[1].positionX, players[1].positionY);

        if (players.Count > 2)
        {
            players[2].positionX = -7f;
            players[2].positionY = -2.75f;
            players[2].gameObject.transform.position = new Vector2(players[2].positionX, players[2].positionY);
        }
        if (players.Count > 3)
        {
            players[3].positionX = 7f;
            players[3].positionY = 2.75f;
            players[3].gameObject.transform.position = new Vector2(players[3].positionX, players[3].positionY);
        }
    }

    private void SetPlayerActive()
    {
        foreach (var item in players)
        {
            item.gameObject.SetActive(true);
            item.alive = true;
        }
    }

    private void PopulatePlayers()
    {
        for(var i = 0; i < LobbyHandler.players.Count; i++)
        {
            var player = LobbyHandler.players[i];
            players.Add(new GamePlayer(player));

            players[i].gameObject = playerTanks[i];

            if (LobbyHandler.IsLocalPlayer(player.id))
                players[i].gameObject.AddComponent<LocalPlayerMovement>();
            else
                players[i].gameObject.AddComponent<OnlinePlayer>();
        }
    }

    private bool ShouldStartNewRound()
    {
        return players.FindAll(p => p.alive == true).Count == 1;
    }

    private void EndGame()
    {
        Debug.Log("Someone got 3 wins");
    }
}
