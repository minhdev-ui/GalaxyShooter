using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _speed = 1.5f;
    private bool _isExplode;
    private Player _player;
    private Animator _anim;
    [SerializeField] private AudioClip _enemySoundExplode;
    private AudioSource _enemyExplodeAudio;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemyExplodeAudio = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("The Player not found!");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animation not available!");
        }

        if (_enemyExplodeAudio == null)
        {
            Debug.LogError("The audio source is NULL");
        }
        else
        {
            _enemyExplodeAudio.clip = _enemySoundExplode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6f)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 6.5f,
                0f);
            transform.position = posToSpawn;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Laser"))
        {
            if (_player != null && !_isExplode)
            {
                other.gameObject.SetActive(false);
                _player.AddScore(10);
                _enemyExplodeAudio.Play();
            }

            _isExplode = true;
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }

        if (other.CompareTag("Player") && !_isExplode)
        {
            _isExplode = true;
            _player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
            _enemyExplodeAudio.Play();
        }
    }
}