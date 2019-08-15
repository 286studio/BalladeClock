using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minute_initialize : MonoBehaviour
{
    // Start is called before the first frame update
    Dropdown dropdown;
    void Awake()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        List<string> m_DropOptions = new List<string>();
        for (int i = 0; i < 60; ++i)
        {
            m_DropOptions.Add(i.ToString());
        }
        dropdown.AddOptions(m_DropOptions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
