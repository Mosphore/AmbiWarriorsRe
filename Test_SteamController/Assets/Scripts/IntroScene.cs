using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroScene : MonoBehaviour
{

    float timer = 5;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}
