using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDamage : MonoBehaviour
{
    [SerializeField] IntVariable _dataInt;
    // Start is called before the first frame update
    private void Awake()
    {
        _currentHealth = _dataInt.enemie_health;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerBehaviour playerHealth = collision.transform.GetComponent<PlayerBehaviour>();
            playerHealth.TakeDamage(50);
        }
    }

    [SerializeField] private int _currentHealth;
}
