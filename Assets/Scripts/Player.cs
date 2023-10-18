using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // private and public variables
    [SerializeField] private float _speed = 5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.1f;
    private float _canFire = 0.0f;
    private float _bouncing = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 1, 0);
        transform.localScale = new Vector3(1, 1, 3.25f);
        Camera.main.transform.position = new Vector3(3, 2, -2);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        Shooting();
        // BouncingMovement();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontalInput, 0, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        Camera.main.transform.Translate(direction * _speed * Time.deltaTime);
        // BouncingMovement(0.15f, 0.4f);
    }

    void Shooting()
    {
        if (Input.GetMouseButton(0) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }
    }

    void BouncingMovement(float duration, float magnitude)
    {
        Vector3 camOriginalPosition = Camera.main.transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float y = Random.Range(-1f, 1f) * magnitude;
            Camera.main.transform.position = new Vector3(camOriginalPosition.x, y, camOriginalPosition.z);
        }

        Camera.main.transform.position = camOriginalPosition;
    }
}