using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjectControler : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _distributor;

    [SerializeField]
    private SpriteRenderer _image;

    [SerializeField]
    private GameObject _healthCanPrefab;

    [SerializeField]
    private GameObject _vendingMachine;

    // Private & Protected
    private int _nbDamage;
    private Transform _position;
    

    private void Awake()
    {
        _position = _vendingMachine.transform;

        //_vendingMachine = transform.position;

    }
    void Start()
    {

        Debug.Log(_vendingMachine);

    }


    void Update()
    {
        _image.sprite = _distributor[_nbDamage];
        
    }




    private void OnTriggerEnter2D(Collider2D collision) 
    {
        
        if (collision.CompareTag("HitBoxPlayer"))
        {
            Debug.Log("Tape La machine");
            _nbDamage++;
            if(_nbDamage >= _distributor.Length)
            {
                Debug.Log("Canette");
                Instantiate(_healthCanPrefab, _position.position, Quaternion.identity);
                //gameObject.transform.DetachChildren();
                Destroy(gameObject);
                
            }



        }
        

    }


}
