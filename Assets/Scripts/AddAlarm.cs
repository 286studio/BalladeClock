using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.iOS;

public class AddAlarm : MonoBehaviour
{
    public GameObject AlarmElementPrefab;
    public Hr_content Hour_dropdown; // 1 ~ 12
    public Min_content Minute_dropdown; // 0 ~ 59
    public AMPM_content AMPM_dropdown; // AM, PM
    public Dropdown Repeat_dropdown; // Never, daily
    public Dropdown Ringer_drowndown;
    public InputField Label_input; // input field
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

        string time = (AMPM_dropdown.getValue() == 0) ? "上午 " : "下午 ";
        time += Hour_dropdown.getValue().ToString();
        time += ":";
        time += Minute_dropdown.getValue() < 10 ? "0" + Minute_dropdown.getValue() : Minute_dropdown.getValue().ToString();

        var ae = newAlarm.GetComponent<AlarmElement>();
        ae.TimeString.text = time;
        ae.profile_idx = CharacterSetting._ins.curCharacter;
        ae.ringer_dp_val = Ringer_drowndown.value;
        ae.Profile.sprite = ae.profile_sprites[ae.ringer_dp_val];
        ae.hr_dp_val = Hour_dropdown.getValue();
        ae.min_dp_val = Minute_dropdown.getValue();
        ae.ampm_dp_val = AMPM_dropdown.getValue();
        ae.repeat_dp_val = Repeat_dropdown.value;
        ae.label_if_val = Label_input.text;
        ae.Id = Notifications.AddAlarm(ae.hr_dp_val, ae.min_dp_val, ae.ampm_dp_val, ae.repeat_dp_val == 0, ae.label_if_val, ae.ringer_dp_val);

        // add the alarm to list
        AlarmList.ui_alarmlist.Add(newAlarm);
        // reorder the list
        AppManager.Prefabs[0].GetComponent<AlarmList>().Reorder();
        // save
        AlarmList.SaveAlarmListToFile();
        // 此界面任务完成，把自身设为不可见
        Swipable.allow_swipe = true;
        gameObject.SetActive(false);
    }

    void cancelButtonClick()
    {
        AppManager.Prefabs[0].SetActive(true);
        Swipable.allow_swipe = true;
        gameObject.SetActive(false);
    }
}
