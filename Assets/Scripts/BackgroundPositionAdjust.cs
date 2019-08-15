using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPositionAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f));
        pos.y = 0;
        pos.z = 2;
        transform.localPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
