using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.iOS;

public class EditAlarm : MonoBehaviour
{
    public AlarmElement EditingAlarm;
    public int Id;
    public Dropdown Hour_dropdown; // 1 ~ 12
    public Dropdown Minute_dropdown; // 0 ~ 59
    public Dropdown AMPM_dropdown; // AM, PM
    public Dropdown Repeat_dropdown; // Never, daily
    public InputField Label_input; // input field
    public Toggle SnoozeToggle; // snooze or not

    public void submitButtonClick()
    {
        // handle ui
        AppManager.Prefabs[0].gameObject.SetActive(true);

        string time = (Hour_dropdown.value + 1).ToString();
        time += ":";
        time += Minute_dropdown.value < 10 ? "0" + Minute_dropdown.value : Minute_dropdown.value.ToString();
        time += " ";
        time += (AMPM_dropdown.value == 0) ? "AM" : "PM";

        EditingAlarm.TimeString.text = time;
        EditingAlarm.Id = Id;
        EditingAlarm.hr_dp_val = Hour_dropdown.value;
        EditingAlarm.min_dp_val = Minute_dropdown.value;
        EditingAlarm.ampm_dp_val = AMPM_dropdown.value;
        EditingAlarm.repeat_dp_val = Repeat_dropdown.value;
        EditingAlarm.label_if_val = Label_input.text;
        EditingAlarm.snooze_tg_val = SnoozeToggle.isOn;

        // handle notification
        // cancel the old one
        Notifications.RemoveAlarm(Id);
        // insert a new one
        Id = Notifications.AddAlarm(Hour_dropdown.value + 1, Minute_dropdown.value, AMPM_dropdown.value, Repeat_dropdown.value == 0, Label_input.text);
        AlarmList.SaveAlarmListToFile();
        Destroy(gameObject);
    }

    public void deleteButtonClick()
    {
        AppManager.Prefabs[0].gameObject.SetActive(true);
        EditingAlarm.X_button_click();
        Destroy(gameObject);
    }
}
