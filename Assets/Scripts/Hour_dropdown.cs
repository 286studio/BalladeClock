using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hour_dropdown : MonoBehaviour
{
    Dropdown dropdown;
    // Start is called before the first frame update
    void OnEnable()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        List<string> m_DropOptions = new List<string>();
        for (int i = 1; i < 13; ++i)
        {
            m_DropOptions.Add(i.ToString());
        }
        dropdown.AddOptions(m_DropOptions);
        dropdown.value = System.DateTime.Now.Hour == 0 ? 11 : (System.DateTime.Now.Hour - 1) % 12;
    }
}
