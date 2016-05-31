using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Rewired;

//[RequireComponent(typeof(CharacterController))]
public class Character : NetworkBehaviour
{
    enum SHOOT_TYPE
    {
        LEFT_MISSILE = 0,
        LEFT_GATLING,
        RIGHT_MISSILE,
        RIGHT_GATLING
    };

    //son
    public AudioClip[] shootSoundsArray;

    public int playerId = 0; // The Rewired player id of this character
    private Player player; // The Rewired Player

    Animator charAnim;
    //NetworkAnimator netCharAnim;
    float leftBazookaAnimTimer, rightBazookaAnimTimer;

    //prefabs
    public GameObject missile;

    public GameObject GatlingParticleEffect;
    public GameObject ImpactParticle;
    public GameObject MissileShootParticle;

    //sprites HUD
    public Texture2D crosshairLeft, crosshairRight;
    public Texture2D cockpit;

    //Camera
    public Transform camParentTransform;
    Transform camMinimap, camTPS, camFPS, camFPSLeft, camFPSRight, // chaque camera du personnage (1ere pers 3eme pers et la minimap
               camPlayer, camLeftAim, camRightAim;// ces 3 dernieres cam servent de couche d'abstraction pour switcher les cameras
    public GameObject armorTop; // on desactive l'affichage de l'armure pour le joueur, pasque sinon elle est devant sa camera, c'est pour placer la cam plus ou on veut

    //Viseurs

    Vector2 leftCrosshairMove, rightCrosshairMove;
    Vector2 leftInput, rightInput, prevLeftInput, prevRightInput;

    //Bras
    [SyncVar]
    Vector3 leftArmLookAt;
    [SyncVar]
    Vector3 rightArmLookAt;

    public Transform LeftArmTransform, RightArmTransform;
    Vector3 aimPosRight;
    Vector3 aimPosLeft;

    //Missile et Tirs
    public Transform shootSpawnLeft, shootSpawnRight;
    float leftShootTimer = 0,
          rightShootTimer = 0,
          leftMissileTimer = 0,
          rightMissileTimer = 0;

    //variables GD 
    public Vector2 TweakLeftCrosshairInitPos, TweakRightCrosshairInitPos;
    public float TweakMissileSpeed = 50.0f;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
        aimPosRight = aimPosLeft = Vector3.zero;
    }

    // Use this for initialization
    void Start()
    {
        
        charAnim = GetComponent<Animator>();
        //netCharAnim = GetComponent<NetworkAnimator>();
        //active la bonne camera pour le perso sur le serveur
        if (isLocalPlayer)
        {
                
            TweakRightCrosshairInitPos += new Vector2(Screen.width / 2, Screen.height / 2);

            TweakLeftCrosshairInitPos += new Vector2(Screen.width / 2, Screen.height / 2);

            Cursor.visible = false;

            //leftCam = transform.FindChild("Camera_Left");
            //rightCam = transform.FindChild("Camera_Right");
            camFPS = camParentTransform.FindChild("CamFPS");
            camFPSLeft = camParentTransform.FindChild("CamFPSLeft");
            camFPSRight = camParentTransform.FindChild("CamFPSRight");

            camTPS = transform.FindChild("CamTPS");
            camMinimap = transform.FindChild("CamMinimap");
            camPlayer = camFPS;
            camLeftAim = camFPSLeft;
            camRightAim = camFPSRight;

            camPlayer.GetComponent<Camera>().enabled = true;
            camMinimap.GetComponent<Camera>().enabled = true;
            camLeftAim.GetComponent<Camera>().enabled = true;
            camRightAim.GetComponent<Camera>().enabled = true;

            armorTop.SetActive(false);

            GetComponentInChildren<AudioListener>(true).enabled = true;
        }
    }

    void OnGUI()
    {
        if (!isLocalPlayer)
            return;

        //dessin des cibles (crosshair)
        GUI.DrawTexture(new Rect(TweakLeftCrosshairInitPos.x + leftCrosshairMove.x - crosshairLeft.width / 6,
                                 TweakLeftCrosshairInitPos.y + leftCrosshairMove.y - crosshairLeft.height / 6,
                                 crosshairLeft.width / 3, crosshairLeft.height / 3),
                                 crosshairLeft);

        GUI.DrawTexture(new Rect(TweakRightCrosshairInitPos.x + rightCrosshairMove.x - crosshairRight.width / 6,
                                 TweakRightCrosshairInitPos.y + rightCrosshairMove.y - crosshairRight.height / 6,
                                 crosshairRight.width / 3, crosshairRight.height / 3),
                                 crosshairRight);

        //if (showcockpit)
        //    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), cockpit);
    }

    [Command]
    void CmdCheat()
    {
        GetComponent<CharacterLife>().LoseLife(200);
    }

    void PlayerControls()
    {

        Vector2 moveLeft, moveRight;
        moveLeft = moveRight = Vector2.zero;

        /*********/
        //Mouvement viseur
        /*********/
        leftInput.x = Input.GetAxis("Horizontal");

        leftInput.y = Input.GetAxis("Vertical");

        rightInput.x = Input.GetAxis("Horizontal2");

        rightInput.y = Input.GetAxis("Vertical2");

        if (leftInput.x != 0 && prevLeftInput.x != 0)
            moveLeft.x = leftInput.x - prevLeftInput.x;

        if (leftInput.y != 0 && prevLeftInput.y != 0)
            moveLeft.y = leftInput.y - prevLeftInput.y;

        prevLeftInput.x = leftInput.x;
        prevLeftInput.y = leftInput.y;

        if (rightInput.x != 0 && prevRightInput.x != 0)
            moveRight.x = rightInput.x - prevRightInput.x;

        if (rightInput.y != 0 && prevRightInput.y != 0)
            moveRight.y = rightInput.y - prevRightInput.y;

        prevRightInput.x = rightInput.x;
        prevRightInput.y = rightInput.y;


        leftCrosshairMove.x += moveLeft.x * 300;
        if (TweakLeftCrosshairInitPos.x + leftCrosshairMove.x > Screen.width - Screen.width / 3 || TweakLeftCrosshairInitPos.x + leftCrosshairMove.x < 0)
            leftCrosshairMove.x -= moveLeft.x * 300;

        leftCrosshairMove.y += moveLeft.y * 300;

        rightCrosshairMove.x += moveRight.x * 300;
        if (TweakRightCrosshairInitPos.x + rightCrosshairMove.x < Screen.width / 3 || TweakRightCrosshairInitPos.x + rightCrosshairMove.x > Screen.width)
            rightCrosshairMove.x -= moveRight.x * 300;

        rightCrosshairMove.y += moveRight.y * 300;

        /*********/
        //tirs
        /*********/
        leftShootTimer += Time.deltaTime;
        rightShootTimer += Time.deltaTime;
        leftMissileTimer += Time.deltaTime;
        rightMissileTimer += Time.deltaTime;

        if (Input.GetAxisRaw("RightMissile") > 0 && rightMissileTimer >= 1.0f)
        {
            shootSpawnRight.GetComponent<AudioSource>().Play();
            CmdPlayBazookaSound(true);
            //charAnim.SetTrigger("BazookaRight");
            rightMissileTimer = 0;
            CalcShoot(SHOOT_TYPE.RIGHT_MISSILE);
        }

        if (Input.GetAxisRaw("LeftMissile") > 0 && leftMissileTimer >= 1.0f)
        {
            shootSpawnLeft.GetComponent<AudioSource>().Play();
            CmdPlayBazookaSound(false);
            //charAnim.SetTrigger("BazookaLeft");
            leftMissileTimer = 0;
            CalcShoot(SHOOT_TYPE.LEFT_MISSILE);
        }

        if (player.GetAxis("LeftGatling") != 0 && leftShootTimer >= 0.085f)
        {
            shootSpawnLeft.GetComponent<AudioSource>().PlayOneShot(shootSoundsArray[Random.Range(0, shootSoundsArray.Length)]);  // on joue le son
            CmdPlayGatlingSound(false);

            leftShootTimer = 0;
            CalcShoot(SHOOT_TYPE.LEFT_GATLING);
        }


        if (player.GetAxis("RightGatling") != 0 && rightShootTimer >= 0.085f)
        {
            shootSpawnLeft.GetComponent<AudioSource>().PlayOneShot(shootSoundsArray[Random.Range(0, shootSoundsArray.Length)]); // on joue le son
            CmdPlayGatlingSound(true);


            rightShootTimer = 0;
            CalcShoot(SHOOT_TYPE.RIGHT_GATLING);
        }

        if (player.GetAxis("RightGatling") != 0)
            charAnim.SetBool("GatlingRight", true);
        else
            charAnim.SetBool("GatlingRight", false);

        if (player.GetAxis("LeftGatling") != 0)
            charAnim.SetBool("GatlingLeft", true);
        else
            charAnim.SetBool("GatlingLeft", false);

    }

    //on calcule la position et l'orientation du tir selon le bras qui a tiré et le type de tir avant d'envoyer le tout au serveur avec la [Command]
    void CalcShoot(SHOOT_TYPE st)
    {
        GetComponent<CharacterMove>().SlowMovement();
        if (st == SHOOT_TYPE.RIGHT_MISSILE)
        {

            charAnim.SetBool("BazookaRight", true);
            rightBazookaAnimTimer = 0.1f;
            //   leftCrosshairInitPos.x + leftCrosshairMove.x
            Vector3 p = camRightAim.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(TweakRightCrosshairInitPos.x + rightCrosshairMove.x,
                                                                                       TweakRightCrosshairInitPos.y - rightCrosshairMove.y, 100));

            RaycastHit hit;
            if (Physics.Raycast(camPlayer.position, p - camPlayer.position, out hit))
            {
                if (hit.collider != null)
                {
                    p = hit.point;
                }
            }

            Vector3 shootDir = p - shootSpawnRight.position;
            shootDir.Normalize();
            CmdShootMissile(shootSpawnRight.position, shootDir);
        }
        else if (st == SHOOT_TYPE.RIGHT_GATLING)
        {
            Vector3 p = camRightAim.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(TweakRightCrosshairInitPos.x + rightCrosshairMove.x,
                                                                           TweakRightCrosshairInitPos.y - rightCrosshairMove.y, 100));
            RaycastHit hit;
            if (Physics.Raycast(camPlayer.position, p - camPlayer.position, out hit))
            {
                if (hit.collider != null)
                {
                    p = hit.point;
                }
            }
            Vector3 shootDir = p - shootSpawnRight.position;
            shootDir.Normalize();
            CmdShootGatling(shootSpawnRight.position, shootDir);
        }
        if (st == SHOOT_TYPE.LEFT_MISSILE)
        {

            charAnim.SetBool("BazookaLeft", true);
            leftBazookaAnimTimer = 0.1f;
            Vector3 p = camLeftAim.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(TweakLeftCrosshairInitPos.x + leftCrosshairMove.x,
                                                                                       TweakLeftCrosshairInitPos.y - leftCrosshairMove.y, 100));
            RaycastHit hit;
            if (Physics.Raycast(camPlayer.position, p - camPlayer.position, out hit))
            {
                if (hit.collider != null)
                {
                    p = hit.point;
                }
            }
            Vector3 shootDir = p - shootSpawnLeft.position;
            shootDir.Normalize();
            CmdShootMissile(shootSpawnLeft.position, shootDir);
        }
        else if (st == SHOOT_TYPE.LEFT_GATLING)
        {
            Vector3 p = camLeftAim.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(TweakLeftCrosshairInitPos.x + leftCrosshairMove.x,
                                                                                       TweakLeftCrosshairInitPos.y - leftCrosshairMove.y, 100));

            RaycastHit hit;
            if (Physics.Raycast(camPlayer.position, p - camPlayer.position, out hit))
            {
                if (hit.collider != null)
                {
                    p = hit.point;
                }
            }
            Vector3 shootDir = p - shootSpawnLeft.position;
            shootDir.Normalize();
            CmdShootGatling(shootSpawnLeft.position, shootDir);
        }

    }

    [Command]
    void CmdShootMissile(Vector3 position, Vector3 direction)
    {
        //tir d'objet physique (missile )
        GameObject MissileClone = (GameObject)Instantiate(missile, position, Quaternion.identity);
        MissileClone.transform.up = direction;
        MissileClone.GetComponent<Rigidbody>().velocity = direction * TweakMissileSpeed;
        MissileClone.GetComponent<MissileScript>().SetOwner(gameObject);
        NetworkServer.Spawn(MissileClone);
        Destroy(MissileClone, 10.0f);

        GameObject MissileShootParticleClone = (GameObject)Instantiate(MissileShootParticle, position, Quaternion.identity);
        MissileShootParticleClone.transform.LookAt(position + 2 * direction);
        NetworkServer.Spawn(MissileShootParticleClone);
        Destroy(MissileShootParticleClone, 1.0f);
    }

    [Command]
    void CmdShootGatling(Vector3 position, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit))
        {
            if (hit.collider != null)
            {
                GameObject ImpactClone = (GameObject)Instantiate(ImpactParticle, hit.point, Quaternion.LookRotation(hit.normal));
                NetworkServer.Spawn(ImpactClone);
                Destroy(ImpactClone, 0.2f);

                GameObject GatlingParticleClone = (GameObject)Instantiate(GatlingParticleEffect, position, Quaternion.identity);
                GatlingParticleClone.transform.LookAt(hit.point);
                NetworkServer.Spawn(GatlingParticleClone);
                Destroy(GatlingParticleClone, 0.2f);

                if (hit.transform.tag == "Character")
                {
                    hit.transform.GetComponent<CharacterLife>().LoseLife(10);
                }

                //if (hit.transform.tag == "DestructBat")
                //{
                //    hit.transform.GetComponent<DestructObject>().TakeDamage(5);
                //}
            }
        }
        else
        {
            GameObject GatlingParticleClone = (GameObject)Instantiate(GatlingParticleEffect, position, Quaternion.identity);
            GatlingParticleClone.transform.LookAt(position + direction * 100);
            NetworkServer.Spawn(GatlingParticleClone);
            Destroy(GatlingParticleClone, 0.2f);
        }
    }

    [Command]
    //false pour bras gauche true pour bras droit
    void CmdPlayBazookaSound(bool side)
    {
        RpcPlayBazookaSound(side);
    }

    [ClientRpc]
    void RpcPlayBazookaSound(bool side)
    {
        if (!isLocalPlayer)
        {
            if (side)
                shootSpawnRight.GetComponent<AudioSource>().Play(); // on joue le son
            else
                shootSpawnLeft.GetComponent<AudioSource>().Play(); // on joue le son
        }
    }
    [Command]
    //false pour bras gauche true pour bras droit
    void CmdPlayGatlingSound(bool side)
    {
        RpcPlayGatlingSound(side);
    }

    [ClientRpc]
    void RpcPlayGatlingSound(bool side)
    {
        if (!isLocalPlayer)
        {
            if (side)
                shootSpawnRight.GetComponent<AudioSource>().PlayOneShot(shootSoundsArray[Random.Range(0, shootSoundsArray.Length)]); // on joue le son
            else
                shootSpawnLeft.GetComponent<AudioSource>().PlayOneShot(shootSoundsArray[Random.Range(0, shootSoundsArray.Length)]); // on joue le son
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(aimPosRight, 1);
        Gizmos.DrawLine(RightArmTransform.position, aimPosRight);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(aimPosLeft, 1);
        Gizmos.DrawLine(LeftArmTransform.position, aimPosLeft);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (leftBazookaAnimTimer > 0)
        {
            leftBazookaAnimTimer -= Time.deltaTime;
        }
        else
        {
            charAnim.SetBool("BazookaLeft", false);
        }

        if (rightBazookaAnimTimer > 0)
        {
            rightBazookaAnimTimer -= Time.deltaTime;
        }
        else
        {
            charAnim.SetBool("BazookaRight", false);
        }

        /**********/
        //le cheat c'est par ici
        /**********/
        if (Input.GetKey(KeyCode.K))
        {
            CmdCheat();
        }

        /**********/
        //ici c'est pour switch entre cam 1ere et 3eme personne
        /**********/
        if (Input.GetKey(KeyCode.O))
        {
            camFPS.GetComponent<Camera>().enabled = false;
            camTPS.GetComponent<Camera>().enabled = true;
            camPlayer = camTPS;
        }
        if (Input.GetKey(KeyCode.P))
        {
            camFPS.GetComponent<Camera>().enabled = true;
            camTPS.GetComponent<Camera>().enabled = false;
            camPlayer = camFPS;
        }

        PlayerControls();

        //assignement de la bonne camera au viseur
        if (TweakLeftCrosshairInitPos.x + leftCrosshairMove.x < (Screen.width / 3))
        {
            camLeftAim = camFPSLeft;
        }
        else
        {
            camLeftAim = camFPS;
        }

        if (TweakRightCrosshairInitPos.x + rightCrosshairMove.x > Screen.width - (Screen.width / 3))
        {
            camRightAim = camFPSRight;
        }
        else
        {
            camRightAim = camFPS;
        }

        // on recuperere le point de visée de chaque bras pour les tourner vers ce point dans late update
        //(si on met ce bout de code dans late update y'a un nullreferenceexception qui pop u_u ) 
        aimPosLeft = camLeftAim.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(TweakLeftCrosshairInitPos.x + leftCrosshairMove.x,
                                                                                      TweakLeftCrosshairInitPos.y - leftCrosshairMove.y, 100));

        aimPosRight = camRightAim.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(TweakRightCrosshairInitPos.x + rightCrosshairMove.x,
                                                                                       TweakRightCrosshairInitPos.y - rightCrosshairMove.y, 100));

    }

    void FixedUpdate()
    {
        TransmitRotations();


    }

    //synchronise la rotation des bras du robot pour les autres clients
    void RotateArmOther()
    {
        if (!isLocalPlayer)
        {
            LeftArmTransform.forward = leftArmLookAt - LeftArmTransform.position;
            RightArmTransform.forward = rightArmLookAt - RightArmTransform.position;
        }
    }

    [Command]
    void CmdProvideRotationsToServer(Vector3 leftlookAt, Vector3 rightLookAt)
    {
        leftArmLookAt = leftlookAt;
        rightArmLookAt = rightLookAt;
    }

    [Client]
    void TransmitRotations()
    {
        if (isLocalPlayer)
        {
            CmdProvideRotationsToServer(aimPosLeft, aimPosRight);
        }
    }

    // pour bouger les bras du robot apres l'animation
    void LateUpdate()
    {

        //Quaternion lookRotation = Quaternion.LookRotation(aimPosLeft - LeftArmTransform.position);
        //LeftArmTransform.rotation = lookRotation;

        LeftArmTransform.forward = aimPosLeft - LeftArmTransform.position;
        RightArmTransform.forward = aimPosRight - RightArmTransform.position;
        //LeftArmTransform.forward =  -(LeftArmTransfor.position - aimPosLeft);
        //LeftArmTransform.forward = Vector3.Cross(LeftArmTransform.right, LeftArmTransform.forward);

        RotateArmOther();
    }
}
