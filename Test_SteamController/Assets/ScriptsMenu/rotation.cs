using UnityEngine;
using System.Collections;

public class rotation : MonoBehaviour {

		public float speed = 10f;

		void Update () {

			if (Input.GetKey("t")){
			transform.Rotate(Vector3.up, speed * Time.deltaTime);
		}
		if (Input.GetKey("r")){
			transform.Rotate(-Vector3.up, speed * Time.deltaTime);
		}
	}
}