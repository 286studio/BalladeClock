using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.iOS;

public class AddAlarm : MonoBehaviour
{
    public GameObject AlarmElementPrefab;
    public Dropdown Hour_dropdown; // 1 ~ 12
    public Dropdown Minute_dropdown; // 0 ~ 59
    public Dropdown AMPM_dropdown; // AM, PM
    public Dropdown Repeat_dropdown; // Never, daily
    public InputField Label_input; // input field
    public Toggle SnoozeToggle; // snooze or not
    public Button submitButton;
    public Button cancelButton;
    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(submitButtonClick);
        cancelButton.onClick.AddListener(cancelButtonClick);
    }

    void submitButtonClick()
    {
        // AlarmList界面设为可见
        AppManager.Prefabs[0].SetActive(true);
        var newAlarm = Instantiate(AlarmElementPrefab, AppManager.Prefabs[2].transform);
        string time = (Hour_dropdown.value + 1).ToString();
        time += ":";
        time += Minute_dropdown.value < 10 ? "0" + Minute_dropdown.value : Minute_dropdown.value.ToString();
        time += " ";
        time += (AMPM_dropdown.value == 0) ? "AM" : "PM";

        var ae = newAlarm.GetComponent<AlarmElement>();
        ae.TimeString.text = time;
        ae.profile_idx = CharacterSetting._ins.curCharacter;
        ae.Profile.sprite = ae.profile_sprites[ae.profile_idx];
        ae.hr_dp_val = Hour_dropdown.value;
        ae.min_dp_val = Minute_dropdown.value;
        ae.ampm_dp_val = AMPM_dropdown.value;
        ae.repeat_dp_val = Repeat_dropdown.value;
        ae.label_if_val = Label_input.text;
        ae.snooze_tg_val = SnoozeToggle.isOn;
        ae.Id = Notifications.AddAlarm(ae.hr_dp_val + 1, ae.min_dp_val, ae.ampm_dp_val, ae.repeat_dp_val == 0, ae.label_if_val);

        // add the alarm to list
        AlarmList.ui_alarmlist.Add(newAlarm);
        // reorder the list
        AppManager.Prefabs[0].GetComponent<AlarmList>().Reorder();
        // save
        AlarmList.SaveAlarmListToFile();
        // 此界面任务完成，把自身设为不可见
        gameObject.SetActive(false);
    }

    void cancelButtonClick()
    {
        AppManager.Prefabs[0].SetActive(true);
        // do nothing
        gameObject.SetActive(false);
    }
}
