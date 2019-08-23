using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Min_content : MonoBehaviour
{
    public GameObject minutes_text;
    int min_val; // TODO
    RectTransform rt;

    // Start is called before the first frame update
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        for (int k = -3; k < 63; ++k)
        {
            int i = k % 60;
            if (i < 0) i += 60;
            var min = Instantiate(minutes_text, transform);
            min.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 4875f - 150f * (k + 3));
            min.GetComponent<Text>().text = i < 10 ? "0" + i : i.ToString();
        }
        setValue(59);
    }

    float last_y;
    void FixedUpdate()
    {
        // 首尾相接效果
        if (rt.anchoredPosition.y < 300f) rt.anchoredPosition += new Vector2(0, 9000f);
        if (rt.anchoredPosition.y > 9450f) rt.anchoredPosition -= new Vector2(0, 9000f);

        // 中间数字趋向居中
        var pos = rt.anchoredPosition3D;
        var v = (pos.y - last_y) / Time.fixedDeltaTime;
        last_y = pos.y;
        if (v < 50 && Input.touches.Length == 0)
        {
            var target = new Vector3(0, ((int)pos.y + 75) / 150 * 150);
            rt.anchoredPosition3D = Vector3.MoveTowards(rt.anchoredPosition3D, target, 150 * Time.fixedDeltaTime);
        }

        // 当前分
        min_val = ((int)pos.y + 75) / 150 - 2;
        if (min_val == 60) min_val = 0;
    }

    public int getValue()
    {
        return min_val;
    }

    public void setValue(int m)
    {
        min_val = m;
        last_y = m * 150f + 300f;
        rt.anchoredPosition = new Vector2(0, last_y);
    }
}
