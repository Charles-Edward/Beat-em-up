using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        print("Sc�ne recharg�e");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Play()
    {
        print("Sc�ne 1");
        SceneManager.LoadScene("Level1");
    }

    public void Victory()
    {
        print("Victory");
        SceneManager.LoadScene("Victory");

    }
}
