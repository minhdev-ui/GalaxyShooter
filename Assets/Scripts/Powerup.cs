using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triple_Shot_Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 1.5f;

    // Start is called before the first frame update
    [SerializeField] private int powerupID;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}