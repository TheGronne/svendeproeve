using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> playerTanks;
    public List<GamePlayer> players = new List<GamePlayer>();

    [SerializeField]
    public List<GameObject> levels;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private List<GameObject> playerScores;

    private bool roundInProgess;

    private List<UnSpawnedBullet> unSpawnedBullets = new List<UnSpawnedBullet>();

    private bool startNewRound = false;
    private int nextLevelId = 0;

    private GameObject currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        PopulatePlayers();
        WebsocketAPI.OnDisconnect += OnDisconnect;
        WebsocketAPI.OnRotation += OnRotation;
        WebsocketAPI.OnPlayerPosition += OnPlayerPosition;
        WebsocketAPI.OnMousePosition += OnMousePosition;
        WebsocketAPI.OnBulletDestroy += OnBulletDestroy;
        WebsocketAPI.OnBulletSpawn += OnShootOnline;
        WebsocketAPI.OnPlayerHit += OnHitOnlinePlayer;
        WebsocketAPI.OnStartRound += OnStartNewRound;
        WebsocketAPI.OnEndGame += EndGame;

        for (int i = players.Count; i < 4; i++)
        {
            playerScores[i].SetActive(false);
        }
        
        StartRound();
    }

    // Update is called once per frame
    async void Update()
    {
        if (roundInProgess)
        {
            if (startNewRound)
                StartNewRound();

            //Send state to other players
            var localPlayer = GetLocalPlayer();
            await WebsocketAPI.SendPlayerState(localPlayer.gameObject.GetComponent<LocalPlayerMovement>().GetCurrentState());

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

            if (unSpawnedBullets.Count > 0)
            {
                foreach (var item in unSpawnedBullets)
                {
                    var bullet = Instantiate(bulletPrefab, new Vector3(item.PosX, item.PosY, -2f), Quaternion.Euler(new Vector3(0f, 0f, item.Angle)));
                    SetBulletProperties(bullet, item.PlayerId);
                }
                unSpawnedBullets.Clear();
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

        for (int i = 0; i < players.Count; i++)
        {
            playerScores[i].GetComponent<TMP_Text>().text = "P" + (i + 1) + ": " + players[i].wins;
        }

        ChangeLevel(nextLevelId);
        SetPlayerPositions();
        SetPlayerActive();
        roundInProgess = true;
    }

    private void EndRound()
    {
        roundInProgess = false;
        var winningPlayer = players.Find(p => p.alive == true);

        foreach (var player in players)
        {
            foreach (var bullet in player.bullets)
            {
                Destroy(bullet);
            }
            player.bullets.Clear();
        }
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

        GetLocalPlayer().gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
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
        players.Clear();
        foreach (var tank in playerTanks)
        {
            try { Destroy(tank.GetComponent<LocalPlayerMovement>()); } catch (Exception ex) { }
            try { Destroy(tank.GetComponent<OnlinePlayer>()); } catch (Exception ex) { }
        }

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

        var localPlayer = GetLocalPlayer();
        localPlayer.gameObject.GetComponent<LocalPlayerMovement>().OnShoot += OnShootLocal;
        localPlayer.gameObject.GetComponent<LocalPlayerMovement>().SetPlayerID(localPlayer.id);
    }

    private void EndGame(int? winningPlayerId)
    {
        if (winningPlayerId != null)
            LobbyHandler.SetWinningPlayer((int)winningPlayerId);

        SceneManager.LoadScene("Menu");
    }

    private void PlayerDie(int playerId)
    {
        var player = GetPlayerById(playerId);
        player.alive = false;
        player.gameObject.SetActive(false);
    }

    private async void OnShootLocal(Vector3 position, float angle)
    {
        if (GetLocalPlayer().bullets.Count >= 5)
            return;

        var bullet = Instantiate(bulletPrefab, position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        SetBulletProperties(bullet, GetLocalPlayer().id);
        await WebsocketAPI.SendBulletSpawn(position.x, position.y, angle);
    }

    private void OnShootOnline(int playerId, float posX, float posY, float angle)
    {
        unSpawnedBullets.Add(new UnSpawnedBullet(playerId, posX, posY, angle));
    }

    private GamePlayer GetLocalPlayer()
    {
        return players.Find(p => p.id == LobbyHandler.localPlayer.id);
    }

    private GamePlayer GetPlayerById(int id)
    {
        return players.Find(p => p.id == id);
    }

    private async void OnBulletDestroy(int playerId, int bulletId)
    {
        var player = GetPlayerById(playerId);
        var bullet = player.bullets.Find(b => b.GetComponent<BulletMovement>().bulletId == bulletId);
        player.bullets.Remove(bullet);
        Destroy(bullet.gameObject);

        if (player.id == GetLocalPlayer().id)
            await WebsocketAPI.SendBulletDestroy(bulletId);
    }

    //Killing player is the player who shot the bullet that killed the player
    private async void OnHitLocalPlayer(int killingPlayerId)
    {
        PlayerDie(GetLocalPlayer().id);
        await WebsocketAPI.SendPlayerHit(killingPlayerId);
    }

    private void OnHitOnlinePlayer(int playerId)
    {
        PlayerDie(playerId);
    }

    private void SetBulletProperties(GameObject bullet, int playerId)
    {
        var player = GetPlayerById(playerId);
        var bulletScript = bullet.GetComponent<BulletMovement>();

        bulletScript.bulletId = FindMissingBulletId(player);
        bulletScript.playerId = playerId;
        bulletScript.OnDestroy += OnBulletDestroy;
        bulletScript.OnHitLocalPlayer += OnHitLocalPlayer;

        player.bullets.Add(bullet);
    }

    private int FindMissingBulletId(GamePlayer player)
    {
        var ids = player.bullets.Select(b => b.GetComponent<BulletMovement>().bulletId).ToArray();

        for (int i = 0; i < ids.Length; i++)
        {
            if (!ids.Contains(i))
                return i;
        }

        return ids.Length;
    }

    private void OnStartNewRound(int winningPlayerId, int winningPlayerWins, int levelId)
    {
        if (winningPlayerId != 0 && winningPlayerWins != 0)
            GetPlayerById(winningPlayerId).wins = winningPlayerWins;


        startNewRound = true;
        nextLevelId = levelId;
    }

    private void StartNewRound()
    {
        EndRound();
        StartRound();
        startNewRound = false;
    }

    private void ChangeLevel(int levelId)
    {
        if (currentLevel != null)
            Destroy(currentLevel);

        currentLevel = Instantiate(levels[levelId], new Vector3(0f, 0f, 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
    }

    private void OnDisconnect(int playerId)
    {
        var player = GetPlayerById(playerId);
        players.Remove(player);
        LobbyHandler.RemovePlayer(playerId);
    }
}
