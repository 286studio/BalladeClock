using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.iOS;

public class EditAlarm : MonoBehaviour
{
    [HideInInspector] public AlarmElement EditingAlarm;
    [HideInInspector] public int Id;
    public Hr_content Hour_dropdown; // 1 ~ 12
    public Min_content Minute_dropdown; // 0 ~ 59
    public AMPM_content AMPM_dropdown; // AM, PM
    public Dropdown Repeat_dropdown; // Never, daily
    public Dropdown Ringer_dropdown;
    public InputField Label_input; // input field

    public void submitButtonClick()
    {
        // handle ui
        AppManager.Prefabs[0].gameObject.SetActive(true);

        string time = (AMPM_dropdown.getValue() == 0) ? "上午 " : "下午 ";
        time += Hour_dropdown.getValue();
        time += ":";
        time += Minute_dropdown.getValue() < 10 ? "0" + Minute_dropdown.getValue() : Minute_dropdown.getValue().ToString();
        time += " ";

        EditingAlarm.TimeString.text = time;
        EditingAlarm.hr_dp_val = Hour_dropdown.getValue();
        EditingAlarm.min_dp_val = Minute_dropdown.getValue();
        EditingAlarm.ampm_dp_val = AMPM_dropdown.getValue();
        EditingAlarm.repeat_dp_val = Repeat_dropdown.value;
        EditingAlarm.ringer_dp_val = Ringer_dropdown.value;
        EditingAlarm.label_if_val = Label_input.text;
        EditingAlarm.Profile.sprite = EditingAlarm.profile_sprites[EditingAlarm.ringer_dp_val];

        // handle notification
        // cancel the old one
        Notifications.RemoveAlarm(Id);
        // insert a new one
        Id = Notifications.AddAlarm(Hour_dropdown.getValue(), Minute_dropdown.getValue(), AMPM_dropdown.getValue(), Repeat_dropdown.value == 0, Label_input.text, Ringer_dropdown.value);
        EditingAlarm.Id = Id;
        AlarmList.SaveAlarmListToFile();
        Swipable.allow_swipe = true;
        Destroy(gameObject);
    }

    public void deleteButtonClick()
    {
        AppManager.Prefabs[0].gameObject.SetActive(true);
        Swipable.allow_swipe = true;
        EditingAlarm.X_button_click();
        Destroy(gameObject);
    }

    public void returnButtonClick()
    {
        AppManager.Prefabs[0].gameObject.SetActive(true);
        Swipable.allow_swipe = true;
        Destroy(gameObject);
    }
}
