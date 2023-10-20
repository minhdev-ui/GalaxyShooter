using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // private and public variables
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _multipleSpeed = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotLaserPrefab;
    [SerializeField] private GameObject _shield;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private int _live = 3;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private float _durationPowerup = 5.0f;
    private bool _isReady;
    private float _canFire = 0.0f;
    private SpawnManager _spawnManager;
    [SerializeField] private int _score = 0;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -6.6f, 0);
        _isReady = true;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager not found!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isReady)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, -3, 0), Time.deltaTime * _speed);
            _isReady = false;
        }
        CalculateMovement();
        Shooting();
        _shield.SetActive(_isShieldActive);
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        // if (transform.position.y > 5.5f)
        // {
        //     transform.Translate(new Vector3(transform.position.x, 5.5f, transform.position.z) * _speed * _multipleSpeed * Time.deltaTime);
        // } else if (transform.position.y < -4.3f)
        // {
        //     transform.Translate(new Vector3(transform.position.x, -4.3f, transform.position.z) * _speed * _multipleSpeed * Time.deltaTime);
        // }
        //
        // if (transform.position.x > 10f)
        // {
        //     transform.Translate(new Vector3(10, transform.position.y, transform.position.z) * _speed * _multipleSpeed * Time.deltaTime);
        // } else if (transform.position.y < -10f)
        // {
        //     transform.Translate(new Vector3(-10, transform.position.y, transform.position.z) * _speed * _multipleSpeed * Time.deltaTime);
        // }
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeedBoostActive)
        {
            transform.Translate(direction * _speed * _multipleSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }

    void Shooting()
    {
        if (Input.GetMouseButton(0) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotLaserPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            return;
        }

        _live--;
        _uiManager.UpdateLives(_live);
        if (_live < 1)
        {
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.transform.CompareTag("enemy"))
    //     {
    //         Damage();
    //         Destroy(other.transform.gameObject);
    //     }
    // }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(PowerDown(0));
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(PowerDown(1));
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        StartCoroutine(PowerDown(2));
    }

    IEnumerator PowerDown(int powerType)
    {
        yield return new WaitForSeconds(_durationPowerup);
        switch (powerType)
        {
            case 0:
                _isTripleShotActive = false;
                break;
            case 1:
                _isSpeedBoostActive = false;
                break;
            case 2:
                _isShieldActive = false;
                break;
        }
    }

    public void AddScore(int point)
    {
        _score += point;
        _uiManager.UpdateScore(_score);
    }
}