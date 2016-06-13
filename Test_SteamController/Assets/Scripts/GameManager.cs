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
        Debug.Log("GameManagerStart");
        //DontDestroyOnLoad(gameObject);
        otherScores = new Score[2];
        otherScores[0].kills = otherScores[0].deaths = otherScores[1].kills = otherScores[1].deaths;
        myScore.kills = 0;
        myScore.deaths = 0;
        one = two = three = false;

        PlayersTab = new Dictionary<NetworkInstanceId, int>();
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
        Debug.Log("DeadPlayer");

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
        Debug.Log("killerId : " + killerId.Value);
        Debug.Log("deadId : " + deadId.Value);
        Debug.Log("myId : " + player.GetComponent<NetworkIdentity>().netId.Value);

        if (player.GetComponent<NetworkIdentity>().netId.Value != deadId.Value)
        {
            otherScores[PlayersTab[deadId]].deaths++;

            Debug.Log("SomeoneIsDead" + deadGuys);
        }
        else
        {
            myScore.deaths++;
            Debug.Log("ImDead" + deadGuys);
        }

        if(player.GetComponent<NetworkIdentity>().netId != killerId)
        {
            otherScores[PlayersTab[killerId]].kills++;
            Debug.Log("SomeoneKilledSomebody");
        }
        else
        {
            myScore.kills++;
            Debug.Log("IKilledSomebody");
        }

        GameObject.Find("killscreen").GetComponent<changescore>().SetMyScore(myScore);
        GameObject.Find("killscreen").GetComponent<changescore>().SetOtherScore(otherScores[0], otherScores[1]);
    }

    public void ConnectPlayer(NetworkInstanceId id)
    {
        connectedPlayers++;
        Debug.Log("SERVER : PlayerConnected : " + connectedPlayers);
        //RpcSyncPlayer(id);
        if (connectedPlayers == 3)
        {
            Debug.Log("SERVER : All Players IN, Starting Countdown");
            RpcStartGame();
        }
    }

    [ClientRpc]
    void RpcStartGame()
    {
        Debug.Log("CLIENT : StartingClientCountdown");
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
                three = true;
                Debug.Log("Start In 3");
            }
            if (two == false && startTimer <= 2)
            {
                two = true;
                Debug.Log("Start In 2");
            }
            if (one == false && startTimer <= 1)
            {
                one = true;
                Debug.Log("Start In 1");
            }
            startTimer -= Time.deltaTime;
        }

        if (startTimer <= 0 && !gameStarted)
        {
            GameObject[] g = GameObject.FindGameObjectsWithTag("Character");
            for (int i = 0; i < g.Length; ++i)
            {
                if (g[i] != player)
                {
                    PlayersTab.Add(g[i].GetComponent<NetworkIdentity>().netId, otherPlayerNumber);

                    otherPlayerNumber++;
                }
            }
            Debug.Log("START, added " + otherPlayerNumber + " players to score array");
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
