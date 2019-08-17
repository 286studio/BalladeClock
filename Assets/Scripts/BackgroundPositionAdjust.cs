using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPositionAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f));
        transform.position = new Vector3(-(10.24f - p.x), 1.82f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
