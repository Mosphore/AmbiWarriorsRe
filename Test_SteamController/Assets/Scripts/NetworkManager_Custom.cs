using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager_Custom : NetworkManager
{

    public void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputIP").transform.FindChild("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    public string GetIP()
    {
        Debug.Log(NetworkManager.singleton.networkAddress);
        return NetworkManager.singleton.networkAddress;
    }

    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    GameObject player = (GameObject)Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
    //    player.GetComponent<SetCharacterColor>().SetColor(robotColor);
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    //}

}
