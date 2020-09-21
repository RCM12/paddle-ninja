using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLostCollider : MonoBehaviour {

    [SerializeField] GameObject ball;

    Collectible[] collectibles;
    GameController gameController;
    Lives lives;
    Paddle paddle;
    int numberOfBalls;

    private void Start() {
        gameController = FindObjectOfType<GameController>();
        lives = FindObjectOfType<Lives>();
    }

    private void Update() {
        paddle = FindObjectOfType<Paddle>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.GetComponent<Ball>()) {
            numberOfBalls = gameController.GetNumberOfBalls();
            numberOfBalls--;
            gameController.SetNumberOfBalls(numberOfBalls);
            gameController.RemoveNewBallFunctionality();
            if(gameController.GetNumberOfBalls() == 0) {
                Instantiate(ball, ball.GetComponent<Ball>().GetVerticalDistanceFromPaddle(), Quaternion.identity);
                gameController.DecreaseLives();
                Destroy(paddle.gameObject);
                gameController.CreateNewPaddle();
                gameController.LifeLostPaddleWasCreated();
                collectibles = FindObjectsOfType<Collectible>();
                foreach (Collectible collectible in collectibles)
                {
                    Destroy(collectible.gameObject);
                }
                if (!lives.IsFirstHeartContainerEmpty())
                {
                    lives.RemoveHeartContainer();
                }
            }
        }
        Destroy(collider.gameObject);                                                                            // we need to use ".gameObject" here since "collider" just refers to the collider component
    }
}
