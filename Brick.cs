using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    [Header("Brick Properties")]
    [SerializeField] int hitsToBreak = 1;
    [SerializeField] int scoreValue = 100;
    [SerializeField] Sprite[] brickStateSprites;

    [Header("Powerup Spawns")]
    [SerializeField] float minimumRangeValue = 0;
    [SerializeField] float maximumRangeValue = 1.2f;
    [SerializeField] float spawnChance = 0.05f;
    [SerializeField] GameObject[] powerupPool;

    GameController gameController;
    float randomSpawnChanceResult;
    float randomItemPoolIndexResult;
    int randomPowerupIndexToSelect;
    int randomSpriteIndexToSelect;

    private void Start() {
        gameController = FindObjectOfType<GameController>();
    }

    void Update() {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(tag == "Unbreakable Brick") {
            return;
        }
        hitsToBreak--;
        CheckBrickType();
        gameController.IncreaseScore(scoreValue);
        if(hitsToBreak <= 0) {
            randomSpawnChanceResult = Random.Range(minimumRangeValue, maximumRangeValue);
            if (randomSpawnChanceResult <= spawnChance) {
                randomItemPoolIndexResult = Random.Range(minimumRangeValue, maximumRangeValue);
                if (randomItemPoolIndexResult <= 0.2f) {
                    randomPowerupIndexToSelect = 0;
                }
                else if (randomItemPoolIndexResult <= 0.4f) {
                    randomPowerupIndexToSelect = 1;
                }
                else if (randomItemPoolIndexResult <= 0.6f) {
                    randomPowerupIndexToSelect = 2;
                }
                else if (randomItemPoolIndexResult <= 0.8f) {
                    randomPowerupIndexToSelect = 3;
                }
                else if (randomItemPoolIndexResult <= 1f) {
                    randomPowerupIndexToSelect = 4;
                }
                else if (randomItemPoolIndexResult <= 1.2f) {
                    randomPowerupIndexToSelect = 5;
                } else if (randomItemPoolIndexResult <= 1.4f) {
                    randomPowerupIndexToSelect = 6;
                } else {
                    randomPowerupIndexToSelect = 7;
                }
                Instantiate(powerupPool[randomPowerupIndexToSelect], transform.position, Quaternion.identity);
            }
            gameController.RemoveBrickFromCount();
            Destroy(gameObject);
        }
    }

    private void CheckBrickType()
    {
        if(brickStateSprites.Length > 0 && gameObject.tag == "Gold Brick") {
            AssignSprite("Gold Brick", 1);
        }
        if (brickStateSprites.Length > 0 && gameObject.tag == "Iron Brick") {
            if (hitsToBreak == 2) {
                AssignSprite("Iron Brick", 2);
            }
            if (hitsToBreak == 1) {
                AssignSprite("Iron Brick", 1);
            }
        }
        if(brickStateSprites.Length > 0 && gameObject.tag == "Diamond Brick") {
            if(hitsToBreak == 4) {
                AssignSprite("Diamond Brick", 4);
            }
            if (hitsToBreak == 3) {
                AssignSprite("Diamond Brick", 3);
            }
            if (hitsToBreak == 2) {
                AssignSprite("Diamond Brick", 2);
            }
            if (hitsToBreak == 1) {
                AssignSprite("Diamond Brick", 1);
            }
        } 
    }

    private void AssignSprite(string tag, int hitsLeft) {
        if(tag == "Gold Brick") {
            SelectGoldBrickSprite();
        } else if(tag == "Iron Brick" && hitsLeft == 2) {
            SelectIronBrickSprite(2);
        } else if(tag == "Iron Brick" && hitsLeft == 1) {
            SelectIronBrickSprite(1);
        } else if(tag == "Diamond Brick" && hitsLeft == 4) {
            SelectDiamondBrickSprite(4);
        } else if (tag == "Diamond Brick" && hitsLeft == 3) {
            SelectDiamondBrickSprite(3);
        } else if (tag == "Diamond Brick" && hitsLeft == 2) {
            SelectDiamondBrickSprite(2);
        } else if (tag == "Diamond Brick" && hitsLeft == 1) {
            SelectDiamondBrickSprite(1);
        }
    }

    private void SelectGoldBrickSprite() {
        randomSpriteIndexToSelect = Random.Range(0, brickStateSprites.Length);
        GetComponent<SpriteRenderer>().sprite = brickStateSprites[randomSpriteIndexToSelect];
    }

    private void SelectIronBrickSprite(int hitsLeft) {
        if(hitsLeft == 2) {
            randomSpriteIndexToSelect = Random.Range(0, brickStateSprites.Length / 2);
            GetComponent<SpriteRenderer>().sprite = brickStateSprites[randomSpriteIndexToSelect];
        } else {
            randomSpriteIndexToSelect = Random.Range(brickStateSprites.Length / 2, brickStateSprites.Length);
            GetComponent<SpriteRenderer>().sprite = brickStateSprites[randomSpriteIndexToSelect];
        }
    }

    private void SelectDiamondBrickSprite(int hitsLeft) {
        if(hitsLeft == 4) {
            randomSpriteIndexToSelect = Random.Range(0, brickStateSprites.Length / 4);
            GetComponent<SpriteRenderer>().sprite = brickStateSprites[randomSpriteIndexToSelect];
        } else if(hitsLeft == 3) {
            randomSpriteIndexToSelect = Random.Range(brickStateSprites.Length / 4, brickStateSprites.Length / 2);
            GetComponent<SpriteRenderer>().sprite = brickStateSprites[randomSpriteIndexToSelect];
        } else if(hitsLeft == 2) {
            randomSpriteIndexToSelect = Random.Range(brickStateSprites.Length / 2, brickStateSprites.Length - (brickStateSprites.Length / 4));
            GetComponent<SpriteRenderer>().sprite = brickStateSprites[randomSpriteIndexToSelect];
        } else {
            randomSpriteIndexToSelect = Random.Range(brickStateSprites.Length - (brickStateSprites.Length / 4), brickStateSprites.Length);
            GetComponent<SpriteRenderer>().sprite = brickStateSprites[randomSpriteIndexToSelect];
        }
    }
}
