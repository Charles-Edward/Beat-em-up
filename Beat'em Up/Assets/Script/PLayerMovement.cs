//  []


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 3f;

    [SerializeField]
    private float _runSpeed = 1.3f;




    // Private & Protected
    private Rigidbody2D _rb2D;
    private Vector2 _direction;



    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        z = transform.Find("Sprite Renderer");

    }

    void Start()
    {

    }

    void Update()
    {
        Move();
    }


    private void FixedUpdate()
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


    // Méthode pour déplacer le player
    private void Move()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");

    }

    private void Flip()
    {
        if (_direction.x < 0)
        {
        } 
        else if (_direction.x > 0) 
        {
        
        }

    }

    private Transform z;
}
