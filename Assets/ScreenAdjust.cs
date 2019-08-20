using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(AppManager.AdjustedWidth, AppManager.DefaultRes.y);
    }
}
