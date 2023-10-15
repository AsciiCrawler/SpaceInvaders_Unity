using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerLaser : MonoBehaviour
{
    public float speed;
    public float selfDestructionTime = 5.0f;
    private Rigidbody2D rb;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.rb.velocity = new Vector2(0f, speed);
        Destroy(this.gameObject, selfDestructionTime);
    }
}
