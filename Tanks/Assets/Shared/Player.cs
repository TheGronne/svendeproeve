using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int id;
    public string username;
    public bool isready;

    public static Player CreateFromJSON(string json)
    {
        return JsonUtility.FromJson<Player>(json);
    }

    public static List<Player> CreateListFromJSON(string json)
    {
        var fullJson = "{\"players\":" + json.ToLower() + "}";
        var list = JsonUtility.FromJson<PlayerList>(fullJson);
        return list.players;
    }
}

[System.Serializable]
public class PlayerList
{
    public List<Player> players;
}
