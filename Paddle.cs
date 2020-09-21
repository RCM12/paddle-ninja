using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float boundaryPadding = 1f;
    [SerializeField] float secondsToWait = 1f;

    float xMinimum;
    float xMaximum;
    bool canMove = true;

    // Start is called before the first frame update
    void Start() {
        transform.position = new Vector2(transform.position.x, transform.position.y);
        EstablishHorizontalBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove) {
            Move();
        }
    }

    private void Move() {
        float changeInXPosition = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float newXPositionRelative = Mathf.Clamp(transform.position.x + changeInXPosition, xMinimum, xMaximum);
        Vector2 newXPosition = new Vector2(newXPositionRelative, transform.position.y);
        transform.position = newXPosition;
    }

    private void EstablishHorizontalBoundaries() {
        Camera mainCamera = Camera.main;
        xMinimum = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + boundaryPadding;
        xMaximum = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - boundaryPadding;
    }

    public void StopMovement() {
        canMove = false;
        GetComponent<SpriteRenderer>().color = new Color(0, 235, 255);
        StartCoroutine(StopMoving());
    }

    private IEnumerator StopMoving() {
        yield return new WaitForSeconds(secondsToWait);
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        canMove = true;
    }
}
