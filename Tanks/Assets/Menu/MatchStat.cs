using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchStat
{
    public List<string> playernames;
    public int kills;
    public int deaths;
    public string winner;

    public static MatchStat CreateFromJSON(string json)
    {
        return JsonUtility.FromJson<MatchStat>(json);
    }

    public static List<MatchStat> CreateListFromJSON(string json)
    {
        var fullJson = "{\"matches\":" + json.ToLower() + "}";
        var list = JsonUtility.FromJson<MatchStatList>(fullJson);
        return list.matches;
    }
}

[System.Serializable]
public class MatchStatList
{
    public List<MatchStat> matches;
}
