using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject _enemyShipPrefab;
    [SerializeField]
    private GameObject[] powerupsPrefab;
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private float _enemySpawnDuration = 5.0f;
    [SerializeField]
    private float _powerupSpawnDuration = 7.0f;

    private GameManager gameManager;


    private int _xAxisLimit = 8;
    private int _yAxisLimit = 6;


    private void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();   
    }

    public void StartSpawning()
    {
        _enemySpawnDuration = 5.0f;
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
        StartCoroutine(EnemySpawnDurationRoutine());
    }

    public void UpdateEnemySpawnDuration()
    {
        if (_enemySpawnDuration <= 0.5f) {
            return;
        }
        _enemySpawnDuration -= (Time.time * 0.005f);
    }

    IEnumerator EnemySpawnRoutine()
    {
        while(!gameManager._gameOver)
        {
            float randomX = Random.Range(-_xAxisLimit, _xAxisLimit);
            Instantiate(_enemyShipPrefab, new Vector3(randomX, _yAxisLimit, 0), Quaternion.identity);
            yield return new WaitForSeconds(_enemySpawnDuration);
        }
    }

    IEnumerator PowerupSpawnRoutine()
    {
        while(!gameManager._gameOver)
        {
            yield return new WaitForSeconds(_powerupSpawnDuration);

            int randomPowerup = Random.Range(0, 3);
            float randomX = Random.Range(-_xAxisLimit, _xAxisLimit);

            Vector3 powerupPosition = new Vector3(randomX, _yAxisLimit, 0);
            GameObject powerup = powerupsPrefab[randomPowerup];
            Instantiate(powerup, powerupPosition, Quaternion.identity);

        }
    }

    IEnumerator EnemySpawnDurationRoutine()
    {
        while (!gameManager._gameOver)
        {
            yield return new WaitForSeconds(1);
            UpdateEnemySpawnDuration();
        }
    }
}
