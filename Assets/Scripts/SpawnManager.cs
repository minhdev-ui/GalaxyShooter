using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUp;
    [SerializeField] private GameObject _powerUpContainer;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private GameObject _asteroidContainer;
    private bool _stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(SpawnAsteroidRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 6f,
                0f);
            GameObject newEnemy = Instantiate(_enemy, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnAsteroidRoutine()
    {
        while (!_stopSpawning)
        {
            yield return new WaitForSeconds(3.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 6f,
                0f);
            GameObject newAsteroid = Instantiate(_asteroid, posToSpawn, Quaternion.identity);
            newAsteroid.transform.parent = _asteroidContainer.transform;
            yield return new WaitForSeconds(6.0f);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (!_stopSpawning)
        {
            yield return new WaitForSeconds(15.0f);
            Debug.Log(_powerUp.Length);
            int randomPowerup = Random.Range(0, 3);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 6f,
                0f);
            GameObject newPowerUp = Instantiate(_powerUp[randomPowerup], posToSpawn, Quaternion.identity);
            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(15.0f);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}