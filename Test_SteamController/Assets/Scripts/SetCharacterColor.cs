using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SetCharacterColor : NetworkBehaviour
{

    public enum ROBOT_COLOR
    {
        ORANGE = 0,
        RED,
        BLUE,
        YELLOW,
        PINK,
        GREEN
    };



    [SyncVar]
    ROBOT_COLOR robotColor = ROBOT_COLOR.ORANGE;

    ROBOT_COLOR saverColor;

    public GameObject leftArmMesh, rightArmMesh, topMesh, bottomMesh, bodyMesh;

    public Material[] top, arm, bottom, body;
    public Texture2D[] cockpits;

    void Start()
    {
        if (isLocalPlayer)
        {
            saverColor = GameObject.Find("ColorSaver").GetComponent<ColorSave>().GetColor();
            Destroy(GameObject.Find("ColorSaver"));
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (robotColor != saverColor)
            {
                CmdSetcolor(saverColor);
            }
        }
        if(leftArmMesh.GetComponent<Renderer>().material != arm[(int)robotColor])
        {
            leftArmMesh.GetComponent<Renderer>().material = arm[(int)robotColor];
            rightArmMesh.GetComponent<Renderer>().material = arm[(int)robotColor];
            topMesh.GetComponent<Renderer>().material = top[(int)robotColor];
            bottomMesh.GetComponent<Renderer>().material = bottom[(int)robotColor];
            bodyMesh.GetComponent<Renderer>().material = body[(int)robotColor];
            GetComponent<CharacterLife>().cockpit = cockpits[(int)robotColor];
        }
    }

    [Command]
    void CmdSetcolor(ROBOT_COLOR c)
    {
        robotColor = c;
    }

}
