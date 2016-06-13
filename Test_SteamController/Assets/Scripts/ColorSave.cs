using UnityEngine;
using System.Collections;

public class ColorSave : MonoBehaviour
{
    SetCharacterColor.ROBOT_COLOR robotColor = SetCharacterColor.ROBOT_COLOR.ORANGE;

    public SetCharacterColor.ROBOT_COLOR GetColor()
    {
        
        return robotColor;
    }

    public void SetColor(SetCharacterColor.ROBOT_COLOR c)
    {
        robotColor = c;
    }


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
