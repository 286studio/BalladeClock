using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    static public Vector2 DefaultRes = new Vector2(1125, 2436);
    static public bool isIPad;
    static public float AdjustedWidth;
    static public float AdjustedHeight;
    static public GameObject[] Prefabs;
    public GameObject[] Prefabs_loader;

    // Start is called before the first frame update
    void Awake()
    {
        Prefabs = Prefabs_loader;
        AdjustedWidth = DefaultRes.y * Screen.width / Screen.height;
        AdjustedHeight = DefaultRes.x * Screen.height / Screen.width;
        Application.targetFrameRate = 60; // FPS改到60
        TouchScreenKeyboard.hideInput = true; // hide input box on the keypad
        isIPad = (float)Screen.width / Screen.height > DefaultRes.x / DefaultRes.y;
        print(Screen.width + "x" + Screen.height + " Ratio: " + (float)Screen.width / Screen.height + " Default Ratio: " + DefaultRes.x / DefaultRes.y);
        print(isIPad);
    }

    public static void MainTimeToBlack()
    {
        var MainTime = GameObject.Find("MainUI_Time");
        foreach (var t in MainTime.GetComponentsInChildren<Text>())
        {
            t.color = Color.black;
            t.GetComponent<Shadow>().enabled = false;
        }
    }

    public static void MainTimeToWhite()
    {
        var MainTime = GameObject.Find("MainUI_Time");
        foreach (var t in MainTime.GetComponentsInChildren<Text>())
        {
            t.color = Color.white;
            t.GetComponent<Shadow>().enabled = true;
        }
    }
}
