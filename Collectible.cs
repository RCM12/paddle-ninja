using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Collectible : MonoBehaviour {

    [SerializeField] float downwardVelocity = -5f;
    [SerializeField] float rotationSpeed = 180f;

    GameController gameController;
    Rigidbody2D thisRigidBody2D;
    Paddle paddle;
    Lives lives;

    private void Start() {
        gameController = FindObjectOfType<GameController>();
        thisRigidBody2D = GetComponent<Rigidbody2D>();
        lives = FindObjectOfType<Lives>();
    }

    void Update() {
        paddle = FindObjectOfType<Paddle>();
        if(tag == "Snowflake") {
            print("Rotating");
            transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
        }
        Move();
    }

    private void Move() {
        Vector2 speed = new Vector2(0, downwardVelocity);
        thisRigidBody2D.velocity = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(tag == "Life Up" && collision.gameObject.GetComponent<Paddle>()) {
            lives.AddHeartContainer();
        }
        if(tag == "Life Down" && collision.gameObject.GetComponent<Paddle>()) {
            gameController.DecreaseLives();
            lives.RemoveHeartContainer();
        }
        if(tag == "Extra Ball" && collision.gameObject.GetComponent<Paddle>()) {
            gameController.AddExtraBall();
        }
        if(tag == "Snowflake" && collision.gameObject.GetComponent<Paddle>()) {
            paddle.StopMovement();
        }
        if(tag == "Paddle Grow" && collision.gameObject.GetComponent<Paddle>()) {
            gameController.ChangeToLargePaddle();
        }
        if(tag == "Paddle Shrink" && collision.gameObject.GetComponent<Paddle>()) {
            gameController.ChangeToSmallPaddle();
        }
        if(tag == "Faster Ball" && collision.gameObject.GetComponent<Paddle>()) {
            gameController.MoveBallsFaster();
        }
        if(tag == "Slower Ball" && collision.gameObject.GetComponent<Paddle>()) {
            gameController.MoveBallsSlower();
        }
        Destroy(gameObject);
    }
}
