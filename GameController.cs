using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField] Text timeText;
    [SerializeField] Text scoreText;
    [SerializeField] Text levelText;
    [SerializeField] int time = 200;
    [SerializeField] int numberOfLives = 3;
    [SerializeField] GameObject newBallToInstantiate;
    [SerializeField] GameObject smallPaddle;
    [SerializeField] GameObject largePaddle;
    [SerializeField] GameObject normalPaddle;
    [SerializeField] float secondsToNormalPaddle = 15f;
    [SerializeField] float ballSpeedIncreaseFactor = 1.5f;
    [SerializeField] float ballSpeedDecreaseFactor = 0.5f;

    GameObject newBall;
    Brick[] bricks;
    Paddle paddle;
    GameObject newPaddle;
    GameObject currentPaddle;
    int score = 0;
    int numberOfBricks;
    int numberOfBalls;
    bool newBallAdded = false;
    bool isResetCoroutineActive = false;
    bool lifeLostPaddleCreated = false;

    void Start() {
        timeText.text = time.ToString();
        scoreText.text = score.ToString();
        UpdateTime();
        bricks = FindObjectsOfType<Brick>();
        numberOfBricks = FindObjectsOfType<Brick>().Length;
        RemoveUnbreakableBricksFromCount();
        paddle = FindObjectOfType<Paddle>();
        currentPaddle = paddle.gameObject;
    }

    void Update() {
        paddle = FindObjectOfType<Paddle>();
        numberOfBalls = FindObjectsOfType<Ball>().Length;
        currentPaddle = paddle.gameObject;
        CheckWinCondition();
    }

    private void UpdateTime() {
        StartCoroutine(DecreaseTime());
    }

    private IEnumerator DecreaseTime() {
        while(time > 0) {
            yield return new WaitForSeconds(1);
            time--;
            timeText.text = time.ToString();
        }
    }

    public void IncreaseScore(int pointsToAdd) {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    public void RemoveBrickFromCount() {
        numberOfBricks--;
    }

    public void DecreaseLives() {
        numberOfLives--;
        if (numberOfLives <= 0) {
            print("Game Over");
        }
    }

    public int GetNumberOfLives() {
        return numberOfLives;
    }

    public int GetNumberOfBalls() {
        return numberOfBalls;
    }

    public void SetNumberOfBalls(int number) {
        numberOfBalls = number;
    }

    public void SetNumberOfLives(int lives) {
        numberOfLives = lives;
        print(numberOfLives);
    }

    public void ChangeToSmallPaddle() {
        newPaddle = Instantiate(smallPaddle, currentPaddle.transform.position, Quaternion.identity) as GameObject;
        Destroy(paddle.gameObject);
        currentPaddle = newPaddle;
        if(isResetCoroutineActive) {
            StopCoroutine(ResetToNormalPaddle());
            StartCoroutine(ResetToNormalPaddle());
        }
    }

    public void ChangeToLargePaddle() {
        newPaddle = Instantiate(largePaddle, currentPaddle.transform.position, Quaternion.identity);
        Destroy(paddle.gameObject);
        currentPaddle = newPaddle;
        if (!isResetCoroutineActive) {
            StopCoroutine(ResetToNormalPaddle());
            StartCoroutine(ResetToNormalPaddle());
        }
    }

    public void MoveBallsFaster() {
        Ball[] ballsInScene = FindObjectsOfType<Ball>();
        foreach(Ball ball in ballsInScene) {
            ball.SetBallSpeed(ballSpeedIncreaseFactor);
            ball.ChangeFastSpeedStatus();
        }
    }

    public void MoveBallsSlower() {
        Ball[] ballsInScene = FindObjectsOfType<Ball>();
        foreach (Ball ball in ballsInScene) {
            ball.SetBallSpeed(ballSpeedDecreaseFactor);
            ball.ChangeSlowSpeedStatus();
        }
    }

    public void CreateNewPaddle() {
        Instantiate(normalPaddle, paddle.transform.position, Quaternion.identity);
    }

    private IEnumerator ResetToNormalPaddle() {
        isResetCoroutineActive = true;
        yield return new WaitForSeconds(secondsToNormalPaddle);
        if(!lifeLostPaddleCreated) {
            Instantiate(normalPaddle, currentPaddle.transform.position, Quaternion.identity);
            Destroy(currentPaddle);
            currentPaddle = normalPaddle;
        }
        isResetCoroutineActive = false;
    }

    public void AddExtraBall() {
        newBall = Instantiate(newBallToInstantiate, paddle.transform.position + newBallToInstantiate.GetComponent<Ball>().GetVerticalDistanceFromPaddle(), Quaternion.identity) as GameObject;
        newBallAdded = true;
        ShootBall(newBall);
    }

    public void LifeLostPaddleWasCreated() {
        lifeLostPaddleCreated = true;
    }

    private void ShootBall(GameObject newBall) {
        newBall.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 5);
    }

    public void RemoveNewBallFunctionality() {
        newBallAdded = false;
    }

    public bool NewBallInstantiated() {
        if(newBallAdded) {
            return true;
        } else {
            return false;
        }
    }

    private void RemoveUnbreakableBricksFromCount() {
        foreach(Brick brick in bricks) {
            if (brick.gameObject.tag == "Unbreakable Brick") {
                numberOfBricks--;
            }
        }
    }

    private void CheckWinCondition() {
        if (numberOfBricks <= 0) {
            print("Winner!");
        }
    }
}
