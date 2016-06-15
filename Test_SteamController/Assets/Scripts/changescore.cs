using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class changescore : MonoBehaviour {
	public Text Mykills;
	public Text Mydeath;
	public Text j2kills;
	public Text j2death;
	public Text j3kills;
	public Text j3death;
	private int myscore = 0;
	private string myscorestring;
	private int mydeathamount = 0;
	private string mydeathamountstring;
	private int j2score = 0;
	private string j2scorestring;
	private int j2deathamount = 0;
	private string j2deathamountstring;
	private int j3score = 0;
	private string j3scorestring;
	private int j3deathamount = 0;
	private string j3deathamountstring;

    //bool displayScore = false;
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}

    public void SetMyScore(GameManager.Score s)
    {

        Debug.Log("SCORECHANGE");
        myscore = s.kills;
        mydeathamount = s.deaths;
        Scorechange();
    }

    public void SetOtherScore(GameManager.Score s, GameManager.Score s2)
    {

        Debug.Log("SCORECHANGE");
        j2score = s.kills;
        j2deathamount = s.deaths;
        j3score = s2.kills;
        j3deathamount = s2.deaths;
        Scorechange();
    }

    // Update is called once per frame
    void Update ()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            GetComponent<Canvas>().enabled = true;

            if(Input.GetKeyUp(KeyCode.Return))
            {
                Destroy(gameObject);
            }
        }

        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            if(Input.GetKeyDown(KeyCode.RightShift))
            {
                GetComponent<Canvas>().enabled = true;
            }
            if(Input.GetKeyUp(KeyCode.RightShift))
            {
                GetComponent<Canvas>().enabled = false;
            }
        }
    }

	void Scorechange(){
        myscorestring = myscore.ToString ();
		Mykills.text = myscorestring;
		mydeathamountstring = mydeathamount.ToString ();
		Mydeath.text = mydeathamountstring;
		j2scorestring = j2score.ToString ();
		j2kills.text = j2scorestring;
		j2deathamountstring = j2deathamount.ToString ();
		j2death.text = j2deathamountstring;
		j3scorestring = j3score.ToString ();
		j3kills.text = j3scorestring;
		j3deathamountstring = j3deathamount.ToString ();
		j3death.text = j3deathamountstring;
	}
}
