using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;

public static class WebsocketAPI
{
    private const string ServerAddess = "https://localhost:7016/";
    private const string Hub = "gameHub";

    private static HubConnection _connection;

    public static event Action<string> OnHandshake;
    public static event Action<int> OnDisconnect;
    public static event Action<int> OnInputReceive;
    public static event Action<int, bool> OnReady;
    public static event Action OnStart;
    public static event Action<int, float> OnRotation;
    public static event Action<int, float, float> OnPlayerPosition;
    public static event Action<int, float> OnMousePosition;

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
            Debug.Log(angle);
            OnMousePosition?.Invoke(playerId, angle);
        });

        _connection.On("ReceiveInput", (int message) =>
        {
            OnInputReceive?.Invoke(message);
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
}
