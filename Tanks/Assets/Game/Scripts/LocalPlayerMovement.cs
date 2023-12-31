using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class PlayerState
{
    public int id;
    public Hull hull;
    public Turret turret;

    [Serializable]
    public class Hull
    {
        public float PosX;
        public float PosY;
        public float Rotation;

        public Hull(LocalPlayerMovement player)
        {
            PosX = player?.transform?.position.x ?? 0;
            PosY = player?.transform?.position.y ?? 0;
            Rotation = player?.rotationAngle ?? 0;
        }
    }

    [Serializable]
    public class Turret
    {
        public float Rotation;

        public Turret(LocalPlayerMovement player)
        {
            Rotation = player?.mouseAngle ?? 0;
        }
    }

    public PlayerState(LocalPlayerMovement player)
    {
        if (player == null)
            return;

        id = player.playerId;
        hull = new Hull(player);
        turret = new Turret(player);
    }
}

public class LocalPlayerMovement : MonoBehaviour
{
    public int playerId;
    private float speed = 1;

    private float rotationSpeed = 360;

    private Rigidbody2D rb;

    private Vector2 movementDirection;

    public float rotationAngle = 0;
    public float mouseAngle = 0;

    private float moveAngle;

    public GameObject topSprite;
    private GameObject topEnd;

    public event Action<Vector3, float> OnShoot;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
        rb = gameObject.GetComponent<Rigidbody2D>();
        topSprite = gameObject.transform.Find("Top").gameObject;
        topEnd = topSprite.transform.Find("TopEnd").gameObject;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(0))
            OnShoot(topEnd.transform.position, mouseAngle);

        movementDirection = new Vector2(horizontalInput, verticalInput);

        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        rotationAngle = transform.rotation.eulerAngles.z;

        SetTopAngle();
    }

    private void FixedUpdate()
    {
        moveAngle = Mathf.Atan2(movementDirection.x, movementDirection.y) * Mathf.Rad2Deg;
        if (moveAngle < 0)
            moveAngle -= moveAngle * 2;
        else if (moveAngle > 0 && moveAngle < 179)
            moveAngle = 360 - moveAngle;

        if (((moveAngle + 1) - (rotationAngle - 1)) > 1 && ((moveAngle + 1) - (rotationAngle - 1)) < 3) // Done due to floating point single precision
            rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void SetTopAngle()
    {
        Vector2 positionOnScreen = transform.position;
        Vector2 mouseOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseAngle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen) + 90;
        topSprite.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, mouseAngle));
    }

    public PlayerState GetCurrentState()
    {
        return new PlayerState(this);
    }

    public void SetPlayerID(int id)
    {
        playerId = id;
    }
}
