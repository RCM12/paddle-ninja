using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] float verticalVelocity = 10f;
    [SerializeField] float ballOffset = 0.5f;
    [SerializeField] float minimumXVelocity = -4f;
    [SerializeField] float maximumXVelocity = 4f;
    [SerializeField] float minimumYVelocity = 9f;
    [SerializeField] float maximumYVelocity = 11f;
    [SerializeField] float minimumBounceFactor = -0.5f;
    [SerializeField] float maximumBounceFactor = 0.5f;
    [SerializeField] float secondsUntilNormalSpeed = 10f;
    
    bool ballLaunched = false;
    bool newBallLaunched = false;
    bool ballSpeedIncreased = false;
    bool ballSpeedDecreased = false;
    bool isBackToNormalSpeed = false;

    float horizontalVelocity;
    Vector3 verticalDistanceFromPaddle;
    Paddle paddle;
    Rigidbody2D ballRigidBody2D;
    GameController gameController;

    // Start is called before the first frame update
    void Start() {
        paddle = FindObjectOfType<Paddle>();
        gameController = FindObjectOfType<GameController>();
        verticalDistanceFromPaddle = new Vector3(paddle.transform.position.x, paddle.transform.position.y + ballOffset);
        transform.position = verticalDistanceFromPaddle;
        ballRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if(gameController.NewBallInstantiated() && !newBallLaunched) {
            LaunchNewBall();
            ballLaunched = true;
        }
        if(ballLaunched == false) {
            MoveBallWithPaddle();
            LaunchBall();
        }
    }

    public Vector3 GetVerticalDistanceFromPaddle() {
        return verticalDistanceFromPaddle;
    }

    private void MoveBallWithPaddle() {
        if(ballLaunched == false) {
            transform.position = new Vector2(paddle.transform.position.x, verticalDistanceFromPaddle.y);
        }
    }

    private void LaunchBall() {                                                     
        if(Input.GetButtonDown("Fire1") && ballLaunched == false) {
            horizontalVelocity = Random.Range(minimumXVelocity, maximumXVelocity);
            ballRigidBody2D.velocity = new Vector2(horizontalVelocity, verticalVelocity);
            ballLaunched = true;
        }
    }

    private void LaunchNewBall() {
        if(tag == "New Ball") {
            horizontalVelocity = Random.Range(minimumXVelocity, maximumXVelocity);
            ballRigidBody2D.velocity = new Vector2(horizontalVelocity, verticalVelocity);
            newBallLaunched = true;
        }
    }

    public float GetVerticalVelocity() {
        return verticalVelocity;
    }

    public float GetMinimumXVelocity() {
        return minimumXVelocity;
    }

    public float GetMaximumXVelocity() {
        return maximumXVelocity;
    }

    public void ChangeFastSpeedStatus() {
        ballSpeedIncreased = true;
    }

    public void ChangeSlowSpeedStatus() {
        ballSpeedDecreased = true;
    }

    public void SetBallSpeed(float speedFactor) {
        ballRigidBody2D.velocity = new Vector2(ballRigidBody2D.velocity.x * speedFactor, ballRigidBody2D.velocity.y * speedFactor);
        if(!isBackToNormalSpeed) {
            StartCoroutine(BackToNormalSpeed());
        }
    }

    private IEnumerator BackToNormalSpeed() {
        yield return new WaitForSeconds(secondsUntilNormalSpeed);
        isBackToNormalSpeed = true;
        if(ballSpeedIncreased) {
            SetBallSpeed(0.667f);
            ballSpeedIncreased = false;
        } else if(ballSpeedDecreased) {
            SetBallSpeed(2f);
            ballSpeedDecreased = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D otherCollider) {
        if(otherCollider.gameObject.tag == "Wall") {
            float currentXVelocity = ballRigidBody2D.velocity.x;
            float currentYVelocity = ballRigidBody2D.velocity.y;
            if(currentYVelocity < 3 && currentYVelocity > 0) {
                ballRigidBody2D.velocity = new Vector2(currentXVelocity, currentYVelocity + 1);
            } else if(currentYVelocity < 0 && currentYVelocity > -3) {
                ballRigidBody2D.velocity = new Vector2(currentXVelocity, currentYVelocity - 1);
            }
        }
    }
}

// IMPORTANT NOTE ABOUT THE LAUNCHNEWBALL() METHOD:
    // if we didn't use the "New Ball" tag as a condition, then the line of code starting with "ballRigidBody2D.velocity" would have applied
    // to all game objects (balls) with this script as a component (this is why the balls that were already in play had the same direction as 
    // the new automatically-launched ball
        // the tag separates the game object behaviors even though they have the same script

