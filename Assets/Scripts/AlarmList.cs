using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmList : MonoBehaviour
{
    public Text CurrentTime;
    public GameObject NoAlarms;
    public GameObject AlarmElementPrefab;
    public Button AddButtton;
    public Button Return;
    public RectTransform ContentRT;
    static public List<GameObject> ui_alarmlist;
    static public float spacer = 200f;
    // Start is called before the first frame update
    void Start()
    {
        print(Application.persistentDataPath);
        if (ui_alarmlist == null) ui_alarmlist = new List<GameObject>();
        AddButtton.onClick.AddListener(AddButtonClick);
        LoadAlarmListFromFile();
        Reorder();
    }

    void AddButtonClick()
    {
        AppManager.Prefabs[1].gameObject.SetActive(true);
        AddAlarm aa = AppManager.Prefabs[1].GetComponent<AddAlarm>();
        aa.Hour_dropdown.setValue(System.DateTime.Now.Hour == 0 ? 12 : System.DateTime.Now.Hour % 12);
        aa.Minute_dropdown.setValue(System.DateTime.Now.Minute);
        aa.AMPM_dropdown.setValue((System.DateTime.Now.Hour >= 12) ? 1 : 0);
        gameObject.SetActive(false);
    }

    public void Reorder()
    {
        if (ui_alarmlist.Count > 0) NoAlarms.gameObject.SetActive(false);
        else NoAlarms.gameObject.SetActive(true);

        var size = ContentRT.sizeDelta;
        size.y = ui_alarmlist.Count * spacer + 60;
        ContentRT.sizeDelta = size;

        for (int i = 0; i < ui_alarmlist.Count; ++i)
        {
            var pos = ui_alarmlist[i].transform.localPosition;
            pos.y = -100f - spacer * i;
            ui_alarmlist[i].transform.localPosition = pos;
        }
    }

    private void Update()
    {
        CurrentTime.text = System.DateTime.Now.ToShortTimeString();
    }

    public static void SaveAlarmListToFile()
    {
        _AlarmListUserData ud = new _AlarmListUserData();
        ud.nid = Notifications.Id;
        if (ui_alarmlist.Count > 0)
        {
            ud.numAlarms = ui_alarmlist.Count;
            ud.id = new int[ud.numAlarms];
            ud.profile_idx = new int[ud.numAlarms];
            ud.hr_dp_val = new int[ud.numAlarms];
            ud.min_dp_val = new int[ud.numAlarms];
            ud.ampm_dp_val = new int[ud.numAlarms];
            ud.repeat_dp_val = new int[ud.numAlarms];
            ud.ringer_dp_val = new int[ud.numAlarms];
            ud.label_if_val = new string[ud.numAlarms];
            for (int i = 0; i < ui_alarmlist.Count; ++i)
            {
                AlarmElement ae_comp = ui_alarmlist[i].GetComponent<AlarmElement>();
                ud.id[i] = ae_comp.Id;
                ud.profile_idx[i] = ae_comp.profile_idx;
                ud.hr_dp_val[i] = ae_comp.hr_dp_val;
                ud.min_dp_val[i] = ae_comp.min_dp_val;
                ud.ampm_dp_val[i] = ae_comp.ampm_dp_val;
                ud.repeat_dp_val[i] = ae_comp.repeat_dp_val;
                ud.ringer_dp_val[i] = ae_comp.ringer_dp_val;
                ud.label_if_val[i] = ae_comp.label_if_val;
            }
        }
        else
        {
            ud.numAlarms = 0;
            ud.id = new int[] { };
            ud.profile_idx = new int[] { };
            ud.hr_dp_val = new int[] { };
            ud.min_dp_val = new int[] { };
            ud.ampm_dp_val = new int[] { };
            ud.repeat_dp_val = new int[] { };
            ud.ringer_dp_val = new int[] { };
            ud.label_if_val = new string[] { };
        }
        SaveSystem.SaveAlarmListUserData(ud);
    }

    public void LoadAlarmListFromFile()
    {
        _AlarmListUserData ud = SaveSystem.LoadAlarmListUserData();

        if (ud != null)
        {
            Notifications.Id = ud.nid;
            for (int i = 0; i < ud.numAlarms; ++i)
            {
                var newAlarm = Instantiate(AlarmElementPrefab, AppManager.Prefabs[2].transform);
                string time = (ud.ampm_dp_val[i] == 0) ? "上午 " : "下午 ";
                time += ud.hr_dp_val[i].ToString();
                time += ":";
                time += ud.min_dp_val[i] < 10 ? "0" + ud.min_dp_val[i] : ud.min_dp_val[i].ToString();
                time += " ";
                var ae = newAlarm.GetComponent<AlarmElement>();
                ae.TimeString.text = time;
                ae.Id = ud.id[i];
                ae.profile_idx = ud.profile_idx[i];
                ae.ringer_dp_val = ud.ringer_dp_val[i];
                ae.Profile.sprite = ae.profile_sprites[ae.ringer_dp_val];
                ae.hr_dp_val = ud.hr_dp_val[i];
                ae.min_dp_val = ud.min_dp_val[i];
                ae.ampm_dp_val = ud.ampm_dp_val[i];
                ae.repeat_dp_val = ud.repeat_dp_val[i];
                ae.label_if_val = ud.label_if_val[i];
                ui_alarmlist.Add(newAlarm);
            }
        }
    }
}
