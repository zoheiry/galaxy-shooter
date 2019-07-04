using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private float _speed = 7.0f;

    [SerializeField]
    private float _fireRate = 0.25f;

    private float _nextFire = 0.0f;

    private float _xAxisLimit = 9.5f;
    private float _yAxisLimit = 4.0f;

    private int totalLives = 3;
    [SerializeField]
    private int _livesRemaining = 3;
    [SerializeField]
    private bool _hasShield = false;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _shieldGameObject;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject[] _engines; //0 is left, 1 is right.
    private bool[] _failedEngines = new bool[2];

    private AudioSource _audioSource;

    public bool hasTripleShot = false;

    public bool hasSpeedBoost = false;


	private void Start () {
        transform.position = new Vector3(0, 0, 0);

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();

        if (uiManager)
        {
            uiManager.UpdateLives(_livesRemaining);
        }


    }
	
	// Update is called once per frame
	private void Update () {
        bool shouldPlayerMove = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        if (shouldPlayerMove) {
            MovePlayer();
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        if (_nextFire < Time.time)
        {
            _audioSource.Play();
            if (hasTripleShot)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.11f, 0), Quaternion.identity);
            }
            _nextFire = Time.time + _fireRate;
        }
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float speedMultiplier = 1.0f;
        if (hasSpeedBoost)
        {
            speedMultiplier = 1.5f;
        }
        float displacement = _speed * speedMultiplier * Time.deltaTime;
        Vector3 translateVector = new Vector3(horizontalInput * displacement, verticalInput * displacement, 0);

        float currentX = transform.position.x;
        float currentY = transform.position.y;

        if (currentY <= (_yAxisLimit * -1))
        {
            transform.position = new Vector3(currentX, _yAxisLimit * -1, 0);
        }
        else if (currentY >= 0)
        {
            transform.position = new Vector3(currentX, 0, 0);
        }

        if (currentX <= (_xAxisLimit * -1))
        {
            transform.position = new Vector3(_xAxisLimit, currentY, 0);
        }
        else if (currentX >= _xAxisLimit)
        {
            transform.position = new Vector3(_xAxisLimit * -1, currentY, 0);
        }
        transform.Translate(translateVector);
    }

    public void TripleShotPowerupOn()
    {
        hasTripleShot = true;
        StartCoroutine(TripleShowPowerDownRoutine());
    }

    public void SpeedBoostPowerupOn()
    {
        hasSpeedBoost = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        hasSpeedBoost = false;
    }

    public IEnumerator TripleShowPowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        hasTripleShot = false;
    }

    public void Damage()
    {
        if (_hasShield)
        {
            destroyShield();
        }
        else
        {
            _livesRemaining--;
            if (_livesRemaining < 0)
            {
                Blowup();
            } else
            {
                uiManager.UpdateLives(_livesRemaining);
            }
        }
        EngineFailureCheck();
    }

    private void EngineFailureCheck()
    {
        if (_livesRemaining == 1)
        {
            int randomEngine = Random.Range(0, 2);
            _engines[randomEngine].SetActive(true);
            _failedEngines[randomEngine] = true;
        }
        else if (_livesRemaining == 0)
        {
            if (_failedEngines[0])
            {
                _engines[1].SetActive(true);
                _failedEngines[1] = true;
            }
            else
            {
                _engines[0].SetActive(true);
                _failedEngines[0] = true;
            }
        }
    }

    private void Blowup()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        gameManager.EndGame();
        Destroy(this.gameObject);
    }

    public void giveShield()
    {
        _hasShield = true;
        _shieldGameObject.SetActive(true);
    }

    public void destroyShield()
    {
        _hasShield = false;
        _shieldGameObject.SetActive(false);
    }
}
