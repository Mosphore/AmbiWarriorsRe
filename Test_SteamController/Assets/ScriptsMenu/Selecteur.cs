using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Selecteur : MonoBehaviour
{
    //variables générales
    int activemenu = 0;
    int posbouton = 0;
    public AudioClip move;
    public AudioClip select;
    public AudioClip back;
    public AudioSource audio;
    private GameObject cam;

    //variables ecran titre
    private SpriteRenderer srps;
    private SpriteRenderer srtitle;
    public GameObject title;
    public GameObject pressstart;

    //variables mainmenu
    public Sprite normal1;
    public Sprite highlight1;
    public Sprite normal2;
    public Sprite highlight2;
    public Sprite normal3;
    public Sprite highlight3;
    public GameObject ecran;
    private SpriteRenderer srecran;
    public GameObject bouton1;
    public GameObject bouton2;
    public GameObject bouton3;
    private SpriteRenderer sr1;
    private SpriteRenderer sr2;
    private SpriteRenderer sr3;

    //variables playmenu
    public GameObject plmenu;
    public GameObject host;
    public GameObject join;
    public Sprite hostDefault;
    public Sprite hostHover;
    public Sprite joinDefault;
    public Sprite joinHover;
    public InputField addresseIP;

    //variables partymenu
    public GameObject parmenu;

    //variables custommenu
    public GameObject custmenu;
    public GameObject red;
    public GameObject blue;
    public GameObject green;
    public GameObject pink;
    public GameObject orange;
    public GameObject yellow;
    public GameObject mecha;
    private SpriteRenderer sred;
    private SpriteRenderer sblue;
    private SpriteRenderer sgreen;
    private SpriteRenderer spink;
    private SpriteRenderer sorange;
    private SpriteRenderer syellow;
    public Sprite red1;
    public Sprite red2, red3;
    public Sprite blue1;
    public Sprite blue2, blue3;
    public Sprite green1;
    public Sprite green2, green3;
    public Sprite pink1;
    public Sprite pink2, pink3;
    public Sprite orange1;
    public Sprite orange2, orange3;
    public Sprite yellow1;
    public Sprite yellow2, yellow3;
    public GameObject leftArmMesh,
                      rightArmMesh,
                      topMesh,
                      bottomMesh,
                      bodyMesh;
    public Material[] top, arm, bottom, body;
    private SetCharacterColor.ROBOT_COLOR selectedColor = SetCharacterColor.ROBOT_COLOR.ORANGE;

    //variables creditsmenu
    public GameObject credmenu;

    // Use this for initialization
    void Start()
    {
        sr1 = bouton1.GetComponent<SpriteRenderer>();
        sr2 = bouton2.GetComponent<SpriteRenderer>();
        sr3 = bouton3.GetComponent<SpriteRenderer>();
        srecran = ecran.GetComponent<SpriteRenderer>();
        srtitle = title.GetComponent<SpriteRenderer>();
        srps = pressstart.GetComponent<SpriteRenderer>();
        sred = red.GetComponent<SpriteRenderer>();
        sblue = blue.GetComponent<SpriteRenderer>();
        sgreen = green.GetComponent<SpriteRenderer>();
        spink = pink.GetComponent<SpriteRenderer>();
        sorange = orange.GetComponent<SpriteRenderer>();
        syellow = yellow.GetComponent<SpriteRenderer>();
        //panpar1 = GameObject.Find ("Party1");
        //panpar2 = GameObject.Find ("Party2");
        //panpar3 = GameObject.Find ("Party3");
        //confirm = GameObject.Find ("confirm");
    }
    // Update is called once per frame
    void Update()
    {

        //press start
        if (Input.GetKeyUp(KeyCode.Return) && activemenu == 0)
        {
            audio.PlayOneShot(select, 1.0F);
            srtitle.enabled = false;
            srps.enabled = false;
            mainmenu();
        }
        //retour au menu

        if (activemenu > 1)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                audio.PlayOneShot(back, 1.0F);
                mainmenu();
            }
        }
        //deplacement curseur mainmenu
        if (activemenu == 1)
        {
            //aller vers le bas
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                posbouton = posbouton + 1;
                audio.PlayOneShot(move, 0.9F);
                over();
                highlight();
            }

            //aller vers le haut
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                posbouton = posbouton - 1;
                audio.PlayOneShot(move, 0.9F);
                over();
                highlight();
            }
            //changement de menu depuis mainmenu
            if (Input.GetKeyUp(KeyCode.Space) && posbouton == 0)
            {
                audio.PlayOneShot(select, 1.0F);
                playmenu();
            }
            if (Input.GetKeyUp(KeyCode.Space) && posbouton == 1)
            {
                audio.PlayOneShot(select, 1.0F);
                custommenu();
            }
            if (Input.GetKeyUp(KeyCode.Space) && posbouton == 2)
            {
                audio.PlayOneShot(select, 1.0F);
                creditsmenu();
            }
        }

        //deplacement curseur menu play
        if (activemenu == 2)
        {
            if (posbouton < 2)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    posbouton = posbouton + 1;
                    audio.PlayOneShot(move, 0.9F);
                    highlight();
                }
            }
            if (posbouton > 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    posbouton = posbouton - 1;
                    audio.PlayOneShot(move, 0.9F);
                    highlight();
                }
            }
        }
        // selection de party depuis menu play
        if (activemenu == 2)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (posbouton == 0)
                    GameObject.Find("NetworkManager").GetComponent<NetworkManager_Custom>().StartupHost();
                if(posbouton == 2)
                    GameObject.Find("NetworkManager").GetComponent<NetworkManager_Custom>().JoinGame();
            }
            if (Input.GetKeyUp(KeyCode.Return))
            {
                posbouton = 2;
                highlight();
            }
        }

        //deplacement curseur menu custom
        if (activemenu == 3)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (posbouton == 0)
                {
                    selectedColor = SetCharacterColor.ROBOT_COLOR.RED;
                    GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.RED);
                }
                if (posbouton == 1)
                {
                    selectedColor = SetCharacterColor.ROBOT_COLOR.BLUE;
                    GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.BLUE);
                }
                if (posbouton == 2)
                {
                    selectedColor = SetCharacterColor.ROBOT_COLOR.GREEN;
                    GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.GREEN);
                }
                if (posbouton == 3)
                {
                    selectedColor = SetCharacterColor.ROBOT_COLOR.PINK;
                    GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.PINK);
                }
                if (posbouton == 4)
                {
                    selectedColor = SetCharacterColor.ROBOT_COLOR.ORANGE;
                    GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.ORANGE);
                }
                if (posbouton == 5)
                {
                    selectedColor = SetCharacterColor.ROBOT_COLOR.YELLOW;
                    GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.YELLOW);
                }
                highlight();
            }
            if (posbouton % 2 == 0)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    posbouton = posbouton + 1;
                    over();
                    highlight();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {

                    posbouton = posbouton + 2;
                    over();
                    highlight();
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    posbouton = posbouton - 2;
                    over();
                    highlight();
                }
            }
            if (posbouton % 2 == 1)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {

                    posbouton = posbouton - 1;
                    over();
                    highlight();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    posbouton = posbouton + 2;
                    over();
                    highlight();
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    posbouton = posbouton - 2;
                    over();
                    highlight();
                }
            }
        }
    }




    //FONCTIONS Appelées

    //pour faire le tour du menu
    void over()
    {
        if (activemenu == 1)
        {
            if (posbouton > 2)
            {
                posbouton = 0;
            }
            if (posbouton < 0)
            {
                posbouton = 2;
            }
        }
        if (activemenu == 3)
        {
            if (posbouton > 4 && posbouton % 2 == 0)
            {
                posbouton = 0;
            }
            if (posbouton < 0 && posbouton % 2 == 0)
            {
                posbouton = 4;
            }
            if (posbouton > 5 && posbouton % 2 == 1)
            {
                posbouton = 1;
            }
            if (posbouton < 0 && posbouton % 2 == -1)
            {
                posbouton = 5;
            }
        }
    }

    //pour changer le sprite des boutons quand le sélecteur est dessus
    void highlight()
    {
        if (activemenu == 1)
        {
            if (posbouton == 0)
            {
                sr1.sprite = highlight1;
                if (sr2.sprite != normal2)
                {
                    sr2.sprite = normal2;
                }
                if (sr3.sprite != normal3)
                {
                    sr3.sprite = normal3;
                }
            }
            if (posbouton == 1)
            {
                sr2.sprite = highlight2;
                if (sr1.sprite != normal1)
                {
                    sr1.sprite = normal1;
                }
                if (sr3.sprite != normal3)
                {
                    sr3.sprite = normal3;
                }
            }
            if (posbouton == 2)
            {
                sr3.sprite = highlight3;
                if (sr1.sprite != normal1)
                {
                    sr1.sprite = normal1;
                }
                if (sr2.sprite != normal2)
                {
                    sr2.sprite = normal2;
                }
            }

        }
        if (activemenu == 2)
        {

            if (posbouton == 0)
            {
                addresseIP.DeactivateInputField();
                host.GetComponent<SpriteRenderer>().sprite = hostHover;
                join.GetComponent<SpriteRenderer>().sprite = joinDefault;
            }
            if (posbouton == 1)
            {
                addresseIP.ActivateInputField();
                join.GetComponent<SpriteRenderer>().sprite = joinDefault;
                host.GetComponent<SpriteRenderer>().sprite = hostDefault;
            }
            if (posbouton == 2)
            {
                addresseIP.DeactivateInputField();
                join.GetComponent<SpriteRenderer>().sprite = joinHover;
                host.GetComponent<SpriteRenderer>().sprite = hostDefault;


            }

        }
        if (activemenu == 3)
        {
            if (posbouton == 0)
            {
                sred.sprite = red2;
                sblue.sprite = blue1;
                sgreen.sprite = green1;
                spink.sprite = pink1;
                sorange.sprite = orange1;
                syellow.sprite = yellow1;

                leftArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.RED];
                rightArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.RED];
                topMesh.GetComponent<Renderer>().material = top[(int)SetCharacterColor.ROBOT_COLOR.RED];
                bottomMesh.GetComponent<Renderer>().material = bottom[(int)SetCharacterColor.ROBOT_COLOR.RED];
                bodyMesh.GetComponent<Renderer>().material = body[(int)SetCharacterColor.ROBOT_COLOR.RED];
                
            }
            if (posbouton == 1)
            {
                sred.sprite = red1;
                sblue.sprite = blue2;
                sgreen.sprite = green1;
                spink.sprite = pink1;
                sorange.sprite = orange1;
                syellow.sprite = yellow1;

                leftArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.BLUE];
                rightArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.BLUE];
                topMesh.GetComponent<Renderer>().material = top[(int)SetCharacterColor.ROBOT_COLOR.BLUE];
                bottomMesh.GetComponent<Renderer>().material = bottom[(int)SetCharacterColor.ROBOT_COLOR.BLUE];
                bodyMesh.GetComponent<Renderer>().material = body[(int)SetCharacterColor.ROBOT_COLOR.BLUE];
                //GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.BLUE);

            }
            if (posbouton == 2)
            {
                sred.sprite = red1;
                sblue.sprite = blue1;
                sgreen.sprite = green2;
                spink.sprite = pink1;
                sorange.sprite = orange1;
                syellow.sprite = yellow1;

                leftArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.GREEN];
                rightArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.GREEN];
                topMesh.GetComponent<Renderer>().material = top[(int)SetCharacterColor.ROBOT_COLOR.GREEN];
                bottomMesh.GetComponent<Renderer>().material = bottom[(int)SetCharacterColor.ROBOT_COLOR.GREEN];
                bodyMesh.GetComponent<Renderer>().material = body[(int)SetCharacterColor.ROBOT_COLOR.GREEN];
                //GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.GREEN);
            }
            if (posbouton == 3)
            {
                sred.sprite = red1;
                sblue.sprite = blue1;
                sgreen.sprite = green1;
                spink.sprite = pink2;
                sorange.sprite = orange1;
                syellow.sprite = yellow1;

                leftArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.PINK];
                rightArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.PINK];
                topMesh.GetComponent<Renderer>().material = top[(int)SetCharacterColor.ROBOT_COLOR.PINK];
                bottomMesh.GetComponent<Renderer>().material = bottom[(int)SetCharacterColor.ROBOT_COLOR.PINK];
                bodyMesh.GetComponent<Renderer>().material = body[(int)SetCharacterColor.ROBOT_COLOR.PINK];

                //GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.PINK);
            }
            if (posbouton == 4)
            {
                sred.sprite = red1;
                sblue.sprite = blue1;
                sgreen.sprite = green1;
                spink.sprite = pink1;
                sorange.sprite = orange2;
                syellow.sprite = yellow1;

                leftArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.ORANGE];
                rightArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.ORANGE];
                topMesh.GetComponent<Renderer>().material = top[(int)SetCharacterColor.ROBOT_COLOR.ORANGE];
                bottomMesh.GetComponent<Renderer>().material = bottom[(int)SetCharacterColor.ROBOT_COLOR.ORANGE];
                bodyMesh.GetComponent<Renderer>().material = body[(int)SetCharacterColor.ROBOT_COLOR.ORANGE];
                //GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.ORANGE);
            }
            if (posbouton == 5)
            {
                sred.sprite = red1;
                sblue.sprite = blue1;
                sgreen.sprite = green1;
                spink.sprite = pink1;
                sorange.sprite = orange1;
                syellow.sprite = yellow2;

                leftArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.YELLOW];
                rightArmMesh.GetComponent<Renderer>().material = arm[(int)SetCharacterColor.ROBOT_COLOR.YELLOW];
                topMesh.GetComponent<Renderer>().material = top[(int)SetCharacterColor.ROBOT_COLOR.YELLOW];
                bottomMesh.GetComponent<Renderer>().material = bottom[(int)SetCharacterColor.ROBOT_COLOR.YELLOW];
                bodyMesh.GetComponent<Renderer>().material = body[(int)SetCharacterColor.ROBOT_COLOR.YELLOW];
                //GameObject.Find("ColorSaver").GetComponent<ColorSave>().SetColor(SetCharacterColor.ROBOT_COLOR.YELLOW);
            }
            
            if(selectedColor == SetCharacterColor.ROBOT_COLOR.RED)
            {
                sred.sprite = red3;
            }
            if (selectedColor == SetCharacterColor.ROBOT_COLOR.BLUE)
            {
                sblue.sprite = blue3;
            }
            if (selectedColor == SetCharacterColor.ROBOT_COLOR.YELLOW)
            {
                syellow.sprite = yellow3;
            }
            if (selectedColor == SetCharacterColor.ROBOT_COLOR.GREEN)
            {
                sgreen.sprite = green3;
            }
            if (selectedColor == SetCharacterColor.ROBOT_COLOR.PINK)
            {
                spink.sprite = pink3;
            }
            if (selectedColor == SetCharacterColor.ROBOT_COLOR.ORANGE)
            {
                sorange.sprite = orange3;
            }
        }
    }

    //pour aller vers le menu play
    void playmenu()
    {
        plmenu.SetActive(true);
        activemenu = 2;
        posbouton = 0;
        sr1.enabled = false;
        sr2.enabled = false;
        sr3.enabled = false;
        highlight();
    }
    //pour aller vers le menu custom
    void custommenu()
    {
        activemenu = 3;
        posbouton = 0;
        sr1.enabled = false;
        sr2.enabled = false;
        sr3.enabled = false;
        custmenu.SetActive(true);
        highlight();
    }
    //pour aller vers les crédits
    void creditsmenu()
    {
        credmenu.SetActive(true);
        activemenu = 4;
        posbouton = 0;
        sr1.enabled = false;
        sr2.enabled = false;
        sr3.enabled = false;
    }
    //pour retourner au main menu
    void mainmenu()
    {
        credmenu.SetActive(false);
        plmenu.SetActive(false);
        custmenu.SetActive(false);
        activemenu = 1;
        posbouton = 0;
        srecran.enabled = true;
        sr1.enabled = true;
        sr2.enabled = true;
        sr3.enabled = true;
        highlight();
    }



}