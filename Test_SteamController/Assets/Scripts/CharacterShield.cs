using UnityEngine;
using UnityEngine.Networking;
using Rewired;
using System.Collections;

public class CharacterShield : NetworkBehaviour {

    public GameObject shieldPrefab;
    public Transform leftShieldSpawn, rightShieldSpawn;
    public int playerId = 0; // The Rewired player id of this character
    private Player player; // The Rewired Player

    bool dead = false;
    bool startGame = false;

    GameObject leftShield, rightShield;

    [SyncVar]
    Vector3 rightForward;
    [SyncVar]
    Vector3 leftForward;
    [SyncVar]
    Vector3 leftPosition;
    [SyncVar]
    Vector3 rightPosition;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
    }
    // Use this for initialization
    void Start ()
    {

    }

    public void StartGame()
    {
        startGame = true;
    }

    public void setDead(bool b)
    {
        dead = b;
    }

    [ClientRpc]
    void RpcSpawnLeftShield()
    {
        if (!isLocalPlayer)
        {
            leftShield = (GameObject)Instantiate(shieldPrefab, leftShieldSpawn.position, Quaternion.identity);
            //leftShield.transform.forward = leftShieldSpawn.forward;
        }
    }

    [Command]
    void CmdSpawnLeftShield()
    {
        if (leftShield == null)
        {
            //leftShield = (GameObject)Instantiate(shieldPrefab, leftShieldSpawn.position, Quaternion.identity);
            //leftShield.transform.forward = leftShieldSpawn.forward;
            RpcSpawnLeftShield();
            //NetworkServer.Spawn(leftShield);
        }
    }

    [ClientRpc]
    void RpcSpawnRightShield()
    {
        if (!isLocalPlayer)
        {
            rightShield = (GameObject)Instantiate(shieldPrefab, rightShieldSpawn.position, Quaternion.identity);
            //rightShield.transform.forward = rightShieldSpawn.forward;
        }
    }

    [Command]
    void CmdSpawnRightShield()
    {
        if (rightShield == null)
        {
            //rightShield = (GameObject)Instantiate(shieldPrefab, rightShieldSpawn.position, Quaternion.identity);
            //rightShield.transform.forward = rightShieldSpawn.forward;
            RpcSpawnRightShield();
            //NetworkServer.Spawn(rightShield);
        }   
    }

    [ClientRpc]
    void RpcDestroyLeftShield()
    {
        Destroy(leftShield);
    }

    [Command]
    void CmdDestroyLeftShield()
    {
        Destroy(leftShield);
        RpcDestroyLeftShield();
    }

    [ClientRpc]
    void RpcDestroyRightShield()
    {
        Destroy(rightShield);
    }

    [Command]
    void CmdDestroyRightShield()
    {
        Destroy(rightShield);
        RpcDestroyRightShield();
    }

    [Command]
    void CmdSyncLeftShieldPosition(Vector3 position, Vector3 forward)
    {
        leftPosition = position;
        leftForward = forward;
    }

    [Command]
    void CmdSyncRightShieldPosition(Vector3 position, Vector3 forward)
    {
        rightPosition = position;
        rightForward = forward;
    }

    void FixedUpdate()
    {
        if (leftShield != null)
        {
            leftShield.transform.position = leftShieldSpawn.position;
            leftShield.transform.forward = leftShieldSpawn.forward;
        }
        if (rightShield != null)
        {
            rightShield.transform.position = rightShieldSpawn.position;
            rightShield.transform.forward = rightShieldSpawn.forward;
        }
        TransmitShieldPos();
    }

    [Client]
    void TransmitShieldPos()
    {
        if (isLocalPlayer)
        {
            //if (rightShield != null)
                CmdSyncRightShieldPosition(rightShieldSpawn.transform.position, rightShieldSpawn.transform.forward);
            //if (leftShield != null)
                CmdSyncLeftShieldPosition(leftShieldSpawn.transform.position, leftShieldSpawn.transform.forward);
        }
    }

    void LateUpdate()
    {  
        if(dead && leftShield != null)
        {
            CmdDestroyLeftShield();
            Destroy(leftShield);
        }
        if (dead && rightShield != null)
        {
            CmdDestroyRightShield();
            Destroy(rightShield);
        }

        if (!isLocalPlayer)
        {
            if (leftShield != null)
            {
                leftShield.transform.position = leftPosition;
                leftShield.transform.forward = leftForward;
            }
            if (rightShield != null)
            {
                rightShield.transform.position = rightPosition;
                rightShield.transform.forward = rightForward;
            }
        }
    }
    // Update is called once per frame
    void Update ()
    {
        if (isLocalPlayer && startGame)
        {
            if (player.GetButtonDown("LeftShield") && !dead)
            {
                GetComponent<CharacterMove>().SetShieldSpeed(true);
                GetComponent<Character>().setCanShootLeft(false);
                CmdSpawnLeftShield();
                leftShield = (GameObject)Instantiate(shieldPrefab, leftShieldSpawn.position, Quaternion.identity);
                //leftShield.transform.forward = leftShieldSpawn.forward;
            }
            if (player.GetButtonDown("RightShield") && !dead)
            {
                GetComponent<CharacterMove>().SetShieldSpeed(true);
                GetComponent<Character>().setCanShootRight(false);
                CmdSpawnRightShield();
                rightShield = (GameObject)Instantiate(shieldPrefab, rightShieldSpawn.position, Quaternion.identity);
                //rightShield.transform.forward = rightShieldSpawn.forward;
            }

            if (player.GetButtonUp("LeftShield") && !dead)
            {
                if(rightShield == null)
                GetComponent<CharacterMove>().SetShieldSpeed(false);

                GetComponent<Character>().setCanShootLeft(true);
                CmdDestroyLeftShield();
                Destroy(leftShield);
            }
            if (player.GetButtonUp("RightShield") && !dead)
            {
                if (leftShield == null)
                    GetComponent<CharacterMove>().SetShieldSpeed(false);

                GetComponent<Character>().setCanShootRight(true);
                CmdDestroyRightShield();
                Destroy(rightShield);
            }
        }
    }
}
