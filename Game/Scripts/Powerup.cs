using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupId; //0 tripleshot, 1 speed boost, 2 shield

    private float _yAxisLimit = 6.0f;

    [SerializeField]
    private AudioClip _clip;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -_yAxisLimit)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player)
            {
                AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);
                if (_powerupId == 0)
                {
                    player.TripleShotPowerupOn();
                }
                else if (_powerupId == 1)
                {
                    player.SpeedBoostPowerupOn();
                }
                else if (_powerupId == 2)
                {
                    player.giveShield();
                }
            }
            Destroy(this.gameObject);
        }
    }
}
