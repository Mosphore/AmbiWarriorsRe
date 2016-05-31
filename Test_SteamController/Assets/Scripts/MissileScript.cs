using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MissileScript : NetworkBehaviour
{
    public float nbDegatSurBatiment = 20.0f;

    public GameObject effetExplosion;
    GameObject playerOwner;

    // Use this for initialization
    void Start()
    {
    }

    public void SetOwner(GameObject Owner)
    {
        playerOwner = Owner;
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        CmdInstancierEffetDestruction();
        if (collision.transform.tag == "Character" && collision.gameObject != playerOwner)
        {
            collision.transform.GetComponent<CharacterLife>().LoseLife(100);
        }
        else if (collision.transform.tag == "DestructBat")
        {
            collision.transform.GetComponent<DestructObject>().TakeDamage(nbDegatSurBatiment);
        }

        // pour Batiments Destructibles
       
    }

    [ClientRpc]
    void RpcEraseMissile()
    {
        transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<Rigidbody>().Sleep();
        transform.GetComponent<CapsuleCollider>().enabled = false;
    }

    [Command]
    void CmdInstancierEffetDestruction()
    {
        //correction pour instancier sur le sol
        Vector3 posInstanciate = transform.position;

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
        Destroy(explosion, 2.0f);
        transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<Rigidbody>().Sleep();
        transform.GetComponent<CapsuleCollider>().enabled = false;
        RpcEraseMissile();
        Destroy(gameObject,3.0f);
        // Destroy(fumee,10.0f);


    }
}

