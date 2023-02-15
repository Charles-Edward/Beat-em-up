using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private int _heal;
    private PlayerBehaviour _playerBehaviour;


    void Start()
    {
        //_playerB = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Heal");
            Heal();
            Destroy(gameObject);
        }

    }

    private void Heal()
    {
        //_playerBehaviour._currentHealth += _heal;

    }
}
