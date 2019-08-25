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
    public Button returnButton;
    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(submitButtonClick);
        cancelButton.onClick.AddListener(cancelButtonClick);
        returnButton.onClick.AddListener(cancelButtonClick);
    }


    int tCount = 255;
    int tCount2 = 255;
    private void FixedUpdate()
    {
        // manipulate transparency
        int rdcc = Repeat_dropdown.transform.childCount;
        if (rdcc == 5)
        {
            tCount += 5;
            if (tCount > 255) tCount = 255;
        }
        else
        {
            tCount -= 5;
            if (tCount < 0) tCount = 0;
        }

        // set ringer dropdown transparency
        var cmps = Ringer_drowndown.GetComponentsInChildren<Image>();
        for (int i = 1; i < 5; ++i)
        {
            var clr = cmps[i].color;
            clr.a = tCount / 255f;
            cmps[i].color = clr;
        }
        var rText = Ringer_drowndown.GetComponentInChildren<Text>();
        var c = rText.color;
        c.a = tCount / 255f;
        rText.color = c;

        // manipulate transparency 2
        int rdcc2 = Ringer_drowndown.transform.childCount;
        if (rdcc2 == 6)
        {
            tCount2 += 5;
            if (tCount2 > 255) tCount2 = 255;
        }
        else
        {
            tCount2 -= 15;
            if (tCount2 < 0) tCount2 = 0;
        }
        // set buttons transparency
        var DoneButtonImages = submitButton.GetComponentsInChildren<Image>();
        for (int i = 1; i <= 2; ++i)
        {
            var clr = DoneButtonImages[i].color;
            clr.a = tCount2 / 255f;
            DoneButtonImages[i].color = clr;
        }
        var cancelButtonImages = cancelButton.GetComponentsInChildren<Image>();
        for (int i = 1; i <= 2; ++i)
        {
            var clr = cancelButtonImages[i].color;
            clr.a = tCount2 / 255f;
            cancelButtonImages[i].color = clr;
        }

    }

    public void submitButtonClick()
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

    public void cancelButtonClick()
    {
        AppManager.Prefabs[0].SetActive(true);
        Swipable.allow_swipe = true;
        gameObject.SetActive(false);
    }
}
