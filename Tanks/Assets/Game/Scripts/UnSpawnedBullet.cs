using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnSpawnedBullet
{
    public int PlayerId { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float Angle { get; set; }

    public UnSpawnedBullet(int id, float x, float y, float angle)
    {
        PlayerId = id;
        PosX = x;
        PosY = y;
        Angle = angle;
    }
}
