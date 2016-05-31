using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnnemyViewer : NetworkBehaviour
{

    GameObject[] playerArray;
    public Camera camPlayer, camLeftAim, camRightAim;
    public Texture2D target;

    Vector3 screenPos;
    Vector3 targetPos;

    //Vector2
    // Use this for initialization
    void Start()
    {
        playerArray = GameObject.FindGameObjectsWithTag("Character");

        foreach (GameObject g in playerArray)
        {
            g.GetComponent<EnnemyViewer>().getConnectedPlayers();
        }
    }

    public void getConnectedPlayers()
    {
        if (isLocalPlayer)
        {
            playerArray = GameObject.FindGameObjectsWithTag("Character");

            Debug.Log(playerArray.Length);
        }
    }

    void OnGUI()
    {
        if (isLocalPlayer)
        {
            foreach (GameObject g in playerArray)
            {
                if (g != gameObject && g!= null)
                {
                    targetPos = g.transform.FindChild("Target").transform.position;
                    screenPos = camPlayer.WorldToViewportPoint(targetPos);

                    if (screenPos.x < 0)
                    {
                        screenPos = camLeftAim.WorldToViewportPoint(targetPos);
                        screenPos.x = camLeftAim.rect.x * Screen.width + Screen.width * camLeftAim.rect.width * screenPos.x;
                    }
                    else if (screenPos.x > 1)
                    {
                        screenPos = camRightAim.WorldToViewportPoint(targetPos);
                        screenPos.x = camRightAim.rect.x * Screen.width + Screen.width * camRightAim.rect.width * screenPos.x;
                    }
                    else
                        screenPos.x = camPlayer.rect.x * Screen.width + Screen.width * camPlayer.rect.width * screenPos.x;

                    screenPos.y = Screen.height - Screen.height * screenPos.y;

                    if(screenPos.z >0)
                    {
                        RaycastHit hit;
                        Physics.Raycast(camPlayer.transform.position, targetPos - camPlayer.transform.position, out hit);
                        if(hit.transform.tag == "Character")
                        {
                            GUI.DrawTexture(new Rect(screenPos.x - target.width / 2, screenPos.y - target.height / 2, target.width, target.height), target);
                        }
                    }
                }
            }
        }


    }
    // Update is called once per frame
    void Update()
    {

        
    }
}
