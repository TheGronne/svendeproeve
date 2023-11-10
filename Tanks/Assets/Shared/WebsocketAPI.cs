using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;

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
    public static event Action<int> OnEndGame;

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

        _connection.On("ReceiveRotation", (int playerId, float rotation) =>
        {
            OnRotation?.Invoke(playerId, rotation);
        });

        _connection.On("ReceivePlayerPosition", (int playerId, float posX, float posY) =>
        {
            OnPlayerPosition?.Invoke(playerId, posX, posY);
        });

        _connection.On("ReceiveMousePosition", (int playerId, float angle) =>
        {
            OnMousePosition?.Invoke(playerId, angle);
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

        _connection.On("EndGame", (int winningPlayerId) =>
        {
            OnEndGame?.Invoke(winningPlayerId);
        });

        await _connection.StartAsync();

        Debug.Log(_connection);
    }

    public static async Task SendInput()
    {
        await _connection.SendAsync("SendInput", EventMessages.Shoot);
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

    public static async Task SendRotation(float rotation)
    {
        await _connection.SendAsync("SendRotation", rotation);
    }

    public static async Task SendPlayerPosition(float posX, float posY)
    {
        await _connection.SendAsync("SendPlayerPosition", posX, posY);
    }

    public static async Task SendMousePosition(float angle)
    {
        await _connection.SendAsync("SendMousePosition", angle);
    }

    public static async Task SendBulletSpawn(float posX, float posY, float angle)
    {
        await _connection.SendAsync("SendBulletSpawn", posX, posY, angle);
    }

    public static async Task SendBulletDestroy(int bulletId)
    {
        await _connection.SendAsync("SendBulletDestroy", bulletId);
    }

    public static async Task SendPlayerHit()
    {
        await _connection.SendAsync("SendPlayerHit");
    }
}
