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
    [SerializeField] private GameObject _engineLeft;
    [SerializeField] private GameObject _engineRight;
    [SerializeField] private GameObject _explode;
    [SerializeField] private GameObject _readyPosition;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _live = 3;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private float _durationPowerup = 3.0f;
    [SerializeField] private AudioClip _laserShotClip;
    public SpriteRenderer spriteRenderer;
    public Sprite spriteTurnLeft;
    public Sprite spriteTurnRight;
    public Sprite spriteIdle;
    private AudioSource _laserShotAudio;
    private bool _isReady;
    private float _canFire = 0.0f;
    private SpawnManager _spawnManager;
    [SerializeField] private int _score = 0;
    private UIManager _uiManager;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -6f, 0);
        _isReady = true;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _laserShotAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer is NULL");
        }
        else
        {
            spriteRenderer.sprite = spriteIdle;
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager not found!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager not found!");
        }

        if (_laserShotAudio == null)
        {
            Debug.LogError("The Audio is Null");
        }
        else
        {
            _laserShotAudio.clip = _laserShotClip;
        }

        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ready();
        if (!_isReady)
        {
            CalculateMovement();
            Shooting();
        }

        _shield.SetActive(_isShieldActive);
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeedBoostActive)
        {
            transform.Translate(direction * _speed * _multipleSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        if (transform.position.y > 5.5f)
        {
            transform.position = new Vector3(transform.position.x, 5.5f, 0);
        }
        else if (transform.position.y < -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);
        }

        if (transform.position.x > 9.88f)
        {
            transform.position = new Vector3(9.88f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.88f)
        {
            transform.position = new Vector3(-9.88f, transform.position.y, 0);
        }

        CheckDirectionMove();
    }

    void CheckDirectionMove()
    {
        _animator.ResetTrigger("OnTurnLeft");
        _animator.ResetTrigger("OnTurnRight");
        _animator.SetBool("Default", false);
        if (Input.GetAxis("Horizontal") > 0)
        {
            _animator.SetTrigger("OnTurnRight");
            spriteRenderer.sprite = spriteTurnRight;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            _animator.SetTrigger("OnTurnLeft");
            spriteRenderer.sprite = spriteTurnLeft;
        }
        else
        {
            _animator.SetBool("Default", true);
            spriteRenderer.sprite = spriteIdle;
        }
    }

    void Ready()
    {
        float dist = Vector3.Distance(transform.position, _readyPosition.transform.position);
        if (_isReady)
        {
            transform.position = Vector3.MoveTowards(transform.position, _readyPosition.transform.position,
                Time.deltaTime *
                (dist + 1));
        }

        if (Mathf.Round(dist) == 0)
        {
            _isReady = false;
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
                GameObject laser = PoolLaserManager.GetInstance().GetLaserPool();
                if (laser != null)
                {
                    laser.transform.position = transform.position;
                    laser.SetActive(true);
                }
            }

            _laserShotAudio.Play();
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
        if (_live == 2)
        {
            _engineLeft.gameObject.SetActive(true);
        }
        else if (_live == 1)
        {
            _engineRight.gameObject.SetActive(true);
        }

        _uiManager.UpdateLives(_live);
        if (_live < 1)
        {
            Destroy(this.gameObject);
            Instantiate(_explode, transform.position, Quaternion.identity);
            _spawnManager.OnPlayerDeath();
        }
    }

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