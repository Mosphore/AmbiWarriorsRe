using UnityEngine;
using System.Collections;

public class PlayRandomSound : MonoBehaviour {

    public AudioClip[] soundArray;

	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().clip = soundArray[Random.Range(0, soundArray.Length - 1)];
        GetComponent<AudioSource>().Play();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
