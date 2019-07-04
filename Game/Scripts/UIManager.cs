using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Sprite[] lives;
    public Image livesImageDisplay;
    public Text scoreText;
    public int score;

    [SerializeField]
    private GameObject titleScreen;

	public void UpdateLives(int remainingLives)
    {
        livesImageDisplay.sprite = lives[remainingLives];
    }

    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "Score: " + score;
    }

    public void ResetUI()
    {
        score = 0;
        scoreText.text = "Score: 0";
        livesImageDisplay.sprite = lives[3];
        HideTitleScreen();
    }

    public void ShowTitleScreen()
    {
        titleScreen.SetActive(true);
    }

    public void HideTitleScreen()
    {
        titleScreen.SetActive(false);
    }
}
