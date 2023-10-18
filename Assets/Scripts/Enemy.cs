using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _speed = 1.5f;
    void Start()
    {
        transform.position = new Vector3(-3.24f, 0, 10.53f);
        transform.localScale = new Vector3(1, 3, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(_player.transform.position * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit by: " + other.transform.name);   
    }
}
