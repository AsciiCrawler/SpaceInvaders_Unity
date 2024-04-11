using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    private const float MIN_TICK_SPEED = 0.0025f;
    private const float MAX_TICK_SPEED = 0.75f;

    public GameObject ALIEN_PREFAB;
    public GameObject PLAYER_PREFAB;
    public InputAction inputActionStartGame;

    private float tickSpeed = 0.0f;
    private float tickSlice = 0.0f;
    private int direction = 1;
    private List<Enemy> enemies = new List<Enemy>();
    private GameObject player;

    void Start()
    {
        GameManager.gameManager = this;

        this.inputActionStartGame.Enable();
        this.StartGame();
        StartCoroutine(TickCoroutine());
    }

    void Update()
    {
        // Restart Game
        if (this.inputActionStartGame.triggered)
            this.StartGame();
    }

    public void StartGame()
    {
        // Delete All Enemies - Clear list
        for (int i = 0; i < this.enemies.Count; i++)
            Destroy(this.enemies[i].gameObject);
        this.enemies = new List<Enemy>();

        // Move/Instantiate Player
        if (this.player == null)
            this.player = Instantiate(this.PLAYER_PREFAB, new Vector3(0, -8, 0), quaternion.identity);
        else
            this.player.transform.position = new Vector3(0, -8, 0);

        // Instantiate All Enemies
        float y = 9.0f;
        for (int j = 0; j < 5; j++)
        {
            float x = -8.0f;
            for (int i = 0; i < 9; i++)
            {
                Enemy enemy = Instantiate(this.ALIEN_PREFAB, new Vector3(x, y, 0), quaternion.identity).GetComponent<Enemy>();
                this.enemies.Add(enemy);
                x += 1.5f;
            }

            y -= 1.5f;
        }

        this.tickSpeed = MAX_TICK_SPEED;
        if (this.enemies.Count != 0)
            this.tickSlice = (this.tickSpeed - MIN_TICK_SPEED) / this.enemies.Count;
    }

    IEnumerator TickCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(this.tickSpeed);
            MoveAll();
        }
    }

    void MoveAll()
    {
        void MoveAllDown()
        {
            for (int i = 0; i < this.enemies.Count; i++)
                this.enemies[i].transform.position = this.enemies[i].transform.position + new Vector3(0.0f, -0.55f, 0.0f);
        }

        if (this.enemies.Count == 0) return;
        int newDirection = direction;

        for (int i = 0; i < this.enemies.Count; i++)
            this.enemies[i].transform.position = this.enemies[i].transform.position + new Vector3(this.direction * 0.2f, 0.0f, 0.0f);

        for (int i = 0; i < this.enemies.Count; i++)
            if (this.enemies[i].transform.position.x > StaticVariables.HORIZONTAL_LIMIT)
            {
                newDirection = -1;
                MoveAllDown();
                break;
            }
            else if (this.enemies[i].transform.position.x < -StaticVariables.HORIZONTAL_LIMIT)
            {
                newDirection = 1;
                MoveAllDown();
                break;
            }

        this.direction = newDirection;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        this.enemies.Remove(enemy);
        Destroy(enemy.gameObject);

        this.tickSpeed -= this.tickSlice;
    }
}
