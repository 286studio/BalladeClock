using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMPM_dropdown : MonoBehaviour
{
    Dropdown dropdown;
    // Start is called before the first frame update
    void OnEnable()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.ClearOptions();
        List<string> m_DropOptions = new List<string>();
        m_DropOptions.Add("AM");
        m_DropOptions.Add("PM");
        dropdown.AddOptions(m_DropOptions);
        dropdown.value = (System.DateTime.Now.Hour >= 12) ? 1 : 0;
    }
}
