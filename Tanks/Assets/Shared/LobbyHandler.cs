using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LobbyHandler
{
    public static Player localPlayer = new Player();
    public static List<Player> players = new List<Player>();
    public static Player lastWinningPlayer = new Player();

    public static void ChangeReadyState(int id, bool ready)
    {
        var player = players.Find(p => p.id == id);
        player.isready = ready;
    }

    public static bool IsEveryoneReady()
    {
        if (players.Count <= 0)
            return false;

        var allReady = true;
        foreach (var player in players)
        {
            if (!player.isready)
                allReady = false;
        }

        return allReady;
    }

    public static void RemovePlayer(int id)
    {
        if (players.Count <= 0)
            return;

        var removedPlayer = players.Find(p => p.id == id);
        players.Remove(removedPlayer);
    }

    public static int GetPlayerIndex(int id)
    {
        return players.FindIndex(p => p.id == id);
    }

    public static bool IsLocalPlayer(int id)
    {
        return localPlayer.id == id;
    }

    public static void SetWinningPlayer(int id)
    {
        var player = players.Find(p => p.id == id);
        lastWinningPlayer = player;
    }
}
