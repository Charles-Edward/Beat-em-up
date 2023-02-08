using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{




    void Start()
    {



        
    }

    void Update()
    {




        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            SendMessageUpwards("PlayerDetected");


        }



    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            SendMessageUpwards("PlayerEscaped");
        }


    }



}
