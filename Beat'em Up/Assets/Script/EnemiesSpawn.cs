using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private List<GameObject> _spawnPoint;



    // Private & Protected
    private int _enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        _spawnPoint = new List<GameObject>();
        GameObject[] array = GameObject.FindGameObjectsWithTag("Spawn");
        foreach (var item in array)
        {
            _spawnPoint.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Spawn");
            _spawnPoint.Remove(gameObject);
           
            foreach (var item in _spawnPoint)
            {

                   _enemyCount++;
                    Instantiate(_enemyPrefab, item.transform.position, Quaternion.identity);

                



            }
           Destroy(this);
            
        }


    }
}
