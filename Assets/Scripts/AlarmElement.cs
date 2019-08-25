using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.iOS;

public class AlarmElement : MonoBehaviour
{
    public GameObject EditAlarmPrefab;

    public Image Profile;
    public Sprite[] profile_sprites;
    public Text TimeString;
    public Button Edit_button;
    public Button X_button;

    // var
    public int profile_idx;
    public int hr_dp_val;
    public int min_dp_val;
    public int ampm_dp_val;
    public int repeat_dp_val;
    public int ringer_dp_val;
    public string label_if_val;

    public int Id;

    private void Awake()
    {
        AdjustToScreenRatio();
    }

    // Start is called before the first frame update
    void Start()
    {
        Edit_button.onClick.AddListener(Edit_button_click);
        X_button.onClick.AddListener(X_button_click);
    }

    public void Edit_button_click()
    {
        var EditUI = Instantiate(EditAlarmPrefab, GameObject.Find("Swipable_right").transform);
        var comp = EditUI.GetComponent<EditAlarm>();
        comp.EditingAlarm = this;
        comp.Id = Id;
        comp.Hour_dropdown.setValue(hr_dp_val);
        comp.Minute_dropdown.setValue(min_dp_val);
        comp.AMPM_dropdown.setValue(ampm_dp_val);
        comp.Repeat_dropdown.value = repeat_dp_val;
        comp.Label_input.text = label_if_val;
        comp.Ringer_dropdown.value = ringer_dp_val;
        Swipable.allow_swipe = false;
        AppManager.Prefabs[0].gameObject.SetActive(false);
    }

    public void X_button_click()
    {
        // remove the ui element
        Cover.SetActive(true);
        AlarmList.ui_alarmlist.Remove(gameObject);
        // remove the notification
        Notifications.RemoveAlarm(Id);
        AlarmList.SaveAlarmListToFile();
        // Destroy(gameObject);
        isDead = true;
    }

    const float outline_ratio = 2010f / 474f; // ratio of the image
    public void AdjustToScreenRatio()
    {
        var rt = GetComponent<RectTransform>();
        var newSzie = new Vector2(AppManager.AdjustedWidth, AppManager.AdjustedWidth / outline_ratio);
        var oldSize = rt.sizeDelta;
        var ratio = newSzie.x / oldSize.x;
        rt.sizeDelta = newSzie;

        AlarmList.spacer = 200f * ratio;

        var profile_rt = Profile.GetComponent<RectTransform>();
        profile_rt.anchoredPosition *= ratio;
        profile_rt.sizeDelta *= ratio;

        TimeString.fontSize = (int)((float)TimeString.fontSize * ratio);
        var timestr_rt = TimeString.GetComponent<RectTransform>();
        timestr_rt.anchoredPosition *= ratio;
        timestr_rt.sizeDelta *= ratio;

        var edit_rt = Edit_button.GetComponent<RectTransform>();
        edit_rt.anchoredPosition *= ratio;
        edit_rt.sizeDelta *= ratio;
        var edit_text = Edit_button.GetComponentInChildren<Text>();
        edit_text.fontSize = (int)((float)edit_text.fontSize * ratio);

        var x_rt = X_button.GetComponent<RectTransform>();
        x_rt.anchoredPosition *= ratio;
        x_rt.sizeDelta *= ratio;
        var x_text = X_button.GetComponentInChildren<Text>();
        x_text.fontSize = (int)((float)x_text.fontSize * ratio);
    }

    bool isDead = false;
    int deadCount = 255;
    public GameObject Cover;
    private void FixedUpdate()
    {
        if (isDead)
        {
            deadCount -= 5;
            if (deadCount == 0)
            {
                // reorder the ui element list
                AppManager.Prefabs[0].GetComponent<AlarmList>().Reorder();
                Destroy(gameObject);
            }
            foreach (var i in GetComponentsInChildren<Image>())
            {
                if (i.name == "Cover") continue;
                var c = i.color;
                c.a = (float)deadCount / 255f;
                i.color = c;
            }
            var tsc = TimeString.color;
            tsc.a = (float)deadCount / 255f;
            TimeString.color = tsc;
        }
    }
}
