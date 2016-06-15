using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour{

    public struct Score
    {
        public int kills;
        public int deaths ;
    }

    public AudioClip bip;
    public AudioClip tut;

    Dictionary<NetworkInstanceId, int> PlayersTabServer;
    Dictionary<NetworkInstanceId, int> PlayersTab;

    Score[] otherScores;
    Score myScore;
    int otherPlayerNumber = 0;

    NetworkInstanceId[] playerIDtab;
    public GameObject player;

    bool startGame = false;
    float startTimer = 3;
    bool gameStarted = false;

    //countdown
    bool one, two, three;
    public Texture2D texThree, texTwo, texOne, texGo, texWaiting;
    float displayGoTimer = 2;

    //endGame
    int deadGuys = 0;
    float deadGuysTimer = 3;

    [SyncVar]
    int connectedPlayers = 0;

	// Use this for initialization
	void Start ()
    {
        //DontDestroyOnLoad(gameObject);
        otherScores = new Score[2];
        otherScores[0].kills = otherScores[0].deaths = otherScores[1].kills = otherScores[1].deaths;
        myScore.kills = 0;
        myScore.deaths = 0;
        one = two = three = false;

        Debug.Log("UPDATESCOREBEFORE");
        PlayersTab = new Dictionary<NetworkInstanceId, int>();

        if(isServer)
        PlayersTabServer = new Dictionary<NetworkInstanceId, int>();
    }

    public void DeadPlayer(NetworkInstanceId killerId, NetworkInstanceId deadId )
    {
        int i;
        if(PlayersTabServer.TryGetValue(deadId, out i))
        {
            PlayersTabServer[deadId]++;
            if (PlayersTabServer[deadId] == 3)
                deadGuys++;
        }
        else
        {
            PlayersTabServer.Add(deadId, 1);
        }
        RpcUpdateScores(killerId, deadId);

        if (deadGuys >= 2)
        {
            RpcEndGame();
        }
    }

    [ClientRpc]
    void RpcEndGame()
    {
        deadGuys = 2;

    }

    [ClientRpc]
    void RpcUpdateScores(NetworkInstanceId killerId, NetworkInstanceId deadId)
    {
       

        if (player.GetComponent<NetworkIdentity>().netId.Value != deadId.Value)
        {
            Debug.Log("UPDATESCOREBEFORE");
            int i = 0;
            if(PlayersTab.TryGetValue(deadId, out i))
                otherScores[i].deaths++;
            else
                Debug.Log("ERROR : CANT FIND PLAYER SCORE");

        }
        else
        {
            myScore.deaths++;
        }

        if(player.GetComponent<NetworkIdentity>().netId != killerId)
        {
            int j = 0;
            if (PlayersTab.TryGetValue(killerId, out j))
                otherScores[j].kills++;
            else
                Debug.Log("ERROR : CANT FIND PLAYER SCORE");
        }
        else
        {
            myScore.kills++;
        }

        GameObject.Find("killscreen").GetComponent<changescore>().SetMyScore(myScore);
        GameObject.Find("killscreen").GetComponent<changescore>().SetOtherScore(otherScores[0], otherScores[1]);
    }

    public void ConnectPlayer(NetworkInstanceId id)
    {
        connectedPlayers++;
        //RpcSyncPlayer(id);
        if (connectedPlayers == 3)
        {
            RpcStartGame();
        }
    }

    [ClientRpc]
    void RpcStartGame()
    {
        startGame = true;
        //player.GetComponent<Character>().StartGame();
    }

    void OnGUI()
    {
        if (displayGoTimer > 0)
        {
            if(!startGame)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - Screen.width / 8, Screen.height / 2 - Screen.width / 10, Screen.width / 4, Screen.width / 5), texWaiting);
            }
            else if(gameStarted)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - Screen.width / 20, Screen.height / 2 - Screen.width / 20, Screen.width / 10, Screen.width / 10), texGo);
            }
            else if (one)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - Screen.width / 20, Screen.height / 2 - Screen.width / 20, Screen.width / 10, Screen.width / 10), texOne);
            }
            else if (two)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - Screen.width / 20, Screen.height / 2 - Screen.width / 20, Screen.width / 10, Screen.width / 10), texTwo);
            }
            else if (three)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - Screen.width / 20, Screen.height / 2 - Screen.width / 20, Screen.width / 10, Screen.width / 10), texThree);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame && startTimer > 0)
        {
            if (three == false && startTimer == 3)
            {
                GetComponent<AudioSource>().PlayOneShot(tut);
                three = true;
            }
            if (two == false && startTimer <= 2)
            {
                GetComponent<AudioSource>().PlayOneShot(tut);
                two = true;
            }
            if (one == false && startTimer <= 1)
            {
                GetComponent<AudioSource>().PlayOneShot(tut);
                one = true;
            }
            startTimer -= Time.deltaTime;
        }

        if (startTimer <= 0 && !gameStarted)
        {
            GetComponent<AudioSource>().PlayOneShot(bip);
            GameObject[] g = GameObject.FindGameObjectsWithTag("Character");
            for (int i = 0; i < g.Length; ++i)
            {
                if (g[i] != player)
                {
                    Debug.Log("BEFORE");
                    PlayersTab.Add(g[i].GetComponent<NetworkIdentity>().netId, otherPlayerNumber);

                    Debug.Log("AFTER");

                    otherPlayerNumber++;
                }
            }
            player.GetComponent<Character>().StartGame();
            player.GetComponent<CharacterShield>().StartGame();
            player.GetComponent<CharacterMove>().StartGame();
            gameStarted = true;
        }

        if (gameStarted && displayGoTimer > 0)
        {
            displayGoTimer -= Time.deltaTime;
        }

        if (isServer)
        {
            if (deadGuys >= 2 && deadGuysTimer > 0)
                deadGuysTimer -= Time.deltaTime;

            if (deadGuysTimer <= 0 )
            {
                GameObject.Find("NetworkManager").GetComponent<NetworkManager_Custom>().StopHost();
            }
        }
    }
}
