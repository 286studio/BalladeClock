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
    public GameObject BG;
    public Image MaskBG;
    bool Entering;
    bool Exiting;
    bool MaskIn = false;
    bool MaskOut = false;
    int MaskCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(submitButtonClick);
        cancelButton.onClick.AddListener(cancelButtonClick);
        returnButton.onClick.AddListener(cancelButtonClick);

        GetComponent<RectTransform>().localScale *= (AppManager.AdjustedWidth / AppManager.DefaultRes.x);
    }


    int tCount = 255;
    int tCount2 = 255;
    private void FixedUpdate()
    {
        // manipulate transparency
        int rdcc = Repeat_dropdown.transform.childCount;
        if (rdcc == 4)
        {
            tCount += 5;
            if (tCount > 255) tCount = 255;
        }
        else
        {
            tCount -= 5;
            if (tCount < 0) tCount = 0;
        }

        // manipulate transparency 2
        int rdcc2 = Ringer_drowndown.transform.childCount;
        if (rdcc2 == 5)
        {
            tCount2 += 5;
            if (tCount2 > 255) tCount2 = 255;
        }
        else
        {
            tCount2 -= 5;
            if (tCount2 < 0) tCount2 = 0;
        }



        // set repeat dropdown transparency
        var cmps = Repeat_dropdown.GetComponentsInChildren<Image>();
        for (int i = 1; i < 3; ++i)
        {
            var clr = cmps[i].color;
            clr.a = tCount2 / 255f;
            cmps[i].color = clr;
        }
        var rText = Repeat_dropdown.GetComponentInChildren<Text>();
        var c = rText.color;
        c.a = tCount2 / 255f;
        rText.color = c;

        // set buttons transparency
        var DoneButtonImages = submitButton.GetComponentsInChildren<Image>();
        for (int i = 1; i < DoneButtonImages.Length; ++i)
        {
            var clr = DoneButtonImages[i].color;
            clr.a = tCount / 255f;
            DoneButtonImages[i].color = clr;
        }
        var cancelButtonImages = cancelButton.GetComponentsInChildren<Image>();
        for (int i = 1; i < cancelButtonImages.Length; ++i)
        {
            var clr = cancelButtonImages[i].color;
            clr.a = tCount / 255f;
            cancelButtonImages[i].color = clr;
        }


        // MaskIn
        if (Entering)
        {
            if (MaskIn)
            {
                MaskCount += 5;
                if (MaskCount == 255)
                {
                    BG.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    MaskOut = true;
                    MaskIn = false;
                }
                MaskBG.color *= new Color(1, 1, 1, MaskCount / 255f / MaskBG.color.a);
            }
            if (MaskOut)
            {
                MaskCount -= 5;
                if (MaskCount == 5)
                {
                    MaskBG.gameObject.SetActive(false);
                    MaskOut = false;
                    Entering = false;
                }
                MaskBG.color *= new Color(1, 1, 1, MaskCount / 255f / MaskBG.color.a);
            }
        }

        if (Exiting)
        {
            if (MaskIn)
            {
                MaskCount += 5;
                if (MaskCount == 255)
                {
                    BG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, AppManager.DefaultRes.y);
                    MaskOut = true;
                    MaskIn = false;
                }
                MaskBG.color *= new Color(1, 1, 1, MaskCount / 255f / MaskBG.color.a);
            }
            if (MaskOut)
            {
                MaskCount -= 5;
                if (MaskCount == 5)
                {
                    MaskOut = false;
                    Exiting = false;
                    gameObject.SetActive(false);
                }
                MaskBG.color *= new Color(1, 1, 1, MaskCount / 255f / MaskBG.color.a);
            }
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
        // gameObject.SetActive(false);
        OnExiting();
    }

    public void cancelButtonClick()
    {
        AppManager.Prefabs[0].SetActive(true);
        Swipable.allow_swipe = true;
        // gameObject.SetActive(false);

        // fade out
        OnExiting();
    }

    private void OnEnable()
    {
        Entering = true;
        MaskIn = true;
        BG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, AppManager.DefaultRes.y);
        MaskBG.gameObject.SetActive(true);
        MaskBG.color *= new Color(1f, 1f, 1f, 5/255f);
        AppManager.MainTimeToBlack();
        MaskCount = 5;
    }

    void OnExiting()
    {
        Exiting = true;
        MaskIn = true;
        BG.GetComponent<RectTransform>().anchoredPosition = new Vector2();
        MaskBG.gameObject.SetActive(true);
        MaskBG.color *= new Color(1f, 1f, 1f, 5 / 255f);
        MaskCount = 5;
        AppManager.MainTimeToWhite();
    }
}
