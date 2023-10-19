using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    // private and public variables
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _multipleSpeed = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotLaserPrefab;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private int _live = 3;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private float _durationPowerup = 5.0f;
    private float _canFire = 0.0f;
    private float _bouncing = 0.1f;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 1, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        Shooting();
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

    void Damege()
    {
            _live--;
        if (_live < 1)
        {
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("enemy"))
        {
            Damege();
            Destroy(other.transform.gameObject);
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
            default:
                break;
        }
    }

}