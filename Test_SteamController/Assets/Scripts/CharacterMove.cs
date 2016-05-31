using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Rewired;

public class CharacterMove : NetworkBehaviour
{

    //son
    AudioSource dashSound;
    AudioSource stepSound;
    public AudioClip[] stepSounds;

    //deplacement du personnage
    float shootMoveTimer = 0;
    float speedMultiplier = 1;
    float dashTimer = 0;

    bool dashingLeft = false,
         dashingRight = false;

    public int playerId = 0; // The Rewired player id of this character
    private Player player; // The Rewired Player
    public float TweakMoveSpeed = 20;
    public float TweakShootSpeedMultiplier = 1;
    public float TweakDashSpeed = 500;
    public float TweakDashDuration = 0.5f;
    public float TweakRotationSpeed = 1;

    Animator charAnim;
    //NetworkAnimator netCharAnim;
    float leftDashAnimTimer, rightDashAnimTimer;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
    }
    // Use this for initialization
    void Start()
    {
        dashSound = transform.FindChild("DashSound").GetComponent<AudioSource>();
        stepSound = transform.FindChild("StepSound").GetComponent<AudioSource>();
        charAnim = GetComponent<Animator>();
        //netCharAnim = GetComponent<NetworkAnimator>();
    }

    public void SlowMovement()
    {
        shootMoveTimer = 0.3f;
    }

    public void StartLeftDashMovement()
    {
            dashingLeft = true;
            dashTimer = TweakDashDuration;
            dashSound.Play();
    }

    public void StartRightDashMovement()
    {
            dashingRight = true;
            dashTimer = TweakDashDuration;
            dashSound.Play();
    }

    public void PlayStepSound()
    {
        stepSound.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (player.GetAxis("MoveHorizontal")>0)
            {
                transform.Rotate(0, TweakRotationSpeed, 0);
            }
            if (player.GetAxis("MoveHorizontal")<0)
            {
                transform.Rotate(0, -TweakRotationSpeed, 0);
            }

            if (player.GetAxis("DashLeft") != 0 && dashTimer <= 0)
            {
                //dashingLeft = true;
                //dashTimer = TweakDashDuration;

            charAnim.SetBool("DashLeft", true);

                leftDashAnimTimer = 0.3f;

            }
            if (player.GetAxis("DashRight") != 0 && dashTimer <= 0)
            {
                //dashingRight = true;
                //dashTimer = TweakDashDuration;

                charAnim.SetBool("DashRight",true);
                rightDashAnimTimer = 0.3f;
            }
            //AnimatorStateInfo currentState = charAnim.GetCurrentAnimatorStateInfo(charAnim.GetLayerIndex("BaseLayer"));


                    /*********/
                    //modifications de vitesse a cause des tirs
                    /*********/
                    if (shootMoveTimer > 0)
            {
                speedMultiplier = TweakShootSpeedMultiplier;
                shootMoveTimer -= Time.deltaTime;
            }
            else
            {
                speedMultiplier = 1;
            }

            /*********/
            //Deplacement perso
            /*********/
            Vector3 moveDirection = Vector3.zero;

            if (dashTimer > 0)
            {
                if (dashingRight)
                {
                    moveDirection = new Vector3(TweakDashSpeed, 0, 0);
                }
                if (dashingLeft)
                {
                    moveDirection = new Vector3(-TweakDashSpeed, 0, 0);
                }
                dashTimer -= Time.deltaTime;

                moveDirection = transform.TransformDirection(moveDirection);
            }
            else
            {
                dashingRight = dashingLeft = false;
                moveDirection = new Vector3(0, 0, -player.GetAxis("MoveVertical"));
                Debug.Log(player.GetAxis("MoveVertical"));
                //if(moveDirection.z > 1)
                //{
                //    moveDirection.z = 1;
                //}
                //if (moveDirection.z < -1)
                //{
                //    moveDirection.z = -1;
                //}
                moveDirection = transform.TransformDirection(moveDirection);

                moveDirection *= TweakMoveSpeed * speedMultiplier * Time.deltaTime;
            }

            if (player.GetAxis("MoveVertical")!= 0)
            {
                charAnim.SetBool("Walk", true);
            }
            else
            {
                charAnim.SetBool("Walk", false);
            }
            //moveDirection.y -= 1000;
            //transform.position += (moveDirection * Time.deltaTime);

            GetComponent<Rigidbody>().AddForce(moveDirection.x, -50, moveDirection.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (leftDashAnimTimer > 0)
        {
            leftDashAnimTimer -= Time.deltaTime;
        }
        else
        {
            charAnim.SetBool("DashLeft", false);
        }

        if (rightDashAnimTimer > 0)
        {
            rightDashAnimTimer -= Time.deltaTime;
        }
        else
        {
            charAnim.SetBool("DashRight", false);
        }

    }
}
