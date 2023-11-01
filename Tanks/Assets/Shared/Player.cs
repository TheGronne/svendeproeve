using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int id;
    public string username;
    public bool isReady;

    public static Player CreateFromJSON(string json)
    {
        return JsonUtility.FromJson<Player>(json);
    }

    public static List<Player> CreateListFromJSON(string json)
    {
        var array = JsonUtility.FromJson<PlayerList>("{\"players\":" + json.ToString() + "}");
        Debug.Log("{\"players\":" + json.ToString() + "}");
        Debug.Log(array);
        Debug.Log(array.players[0].username);
        return new List<Player>();
    }
}

public class PlayerList
{
    public List<Player> players;
}
