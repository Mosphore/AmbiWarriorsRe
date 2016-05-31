using UnityEngine; using System.Collections;

public class ShakeCamera : MonoBehaviour { 

	public bool Shaking; 
	private float ShakeDecay; 
	private float ShakeIntensity;
	private Vector3 OriginalPos;
	private Quaternion OriginalRot;
	public float Intensity = 0.05f;

    RaycastHit[] oldHits;
	
	void Start()
	{
		Shaking = false;
		OriginalRot = transform.rotation;

	}
	
	
	// Update is called once per frame
	void Update ()
	{
        //RaycastHit[] hits;
        //hits = Physics.RaycastAll(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position - transform.position, 100.0f/*Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position)*/);

        //for (int i = 0; i < hits.Length; i++)
        //{
        //    RaycastHit hit = hits[i];
        //    Renderer rend = hit.transform.GetComponent<Renderer>();


        //    if (rend)
        //    {
               
        //        //Color oldtempColor = oldrend.material.color;
        //        //oldtempColor.a = 1.0F;
        //        //oldrend.material.color = oldtempColor;

        //        // Change the material of all hit colliders
        //        // to use a transparent shader.
        //        //rend.material.shader = Shader.Find("Transparent/Diffuse");
        //        Color tempColor = rend.material.color;
        //        tempColor.a = 0.3F;
        //        rend.material.color = tempColor;
        //        oldrend = rend;
        //    }
        //}

        if (ShakeIntensity > 0)
		{

			transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
			                                    OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
			                                    OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
			                                    OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f);
			
			ShakeIntensity -= ShakeDecay;
		}
		else if (Shaking)
		{
			Shaking = false;
		}

	}


	/*void OnGUI() {
		
		if (GUI.Button(new Rect(10, 200, 50, 30), "Shake"))
			DoShake(Intensity);
	} */

	public void DoShake(float Intensity)
	{


		
		ShakeIntensity = Intensity;
		ShakeDecay = 0.001f;
		Shaking = true;
	}
	
}