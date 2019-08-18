using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hr_content : MonoBehaviour
{
    public GameObject hours_text;
    int hr_val;
    RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        for (int k = -2; k < 16; ++k)
        {
            int i = k % 12;
            if (i <= 0) i += 12;
            var min = Instantiate(hours_text, transform);
            min.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1275f - 150f * (k + 2));
            min.GetComponent<Text>().text = i < 10 ? "0" + i : i.ToString();
        }
        setHourValue(12);
    }

    void FixedUpdate()
    {
        // 首尾相接效果
        if (rt.anchoredPosition.y < 150f) rt.anchoredPosition += new Vector2(0, 1800f);
        if (rt.anchoredPosition.y > 1950f) rt.anchoredPosition -= new Vector2(0, 1800f);

        // 中间数字趋向居中
        var pos = rt.anchoredPosition3D;
        var target = new Vector3(0, ((int)pos.y + 75) / 150 * 150);
        rt.anchoredPosition3D = Vector3.MoveTowards(rt.anchoredPosition3D, target, 50 * Time.fixedDeltaTime);

        // 当前时
        hr_val = ((int)pos.y + 75) / 150 - 1;
        if (hr_val == 0) hr_val = 12;
    }

    public int getHourValue()
    {
        return hr_val;
    }

    public void setHourValue(int h)
    {
        hr_val = h;
        rt.anchoredPosition = new Vector2(0, h * 150f + 150f);
    }
}
