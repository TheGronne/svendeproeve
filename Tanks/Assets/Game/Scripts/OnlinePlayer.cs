using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject topSprite;

    void Start()
    {
        topSprite = gameObject.transform.Find("Top").gameObject;
    }

    public void SetRotation(float rotation)
    {
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }

    public void SetPlayerPosition(float posX, float posY)
    {
        transform.position = new Vector2(posX, posY);
    }

    public void SetMousePosition(float angle)
    {
        if (topSprite != null)
            topSprite.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}
