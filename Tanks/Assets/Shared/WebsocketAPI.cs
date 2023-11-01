using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;

public class WebsocketAPI : MonoBehaviour
{
    private const string ServerAddess = "https://localhost:7016/";
    private const string Hub = "gameHub";

    private HubConnection _connection;

    public event Action<int> OnMessageReceived;
    public event Action<string> OnHandshake;

    public async Task InitAsync(Player player)
    {
        _connection = new HubConnectionBuilder()
                .WithUrl(ServerAddess + Hub)
                .Build();

        _connection.On("Connected", async () =>
        {
            await HandShake(player.id, player.username);
        });

        _connection.On("HandShake", (string playersJson) =>
        {
            Debug.Log("Handshake");
            Debug.Log(playersJson);
            OnHandshake(playersJson);
        });

        _connection.On("ChangeReady", (int playerId, bool ready) =>
        {
            OnMessageReceived?.Invoke(playerId);
        });

        _connection.On("ReceiveInput", (int message) =>
        {
            OnMessageReceived?.Invoke(message);
        });

        _connection.On("ReceiveMousePos", (decimal x, decimal y) =>
        {
            
        });


        await _connection.StartAsync();


        Debug.Log(_connection);
    }

    public async Task SendInput()
    {
        await _connection.SendAsync("SendInput", EventMessages.Shoot);
    }

    public async Task HandShake(int id, string username)
    {
        await _connection.SendAsync("HandShake", id, username);
    }

    public async Task SendReady()
    {
        await _connection.SendAsync("SendReady", true);
    }
}
