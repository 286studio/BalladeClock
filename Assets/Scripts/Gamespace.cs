using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamespace : MonoBehaviour
{
    public GameObject _background;
    public GameObject[] _characters;

    public static GameObject Background;
    public static GameObject[] Characters;

    // Start is called before the first frame update
    void Awake()
    {
        Background = _background;
        Characters = _characters;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
