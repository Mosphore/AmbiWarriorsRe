using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
/// Les liens avec les particules doivent être fait IG
/// La taille doit être remplie afin que l'instanciation soit faite
/// au niveau du sol de l'objet destructible
/// L'objet doit être mis dans la map avant la partie sinon il 
/// n'a pas l'autorité "serveur"
/// 
/// reste a link un son
/// </summary>

public class DestructObject : NetworkBehaviour {

    const float MAXLIFE = 100.0f;
    [SyncVar]
    public float life ;

    // sert à calculer l'endroit du sol par rapport à l'objet
    private float tailleObjet;
    //public GameObject effetFumee;
    public GameObject effetExplosion;


    // Use this for initialization
    void Start () {
        life = MAXLIFE;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(float damage)
    {
        if (!isServer)
            return;

        life -= damage;
        if(life <= 0)
        {
            CmdInstancierEffetDestruction();
        }
    }

    // pour instancier les effets de particule

    void CmdInstancierEffetDestruction()
    {
        //correction pour instancier sur le sol
        Vector3 posInstanciate = transform.position;
        posInstanciate.y -= 6.5f;

        //effet de fumee
        //GameObject fumee = (GameObject)Instantiate(
        //   effetFumee,
        //   posInstanciate,
        //   Quaternion.Euler(270,0,0));

        //effet explosion
        GameObject explosion = (GameObject)Instantiate(
           effetExplosion,
          posInstanciate,
           Quaternion.identity);


        // spawn des effets
        //NetworkServer.Spawn(fumee);
        NetworkServer.Spawn(explosion);
        Destroy(gameObject, 0.3f);
       // Destroy(fumee,10.0f);
        Destroy(explosion,2.0f);


    }

}
