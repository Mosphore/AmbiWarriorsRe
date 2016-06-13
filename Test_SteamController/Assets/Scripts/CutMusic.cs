using UnityEngine;
using System.Collections;

public class CutMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.M))
        {
            if(GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Pause();
            }
            else
                GetComponent<AudioSource>().Play();
        }
	
	}
}
