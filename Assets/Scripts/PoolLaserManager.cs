using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolLaserManager : MonoBehaviour
{
    private List<GameObject> _pool;
    [SerializeField] private GameObject _laser;
    [SerializeField] private GameObject _player;
    static PoolLaserManager sharedInstance;

    public static PoolLaserManager GetInstance()
    {
        return sharedInstance;
    }

    private void Start()
    {
        _pool = new List<GameObject>();
    }

    private void Awake()
    {
        sharedInstance = this;
    }

    public GameObject GetLaserPool()
    {
        foreach (var pool in _pool)
        {
            if (!pool.gameObject.activeInHierarchy)
            {
                pool.gameObject.SetActive(true);
                return pool.gameObject;
            }

        }

        var laserTmp = Instantiate(_laser, _player.transform.position, Quaternion.identity);
        _pool.Add(laserTmp);
        return null;
    }
}