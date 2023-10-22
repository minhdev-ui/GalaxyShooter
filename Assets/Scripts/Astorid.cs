using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astorid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 3.5f;
    private Player _player;
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private GameObject _explode;

    [SerializeField] private int _live = 4;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player not found!");
        }

        float randomScale = Random.Range(0.2f, 0.5f);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        _explode.transform.localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    void OnDamage()
    {
        _live--;
        if (_live < 1)
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _speed = 0;
            Instantiate(_explode, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.01f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Laser"))
        {
            other.gameObject.SetActive(false);
            OnDamage();
        }

        if (other.CompareTag("Player"))
        {
            _player.Damage();

            Instantiate(_explode, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.01f);
        }
    }
}