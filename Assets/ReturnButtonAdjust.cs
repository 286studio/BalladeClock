using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButtonAdjust : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        float spacer = 100f * AppManager.AdjustedWidth / AppManager.DefaultRes.x;
        var rt = GetComponent<RectTransform>();
        var pos = rt.anchoredPosition;
        pos.x = -(AppManager.AdjustedWidth / 2 - spacer);
        pos.y = AppManager.DefaultRes.y / 2 - spacer;
        rt.anchoredPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
