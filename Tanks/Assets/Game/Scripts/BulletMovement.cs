using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private float speed = 3f;
    private int bounces = 0;

    private Vector3 velocity;
    private Rigidbody2D rigidbody;

    public event Action<int, int> OnDestroy;
    public event Action OnHitLocalPlayer;

    public int bulletId;
    public int playerId;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        velocity = transform.up;
        rigidbody.velocity = velocity * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (bounces >= 2)
                OnDestroy(playerId, bulletId);
            else
            {
                Reflect(rigidbody, collision.contacts[0].normal);
                bounces++;
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<LocalPlayerMovement>() != null)
                OnHitLocalPlayer();

            OnDestroy(playerId, bulletId);
        }
        if (collision.gameObject.tag == "Bullet")
        {
            OnDestroy(playerId, bulletId);
        }
    }

    private void Reflect(Rigidbody2D rb, Vector3 reflectVector)
    {
        velocity = Vector3.Reflect(velocity, reflectVector);
        rb.velocity = velocity * speed;

        var moveAngle = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
        if (moveAngle < 0)
            moveAngle -= moveAngle * 2;
        else if (moveAngle > 0 && moveAngle < 179)
            moveAngle = 360 - moveAngle;

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, moveAngle));
    }
}
