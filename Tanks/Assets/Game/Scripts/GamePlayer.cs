using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer
{
    public GameObject gameObject;
    public float rotation;
    public float positionX = 0;
    public float positionY = 0;
    public float mouseAngle = 0;
    public int id;
    public string username;
    public int wins = 0;
    public bool alive = false;

    public GamePlayer(Player player)
    {
        id = player.id;
        username = player.username;
    }
}
