using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] IntVariable _playerHp;
    [SerializeField] int _initPlayerHp = 10;

    private void Awake()
    {
        _playerHp.m_value = _initPlayerHp;
    }


    private void Start()
    {
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
        }
    }
}
