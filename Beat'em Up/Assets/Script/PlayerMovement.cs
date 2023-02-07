//  []


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 70f;

    [SerializeField]
    private float _runSpeed = 1.3f;

    // Private & Protected
    private Rigidbody2D _rb2D;
    private Vector2 _direction;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb2D = GetComponent<Rigidbody2D>();
        flip = GetComponentInChildren<SpriteRenderer>();
        isStatic = false;
    }

    void Start()
    {
    }

    void Update()
    {
        //Move();

    }
    private void FixedUpdate()
    {
        /*
        if (isStatic)
        {
            Debug.Log("static");
            _rb2D.velocity = _direction.normalized * 0 * Time.fixedDeltaTime;

        }
        else
        {
            if (Input.GetButton("Fire3"))
            {
                _rb2D.velocity = _direction.normalized * (_moveSpeed * _runSpeed) * Time.fixedDeltaTime;

            }
            else
            {
                _rb2D.velocity = _direction.normalized * _moveSpeed * Time.fixedDeltaTime;
            }
        }
        */

    }


    // Méthode pour déplacer le player
    /*
    private void Move()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isStatic = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isStatic = false;
        }
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");


        Flip();
    }


private void Flip()
    {
        if (_direction.x < 0)
        {
            flip.flipX = true;

        }
        else if (_direction.x > 0)
        {
            flip.flipX = false;
        }

    }
*/

    private Animator _animator;
    private SpriteRenderer flip;
    private bool isStatic;

}
