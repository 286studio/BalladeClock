using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minute_dropdown : MonoBehaviour
{
    Dropdown dropdown;
    // Start is called before the first frame update
    void OnEnable()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        List<string> m_DropOptions = new List<string>();
        for (int i = 0; i < 60; ++i)
        {
            m_DropOptions.Add(i.ToString());
        }
        dropdown.AddOptions(m_DropOptions);
        dropdown.value = System.DateTime.Now.Minute;
    }
}
