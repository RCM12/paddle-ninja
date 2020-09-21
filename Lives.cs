using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour {

    [SerializeField] Sprite fullHeartSprite;
    [SerializeField] Sprite emptyHeartSprite;
    [SerializeField] Sprite halfHeartSprite;
    [SerializeField] GameObject[] hearts;

    GameController gameController;

    int numberOfLives;

    void Start() {
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        
    }

    public void AddHeartContainer() {
        if(!IsLastHeartContainerFull()) {
            numberOfLives = gameController.GetNumberOfLives();
            hearts[numberOfLives].GetComponent<Image>().sprite = fullHeartSprite;
            gameController.SetNumberOfLives(numberOfLives + 1);
        }
    }

    public void RemoveHeartContainer() {
        numberOfLives = gameController.GetNumberOfLives();
        hearts[numberOfLives].GetComponent<Image>().sprite = emptyHeartSprite;
    }

    public bool IsFirstHeartContainerEmpty() {
        if(hearts[0].GetComponent<Image>().sprite == emptyHeartSprite) {
            return true;
        } else {
            return false;
        }
    }

    public bool IsLastHeartContainerFull() {
        if(hearts[hearts.Length - 1].GetComponent<Image>().sprite == fullHeartSprite) {
            return true;
        } else {
            return false;
        }
    }
}
