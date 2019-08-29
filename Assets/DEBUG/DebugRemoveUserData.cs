using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DebugRemoveUserData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate
        {
            string path = Application.persistentDataPath + "/AlarmListUserData.b2cud";
            string path2 = Application.persistentDataPath + "/SettingUserData.b2cud";
            File.Delete(path);
            File.Delete(path2);
        });
    }
}
