using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class WebsocketAPI
{
    private const string ServerAddess = "https://localhost:7019/";
    private const string Hub = "gameHub";

    private static HubConnection _connection;

    public static event Action<string> OnHandshake;
    public static event Action<int> OnDisconnect;
    public static event Action<int, bool> OnReady;
    public static event Action OnStart;
    public static event Action<int, float> OnRotation;
    public static event Action<int, float, float> OnPlayerPosition;
    public static event Action<int, float> OnMousePosition;
    public static event Action<int, float, float, float> OnBulletSpawn;
    public static event Action<int, int> OnBulletDestroy;
    public static event Action<int> OnPlayerHit;
    public static event Action<int, int, int> OnStartRound;
    public static event Action<int?> OnEndGame;

    public static async Task InitAsync(Player player)
    {
        _connection = new HubConnectionBuilder()
                .WithUrl(ServerAddess + Hub)
                .Build();

        _connection.On("Connected", async () =>
        {
            await HandShake(player.id, player.username);
        });

        _connection.On("Disconnected", async (int id) =>
        {
            OnDisconnect?.Invoke(id);
        });

        _connection.On("HandShake", (string playersJson) =>
        {
            OnHandshake?.Invoke(playersJson);
        });

        _connection.On("ChangeReady", (int playerId, bool ready) =>
        {
            OnReady?.Invoke(playerId, ready);
        });

        _connection.On("StartGame", () =>
        {
            OnStart?.Invoke();
        });

        _connection.On("ReceivePlayerState", (byte[] bytes) =>
        {
            PlayerState state;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                state = (PlayerState)bf.Deserialize(ms);
            }

            OnRotation?.Invoke(state.id, state.hull.Rotation);
            OnPlayerPosition?.Invoke(state.id, state.hull.PosX, state.hull.PosY);
            OnMousePosition?.Invoke(state.id, state.turret.Rotation);
        });

        _connection.On("ReceiveBulletSpawn", (int playerId, float posX, float posY, float angle) =>
        {
            OnBulletSpawn?.Invoke(playerId, posX, posY, angle);
        });

        _connection.On("ReceiveBulletDestroy", (int playerId, int bulletId) =>
        {
            OnBulletDestroy?.Invoke(playerId, bulletId);
        });

        _connection.On("ReceivePlayerHit", (int playerId) =>
        {
            OnPlayerHit?.Invoke(playerId);
        });

        _connection.On("StartRound", (int winningPlayerId, int winningPlayerWins, int levelId) =>
        {
            Debug.Log("Start Round");
            OnStartRound?.Invoke(winningPlayerId, winningPlayerWins, levelId);
        });

        _connection.On("EndGame", (int? winningPlayerId) =>
        {
            OnEndGame?.Invoke(winningPlayerId);
        });

        await _connection.StartAsync();

        Debug.Log(_connection);
    }

    public static async Task HandShake(int id, string username)
    {
        await _connection.SendAsync("HandShake", id, username);
    }

    public static async Task SendReady(bool ready)
    {
        await _connection.SendAsync("SendReady", ready);
    }

    public static async Task StartGame()
    {
        await _connection.SendAsync("StartGame");
    }

    public static async Task SendPlayerState(PlayerState state)
    {
        byte[] bytes;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, state);
            bytes = ms.ToArray();
        }
        await _connection.SendAsync("SendPlayerState", bytes);
    }

    public static async Task SendBulletSpawn(float posX, float posY, float angle)
    {
        await _connection.SendAsync("SendBulletSpawn", posX, posY, angle);
    }

    public static async Task SendBulletDestroy(int bulletId)
    {
        await _connection.SendAsync("SendBulletDestroy", bulletId);
    }

    public static async Task SendPlayerHit(int killingPlayerId)
    {
        await _connection.SendAsync("SendPlayerHit", killingPlayerId);
    }
    
    public static async Task Disconnect()
    {
        await _connection.StopAsync();
    }
}
