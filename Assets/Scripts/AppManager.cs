using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    static public Vector2 DefaultRes = new Vector2(1125, 2436);
    static public float AdjustedWidth;
    static public GameObject[] Prefabs;
    public GameObject[] Prefabs_loader;

    // Start is called before the first frame update
    void Awake()
    {
        Prefabs = Prefabs_loader;
        AdjustedWidth = DefaultRes.y * Screen.width / Screen.height;
        Application.targetFrameRate = 60; // FPS改到60
    }
}
