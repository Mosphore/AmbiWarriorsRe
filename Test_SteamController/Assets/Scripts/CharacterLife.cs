using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CharacterLife : NetworkBehaviour
{

    [SyncVar]
    public float life = 1000;
    [SyncVar]
    int stocks = 3;
    float prevLife = 1000;

    public GameObject deathParticle;

    Texture2D texHealth = null;
    GUIStyle texHealthStyle = null;

    Texture2D texStocks = null;
    GUIStyle texStocksStyle = null;

    public Texture2D texStocksOthers;

    Texture2D texFadeBlack = null;
    GUIStyle texFadeBlackStyle = null;

    float x, y, size;
    public float xlife1, xlife2, ylife1, ylife2, sizelife1, sizelife2;

    Animator charAnim;

    public Texture2D cockpit;

    bool displayHUD = true;
    float fadeBlackAlpha = 0;

    Transform camFPS, camFPSLeft, camFPSRight, camTPS;
    public Transform camParentTransform;
    public GameObject armorTop; // on desactive l'affichage de l'armure pour le joueur, pasque sinon elle est devant sa camera, c'est pour placer la cam plus ou on veut

    void Start()
    {
        camFPS = camParentTransform.FindChild("CamFPS");
        camFPSLeft = camParentTransform.FindChild("CamFPSLeft");
        camFPSRight = camParentTransform.FindChild("CamFPSRight");

        camTPS = transform.FindChild("CamTPS");

        charAnim = GetComponent<Animator>();
        x = Screen.width / 5.5f;
        y = Screen.height - Screen.height / 7.0f;
        size = Screen.height / 5.8f;
        if (texHealth == null)
            texHealth = new Texture2D(1, 1);

        if (texHealthStyle == null)
            texHealthStyle = new GUIStyle();

        texHealth.SetPixel(0, 0, Color.green);
        texHealth.Apply();
        texHealthStyle.normal.background = texHealth;

        if (texFadeBlack == null)
            texFadeBlack = new Texture2D(1, 1);

        if (texFadeBlackStyle == null)
            texFadeBlackStyle = new GUIStyle();

        texFadeBlack.SetPixel(0, 0, new Color(0,0,0,0));
        texFadeBlack.Apply();
        texFadeBlackStyle.normal.background = texFadeBlack;

        if (texStocks == null)
            texStocks = new Texture2D(1, 1);

        if (texStocksStyle == null)
            texStocksStyle = new GUIStyle();

        texStocks.SetPixel(0, 0, Color.white);
        texStocks.Apply();
        texStocksStyle.normal.background = texStocks;

    }

    [ClientRpc]
    void RpcDie(NetworkInstanceId id)
    {
        if (isLocalPlayer)
        {
            charAnim.SetBool("Dead", true);
            camFPS.GetComponent<Camera>().enabled = false;
            camFPSLeft.GetComponent<Camera>().enabled = false;
            camFPSRight.GetComponent<Camera>().enabled = false;
            camTPS.GetComponent<Camera>().enabled = true;
            displayHUD = false;
            GetComponent<Character>().setDead(true);
            GetComponent<CharacterMove>().setDead(true);
            armorTop.SetActive(true);

            StartCoroutine(Respawn(8));
        }
    }

    IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(time);

        displayHUD = true;
        if (stocks > 0)
        {
            GetComponent<Character>().setDead(false);
            GetComponent<CharacterMove>().setDead(false);
            GetComponent<CharacterShield>().setDead(false);
            armorTop.SetActive(false);
            charAnim.SetBool("Dead", false);
            camFPS.GetComponent<Camera>().enabled = true;
            camFPSLeft.GetComponent<Camera>().enabled = true;
            camFPSRight.GetComponent<Camera>().enabled = true;
            camTPS.GetComponent<Camera>().enabled = false;
            transform.position = GameObject.Find("Spawner").transform.position;
            CmdSetlife(1000);
        }
        else
        {
            life = -1;
            GameObject[] playertab = GameObject.FindGameObjectsWithTag("Character");
            for(int i = 0; i< playertab.Length; ++i)
            {
                if (playertab[i] != gameObject && playertab[i].GetComponent<CharacterLife>().life > 0)
                {
                    playertab[i].GetComponent<CharacterLife>().armorTop.SetActive(false);
                    camFPS = playertab[i].GetComponent<CharacterLife>().camParentTransform.FindChild("CamFPS");
                    camFPSLeft = playertab[i].GetComponent<CharacterLife>().camParentTransform.FindChild("CamFPSLeft");
                    camFPSRight = playertab[i].GetComponent<CharacterLife>().camParentTransform.FindChild("CamFPSRight");
                    camFPS.GetComponent<Camera>().enabled = true;
                    camFPSLeft.GetComponent<Camera>().enabled = true;
                    camFPSRight.GetComponent<Camera>().enabled = true;
                    camTPS.GetComponent<Camera>().enabled = false;
                    

                    i = 5;// la sortie de for la plus sale de l'univers!
                }
            }

        }

    }

    [Command]
    void CmdSetlife(int l)
    {
        life = l;
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // move back to zero location
            transform.position = GameObject.Find("Spawner").transform.position;
        }
    }

    public void LoseLife(int damage, NetworkInstanceId id)
    {
        if (!isServer || life <=0)
            return;

        life -= damage;

        if (life <= 0)
        {

            life = 0;

            // called on the server, will be invoked on the clients
            stocks -= 1;
            RpcDie(id);
            GameObject.Find("GameManager").GetComponent<GameManager>().DeadPlayer(id, GetComponent<NetworkIdentity>().netId);
        }
    }

    public void SpawnDeathParticle()
    {
        GameObject particle = (GameObject)Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(particle, 15);
    }

    void UpdateHealthBar()
    {
        if (life != prevLife)
        {
            if (life > 500)
            {
                texHealth.SetPixel(0, 0, Color.Lerp(Color.yellow, Color.green, (life - 500) / 500));
            }
            else
            {
                texHealth.SetPixel(0, 0, Color.Lerp(Color.red, Color.yellow, life / 500));
            }
            texHealth.Apply();
            texHealthStyle.normal.background = texHealth;
            prevLife = life;
        }
    }

    void updateDeathFadeBlack()
    {
        if (life == 0 && fadeBlackAlpha < 1)
        {
            fadeBlackAlpha += Time.deltaTime / 8.0f;
            texFadeBlack.SetPixel(0, 0, new Color(0, 0, 0, fadeBlackAlpha));

            texFadeBlack.Apply();
            texFadeBlackStyle.normal.background = texFadeBlack;
        }
        else if (fadeBlackAlpha > 0 && life !=0)
        {
            if (fadeBlackAlpha < 0)
                fadeBlackAlpha = 0;

            fadeBlackAlpha -= Time.deltaTime/2.0f;

            texFadeBlack.SetPixel(0, 0, new Color(0, 0, 0, fadeBlackAlpha));

            texFadeBlack.Apply();
            texFadeBlackStyle.normal.background = texFadeBlack;
        }
    }

    void OnGUI()
    {
        if (!isLocalPlayer)
            return;

        if (displayHUD)
        {
            GUIUtility.RotateAroundPivot(-7, new Vector2(x, y));
            GUI.Box(new Rect(x, y, size, size), GUIContent.none, texHealthStyle);
            if (stocks >= 3)
                GUI.color = Color.green;
            else
                GUI.color = Color.red;
            GUI.Box(new Rect(Screen.width * 0.29f, Screen.height*0.87f, Screen.width*0.035f, Screen.width * 0.035f), GUIContent.none, texStocksStyle);

            if(stocks >= 2)
                GUI.color = Color.green;
            else
                GUI.color = Color.red;
            GUI.Box(new Rect(Screen.width * 0.29f, Screen.height * 0.935f, Screen.width * 0.035f, Screen.width * 0.035f), GUIContent.none, texStocksStyle);

            GUIUtility.RotateAroundPivot(7, new Vector2(x, y));
            GUI.color = Color.white;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), cockpit);
        }

        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none, texFadeBlackStyle);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
        updateDeathFadeBlack();
    }
}
