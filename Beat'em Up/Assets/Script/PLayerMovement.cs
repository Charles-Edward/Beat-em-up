//  []


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerMovement : MonoBehaviour
{ 
    [SerializeField]
    private float _moveSpeed = 3f;

    [SerializeField]
    private float _runSpeed = 6f;




    // Private & Protected
    private Rigidbody2D _rb2D;
    private Vector2 _direction;



    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();


    }


    void Start()
    {
        
    }





    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        _rb2D.velocity = _direction;
        Move();

    }

    // Méthode pour déplacer le player
    private void Move()
    {
        _direction.x = Input.GetAxisRaw("Horizontal") * _moveSpeed;
        _direction.y = Input.GetAxisRaw("Vertical") * _moveSpeed;


    }





}
