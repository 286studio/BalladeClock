using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthAdjust : MonoBehaviour
{
    [SerializeField] RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        var size = rt.sizeDelta;
        size.x = AppManager.AdjustedWidth;
        rt.sizeDelta = size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
