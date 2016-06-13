//using UnityEngine;
//using System.Collections;

//public class Selecteur : MonoBehaviour {
//	//variables générales
//	int activemenu = 0;
//	int posbouton = 0;
//	public AudioClip move;
//	public AudioClip select;
//	public AudioClip back;
//	public AudioSource audio;
//	private GameObject cam;

//	//variables ecran titre
//	private SpriteRenderer srps;
//	private SpriteRenderer srtitle;
//	public GameObject title;
//	public GameObject pressstart;

//	//variables mainmenu
//	public Sprite normal1;
//	public Sprite highlight1;	
//	public Sprite normal2;
//	public Sprite highlight2;	
//	public Sprite normal3;
//	public Sprite highlight3;
//	public GameObject ecran;
//	private SpriteRenderer srecran;
//	public GameObject bouton1;
//	public GameObject bouton2;
//	public GameObject bouton3;
//	private SpriteRenderer sr1;
//	private SpriteRenderer sr2;
//	private SpriteRenderer sr3;

//	//variables playmenu
//	private bool gameselected = false;
//	public GameObject plmenu;
//	public GameObject panpar1;
//	public GameObject panpar2;
//	public GameObject panpar3;
//	public GameObject confirm;
//	public GameObject hosting;
//	public Sprite partydispo;
//	public Sprite partyhighlight;

//	//variables partymenu
//	public GameObject parmenu;

//	//variables custommenu
//	public GameObject custmenu;
//	public GameObject red;
//	public GameObject blue;
//	public GameObject green;
//	public GameObject pink;
//	public GameObject orange;
//	public GameObject yellow;
//	public GameObject mecha;
//	private SpriteRenderer sred;
//	private SpriteRenderer sblue;
//	private SpriteRenderer sgreen;
//	private SpriteRenderer spink;
//	private SpriteRenderer sorange;
//	private SpriteRenderer syellow;
//	public Sprite red1;
//	public Sprite red2;
//	public Sprite blue1;
//	public Sprite blue2;
//	public Sprite green1;
//	public Sprite green2;
//	public Sprite pink1;
//	public Sprite pink2;
//	public Sprite orange1;
//	public Sprite orange2;
//	public Sprite yellow1;
//	public Sprite yellow2;
//	public Mesh mred;
//	public Mesh mblue;
//	public Mesh mgreen;
//	public Mesh mpink;
//	public Mesh morange;
//	public Mesh myellow;
//	private MeshRenderer mechmesh;

//	//variables creditsmenu
//	public GameObject credmenu;

//	// Use this for initialization
//	void Start () {
//		sr1 = bouton1.GetComponent<SpriteRenderer> ();
//		sr2 = bouton2.GetComponent<SpriteRenderer> ();
//		sr3 = bouton3.GetComponent<SpriteRenderer> ();
//		srecran = ecran.GetComponent<SpriteRenderer> ();
//		srtitle = title.GetComponent<SpriteRenderer> ();
//		srps = pressstart.GetComponent<SpriteRenderer> ();
//		sred = red.GetComponent<SpriteRenderer> ();
//		sblue = blue.GetComponent<SpriteRenderer> ();
//		sgreen = green.GetComponent<SpriteRenderer> ();
//		spink = pink.GetComponent<SpriteRenderer> ();
//		sorange = orange.GetComponent<SpriteRenderer> ();
//		syellow = yellow.GetComponent<SpriteRenderer> ();
//		mechmesh = mecha.GetComponent<MeshRenderer> ();
//		//panpar1 = GameObject.Find ("Party1");
//		//panpar2 = GameObject.Find ("Party2");
//		//panpar3 = GameObject.Find ("Party3");
//		//confirm = GameObject.Find ("confirm");
//	}
//	// Update is called once per frame
//	void Update () {

//		//press start
//		if (Input.GetKeyDown("space") && activemenu == 0){
//			audio.PlayOneShot(select, 1.0F);
//			srtitle.enabled = false;
//			srps.enabled = false;
//			mainmenu ();
//		}
//		//retour au menu

//		if (activemenu > 1 && gameselected == false) {
//			if (Input.GetKeyDown ("g")) {
//				audio.PlayOneShot (back, 1.0F);
//				mainmenu ();
//			}
//		}
//		//deplacement curseur mainmenu
//		if (activemenu == 1) {
//			//aller vers le bas
//			if (Input.GetKeyDown ("v")) {
//				posbouton = posbouton + 1;
//				audio.PlayOneShot (move, 0.9F);
//				over ();
//				highlight ();
//			}

//			//aller vers le haut
//			if (Input.GetKeyDown ("f")) {
//				posbouton = posbouton - 1;
//				audio.PlayOneShot (move, 0.9F);
//				over ();
//				highlight ();
//			}
//			//changement de menu depuis mainmenu
//			if (Input.GetKeyUp("h") && posbouton == 0){
//				audio.PlayOneShot(select, 1.0F);
//				playmenu ();
//			}
//			if (Input.GetKeyUp("h") && posbouton == 1){
//				audio.PlayOneShot(select, 1.0F);
//				custommenu ();
//			}		
//			if (Input.GetKeyUp("h") && posbouton == 2){
//				audio.PlayOneShot(select, 1.0F);
//				creditsmenu ();
//			}
//		}

//		//deplacement curseur menu play
//		if (activemenu == 2) {
//			if (posbouton < 3) {
//				if (Input.GetKeyDown ("v"))  {
//					posbouton = posbouton + 1;
//					audio.PlayOneShot (move, 0.9F);
//					highlight ();
//				}
//			}
//			if (posbouton > 0) {
//				if (Input.GetKeyDown ("f")) {
//					posbouton = posbouton - 1;
//					audio.PlayOneShot (move, 0.9F);
//					highlight ();
//				}
//			}
//		}
//		// selection de party depuis menu play
//		if (activemenu == 2){
//			if (Input.GetKeyDown ("h") && gameselected == false){
//				gameselected = true;
//				if (posbouton == 0){
//					audio.PlayOneShot(select, 1.0F);
//					print ("party one selected");
//					highlight ();
//				}
//				if (posbouton == 1){
//					audio.PlayOneShot(select, 1.0F);
//					print ("party two selected");
//					highlight ();
//				}
//				if (posbouton == 2){
//					audio.PlayOneShot(select, 1.0F);
//					print ("party three selected");
//					highlight ();
//				}
//				if (posbouton == 3){
//					audio.PlayOneShot(select, 1.0F);
//					print ("hosting selected");
//					highlight ();
//				}
//			}
//			else if (gameselected == true){
//				if (Input.GetKeyDown ("h")){
//					audio.PlayOneShot(select, 1.0F);
//					plmenu.SetActive (false);
//					parmenu.SetActive (true);

//				}
//				if (Input.GetKeyDown ("g")){
//					audio.PlayOneShot(back, 1.0F);
//					gameselected = false;
//					parmenu.SetActive (false);
//					plmenu.SetActive (true);
//					posbouton = 0;
//					highlight ();
//				}
//			}
//		}

	
//		//deplacement curseur menu custom
//		if (activemenu == 3) {
//				if (posbouton%2 == 0) {
//					if (Input.GetKeyDown("v")){
//						posbouton = posbouton +1;
//						over ();
//						highlight ();
//					}
//					if (Input.GetKeyDown("b")) {

//						posbouton = posbouton +2;
//						over ();
//						highlight ();
//					}
//					if (Input.GetKeyDown("c")) {
//						posbouton = posbouton -2;
//						over ();
//						highlight ();
//					}
//				}
//				if (posbouton%2 == 1) {
//					if (Input.GetKeyDown ("f")){
				
//						posbouton = posbouton -1;
//						over ();
//						highlight ();
//					}
//					if (Input.GetKeyDown("b")) {
//						posbouton = posbouton +2;
//						over ();
//						highlight ();
//					}
//					if (Input.GetKeyDown("c")) {
//						posbouton = posbouton -2;
//						over ();
//						highlight ();
//					}
//				}
//			}
//	}




////FONCTIONS Appelées

//	//pour faire le tour du menu
//	void over () {
//		if (activemenu == 1){
//			if (posbouton > 2) {
//			posbouton = 0;
//			}
//			if (posbouton < 0) {
//			posbouton = 2;
//			}
//		}
//		if (activemenu == 3) {
//			if (posbouton > 4 && posbouton % 2 == 0) {
//				posbouton = 0;
//			}
//			if (posbouton < 0 && posbouton % 2 == 0) {
//				posbouton = 4;
//			}
//			if (posbouton > 5 && posbouton % 2 == 1) {
//				posbouton = 1;
//			}
//			if (posbouton < 0 && posbouton % 2 == -1) {
//				posbouton = 5;
//			}
//		}
//	}

//	//pour changer le sprite des boutons quand le sélecteur est dessus
//	void highlight () {
//		if (activemenu == 1) {
//			if (posbouton == 0) {
//				sr1.sprite = highlight1;
//				if (sr2.sprite != normal2) {
//					sr2.sprite = normal2;
//				}
//				if (sr3.sprite != normal3) {
//					sr3.sprite = normal3;
//				}
//			}
//			if (posbouton == 1) {
//				sr2.sprite = highlight2;
//				if (sr1.sprite != normal1) {
//					sr1.sprite = normal1;
//				}
//				if (sr3.sprite != normal3) {
//					sr3.sprite = normal3;
//				}
//			}
//			if (posbouton == 2) {
//				sr3.sprite = highlight3;
//				if (sr1.sprite != normal1) {
//					sr1.sprite = normal1;
//				}
//				if (sr2.sprite != normal2) {
//					sr2.sprite = normal2;
//				}
//			}

//		}
//		if (activemenu == 2) {
//			if (gameselected == false) {
//				if (posbouton == 0) {
//					panpar1.GetComponent<SpriteRenderer> ().sprite = partyhighlight;
//					panpar2.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					panpar3.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					hosting.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					confirm.GetComponent<SpriteRenderer> ().sprite = partydispo;
//				}
//				if (posbouton == 1) {
//					panpar2.GetComponent<SpriteRenderer> ().sprite = partyhighlight;
//					panpar1.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					panpar3.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					hosting.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					confirm.GetComponent<SpriteRenderer> ().sprite = partydispo;
//				}
//				if (posbouton == 2) {
//					panpar3.GetComponent<SpriteRenderer> ().sprite = partyhighlight;
//					panpar2.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					panpar1.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					hosting.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					confirm.GetComponent<SpriteRenderer> ().sprite = partydispo;
//				}
//				if (posbouton == 3) {
//					hosting.GetComponent<SpriteRenderer> ().sprite = partyhighlight;
//					panpar3.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					panpar2.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					panpar1.GetComponent<SpriteRenderer> ().sprite = partydispo;
//					confirm.GetComponent<SpriteRenderer> ().sprite = partydispo;
//				}
//			}
//			if (gameselected == true) {
//			confirm.GetComponent<SpriteRenderer> ().sprite = partyhighlight;
//			}
//		}
//		if (activemenu == 3) {
//			if (posbouton == 0) {
//				sred.sprite = red2;
//				sblue.sprite = blue1;
//				sgreen.sprite = green1;
//				spink.sprite = pink1;
//				sorange.sprite = orange1;
//				syellow.sprite = yellow1;
//			}
//			if (posbouton == 1) {
//				sred.sprite = red1;
//				sblue.sprite = blue2;
//				sgreen.sprite = green1;
//				spink.sprite = pink1;
//				sorange.sprite = orange1;
//				syellow.sprite = yellow1;

//			}
//			if (posbouton == 2) {
//				sred.sprite = red1;
//				sblue.sprite = blue1;
//				sgreen.sprite = green2;
//				spink.sprite = pink1;
//				sorange.sprite = orange1;
//				syellow.sprite = yellow1;
//			}
//			if (posbouton == 3) {
//				sred.sprite = red1;
//				sblue.sprite = blue1;
//				sgreen.sprite = green1;
//				spink.sprite = pink2;
//				sorange.sprite = orange1;
//				syellow.sprite = yellow1;
//			}
//			if (posbouton == 4) {
//				sred.sprite = red1;
//				sblue.sprite = blue1;
//				sgreen.sprite = green1;
//				spink.sprite = pink1;
//				sorange.sprite = orange2;
//				syellow.sprite = yellow1;
//			}
//			if (posbouton == 5) {
//				sred.sprite = red1;
//				sblue.sprite = blue1;
//				sgreen.sprite = green1;
//				spink.sprite = pink1;
//				sorange.sprite = orange1;
//				syellow.sprite = yellow2;
//			}
//		}
//	}

//	//pour aller vers le menu play
//	void playmenu (){
//		plmenu.SetActive (true);
//		activemenu = 2;
//		posbouton = 0;
//		sr1.enabled = false;
//		sr2.enabled = false;
//		sr3.enabled = false;
//		highlight ();
//	}
//	//pour aller vers le menu custom
//	void custommenu (){
//		activemenu = 3;
//		posbouton = 0;
//		sr1.enabled = false;
//		sr2.enabled = false;
//		sr3.enabled = false;
//		custmenu.SetActive(true);
//		highlight ();
//	}
//	//pour aller vers les crédits
//	void creditsmenu (){
//		credmenu.SetActive (true);
//		activemenu = 4;
//		posbouton = 0;
//		sr1.enabled = false;
//		sr2.enabled = false;
//		sr3.enabled = false;
//	}
//	//pour retourner au main menu
//	void mainmenu (){
//		credmenu.SetActive (false);
//		plmenu.SetActive (false);
//		custmenu.SetActive(false);
//		activemenu = 1;
//		posbouton = 0;
//		srecran.enabled = true;
//		sr1.enabled = true;
//		sr2.enabled = true;
//		sr3.enabled = true;
//		highlight ();
//	}
	


//}