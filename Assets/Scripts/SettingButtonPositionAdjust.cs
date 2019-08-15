using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButtonPositionAdjust : MonoBehaviour
{
    RectTransform rt;
    int spacer = 20;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        float pos_x = AppManager.AdjustedWidth / 2 - (rt.sizeDelta.x / 2 + spacer);
        float pos_y = -AppManager.DefaultRes.y / 2 + (rt.sizeDelta.y / 2 + spacer);
        rt.anchoredPosition = new Vector2(pos_x, pos_y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
