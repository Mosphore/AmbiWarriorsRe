using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnnemyViewer : NetworkBehaviour
{

    GameObject[] playerArray;
    public Camera camPlayer, camLeftAim, camRightAim;
    public Texture2D target, healthContour, healthBar;

    Vector3 screenPos;
    Vector3 targetPos;

    //Vector2
    // Use this for initialization
    void Start()
    {
        
    }

    public void getConnectedPlayers()
    {

    }

    void OnGUI()
    {
        if (isLocalPlayer)
        {
            playerArray = GameObject.FindGameObjectsWithTag("Character");
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
                        if (Physics.Raycast(camPlayer.transform.position, g.transform.FindChild("RaycastTarget").position - camPlayer.transform.position, out hit))
                        {
                            if (hit.transform.tag == "Character")
                            {
                                //float distanceRatio;
                                //Vector3 vecToTarget = targetPos - transform.position;
                                //distanceRatio = 1.5f / vecToTarget.magnitude;
                                //Debug.Log(distanceRatio);

                                //if (distanceRatio < Screen.width /12)
                                //    distanceRatio = Screen.width /12;

                                //if (distanceRatio > Screen.width / 8)
                                //    distanceRatio = Screen.width / 8;

                                //GUI.DrawTexture(new Rect(screenPos.x - target.width / 2 /*- targetWidth*/, screenPos.y - target.height / 2 /*- targetHeight*/, target.width, target.height), target);
                                GUI.DrawTexture(new Rect(screenPos.x - Screen.width / 17,
                                                         screenPos.y - Screen.width / 16,
                                                         Screen.width / 8.5f, Screen.width / 8), healthContour);
                                
                                for (int i = 0; i < g.GetComponent<CharacterLife>().life / 125; ++i)
                                {
                                    //LES CHIFFRES APRES C'EST DE LA MAGIE!!!
                                    GUI.DrawTexture(new Rect(screenPos.x - Screen.width / 16 + (Screen.width / 8 / 10) + i * (Screen.width / 8 / 10),
                                                         screenPos.y - (Screen.width / 8) / 8.75f,
                                                         (Screen.width/8) /8, (Screen.width / 8) / 8), healthBar);
                                }

                                //GUI.DrawTexture(new Rect(screenPos.x - target.width / 2 - targetWidth, screenPos.y - target.height / 2 + targetHeight, target.width , target.height), target);
                                //GUI.DrawTexture(new Rect(screenPos.x - target.width / 2 + targetWidth, screenPos.y - target.height / 2 - targetHeight, target.width , target.height), target);
                            }
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
