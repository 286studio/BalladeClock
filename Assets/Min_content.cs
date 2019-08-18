using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Min_content : MonoBehaviour
{
    public GameObject minutes_text;
    public int min_val; // TODO
    RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        for (int k = -3; k < 63; ++k)
        {
            int i = k % 60;
            if (i < 0) i += 60;
            var min = Instantiate(minutes_text, transform);
            min.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 4875f - 150f * (k + 3));
            min.GetComponent<Text>().text = i < 10 ? "0" + i : i.ToString();
            rt.anchoredPosition = new Vector2(0, 3000);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 首尾相接效果
        if (rt.anchoredPosition.y < 300f) rt.anchoredPosition += new Vector2(0, 9000f);
        if (rt.anchoredPosition.y > 9450f) rt.anchoredPosition -= new Vector2(0, 9000f);
    }

    void Update()
    {
        // 中间数字趋向居中
        var pos = rt.anchoredPosition3D;
        var target = new Vector3(0, ((int)pos.y + 75) / 150 * 150);
        rt.anchoredPosition3D = Vector3.MoveTowards(rt.anchoredPosition3D, target, 2f);
        // min_val = ??? TODO
    }
}
