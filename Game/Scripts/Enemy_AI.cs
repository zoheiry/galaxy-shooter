using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour {

    private float _speed = 3.0f;
    private int _xAxisLimit = 8;
    private int _yAxisLimit = 6;

    [SerializeField]
    private GameObject _enemyExplosionPrefab;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private AudioClip _clip;

    // Use this for initialization
    void Start () {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
	
	// Update is called once per frame
	void Update () {
        MoveEnemy();
	}

    private void MoveEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        float currentYPosition = transform.position.y;
        if (currentYPosition < -_yAxisLimit)
        {
            RespawnEnemy();
        }
    }

    private void RespawnEnemy()
    {
        // int newXPosition = Random.Range(-_xAxisLimit, _xAxisLimit);
        // transform.position = new Vector3(newXPosition, _yAxisLimit + 1, 0);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player)
            {
                player.Damage();
                Blowup();
            }

        }
        else if (collision.tag == "Laser")
        {
            Laser laser = collision.GetComponent<Laser>();

            if (laser)
            {
                if (laser.transform.parent)
                {
                    Destroy(laser.transform.parent.gameObject);
                }
                Destroy(laser.gameObject);
                Blowup();
            }
        }
    }

    private void Blowup()
    {
        AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);
        if (uiManager)
        {
            uiManager.UpdateScore();
        }
        Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
