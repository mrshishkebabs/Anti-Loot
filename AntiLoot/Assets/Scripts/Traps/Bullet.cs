using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 24f;

    [SerializeField] private Rigidbody2D rb;

    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag(Tags.PLAYER))
        {
            gameObject.SetActive(false);
        }
    }
}
