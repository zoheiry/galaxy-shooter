using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool _gameOver = true;

    [SerializeField]
    private GameObject playerPrefab;

    private UIManager uiManager;

    [SerializeField]
    private SpawnManager spawnManager;

    private void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void Update()
    {
        if (_gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        _gameOver = false;
        uiManager.ResetUI();
        spawnManager.StartSpawning();
    }

    public void EndGame()
    {
        uiManager.ShowTitleScreen();
        _gameOver = true;
    }
}
