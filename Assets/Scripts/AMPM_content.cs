using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMPM_content : MonoBehaviour
{

    int ampm_val;
    RectTransform rt;

    // Start is called before the first frame update
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        setValue(0);
    }

    void FixedUpdate()
    {
        // 中间数字趋向居中
        var pos = rt.anchoredPosition3D;
        rt.anchoredPosition3D = Vector3.MoveTowards(rt.anchoredPosition3D, new Vector3(0f, pos.y > 75f ? 150f : 0f, 0f), 50 * Time.fixedDeltaTime);

		// 当前时
		ampm_val = pos.y > 75f ? 1 : 0;
	}

    public int getValue()
    {
        return ampm_val;
    }

    public void setValue(int v)
    {
        ampm_val = v;
        if (v == 0) rt.anchoredPosition = new Vector2();
        else rt.anchoredPosition = new Vector2(0f, 150f);
    }
}
