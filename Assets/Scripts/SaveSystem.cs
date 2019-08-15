using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    // Alarm list related

    // Save user setting in case user close the app
    public static void SaveAlarmListUserData(_AlarmListUserData ud)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/AlarmListUserData.b2cud";
        FileStream stream = new FileStream(path, FileMode.Create);
        bf.Serialize(stream, ud);
        stream.Close();
    }

    public static _AlarmListUserData LoadAlarmListUserData()
    {
        string path = Application.persistentDataPath + "/AlarmListUserData.b2cud";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            _AlarmListUserData ud = bf.Deserialize(stream) as _AlarmListUserData;
            stream.Close();
            return ud;
        }
        return null;
    }

    public static void SaveSettingUserData(_SettingUserData ud)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SettingUserData.b2cud";
        FileStream stream = new FileStream(path, FileMode.Create);
        bf.Serialize(stream, ud);
        stream.Close();
    }

    public static _SettingUserData LoadSettingUserData()
    {
        string path = Application.persistentDataPath + "/SettingUserData.b2cud";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            _SettingUserData ud = bf.Deserialize(stream) as _SettingUserData;
            stream.Close();
            return ud;
        }
        return null;
    }
}
