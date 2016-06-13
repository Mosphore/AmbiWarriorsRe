using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MissileScript : NetworkBehaviour
{
    public float nbDegatSurBatiment = 20.0f;

    public GameObject effetExplosion;
    NetworkInstanceId playerOwner;
    bool originArm;

    // Use this for initialization
    void Start()
    {
    }

    public void SetOwner(NetworkInstanceId Owner, bool side)
    {
        playerOwner = Owner;
        originArm = side;
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (isServer)
        {
            Debug.Log("server : " + playerOwner);
            CmdInstancierEffetDestruction();
            if (collision.transform.tag == "Character")
            {
                GameObject g = NetworkServer.FindLocalObject(playerOwner);
                if(g!= null)
                g.GetComponent<Character>().callHitmarker(originArm);
                collision.transform.GetComponent<CharacterLife>().LoseLife(100, playerOwner);
            }
            else if (collision.transform.tag == "DestructBat")
            {
                collision.transform.GetComponent<DestructObject>().TakeDamage(nbDegatSurBatiment);
            }
        }
        // pour Batiments Destructibles
    }

    [ClientRpc]
    void RpcEraseMissile()
    {
        Destroy(transform.FindChild("FX_MissilleSparksTrail").gameObject);
        Destroy(transform.FindChild("FX_MissilleSmokeTrail").gameObject);
        transform.GetComponentInChildren<MeshRenderer>().enabled = false;
        transform.GetComponent<Rigidbody>().Sleep();
        transform.GetComponent<CapsuleCollider>().enabled = false;
    }

    //[Command]
    void CmdInstancierEffetDestruction()
    {
        //correction pour instancier sur le sol

        Vector3 posInstanciate = transform.position;

        //effet explosion
        GameObject explosion = (GameObject)Instantiate(
           effetExplosion,
          posInstanciate,
           Quaternion.identity);


        // spawn des effets
        //NetworkServer.Spawn(fumee);
        NetworkServer.Spawn(explosion);
        Destroy(explosion, 2.0f);
        transform.GetComponentInChildren<MeshRenderer>().enabled = false;
        transform.GetComponent<Rigidbody>().Sleep();
        transform.GetComponent<CapsuleCollider>().enabled = false;
        RpcEraseMissile();

        Destroy(gameObject, 3.0f);
    }
}

