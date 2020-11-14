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
    public Button submitButton;
    public Button cancelButton;
    public GameObject BG;
    public GameObject AllText;
    public Image MaskBG;
    bool Entering;
    bool Exiting;
    bool MaskIn = false;
    bool MaskOut = false;
    int MaskCount = 0;

    void Start()
    {
        submitButton.onClick.AddListener(submitButtonClick);
        cancelButton.onClick.AddListener(deleteButtonClick);

        var scaleMul = AppManager.AdjustedWidth / AppManager.DefaultRes.x;
        GetComponent<RectTransform>().localScale *= scaleMul;
        if (AppManager.isIPad)
        {
            (AllText.transform as RectTransform).localScale /= scaleMul;
            (AllText.transform as RectTransform).localScale *= 1.25f;
        }

        Entering = true;
        MaskIn = true;
        BG.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, AppManager.DefaultRes.y);
        MaskBG.gameObject.SetActive(true);
        MaskBG.color *= new Color(1f, 1f, 1f, 5 / 255f);
        AppManager.MainTimeToBlack();
        MaskCount = 5;
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
        int rdcc2 = Ringer_dropdown.transform.childCount;
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
                    Destroy(gameObject);
                }
                MaskBG.color *= new Color(1, 1, 1, MaskCount / 255f / MaskBG.color.a);
            }
        }
    }

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
        Id = Notifications.AddAlarm(Hour_dropdown.getValue(), Minute_dropdown.getValue(), AMPM_dropdown.getValue(), Repeat_dropdown.value == 1, Label_input.text, Ringer_dropdown.value);
        EditingAlarm.Id = Id;
        AlarmList.SaveAlarmListToFile();
        Swipable.allow_swipe = true;
        // Destroy(gameObject);
        OnExiting();
    }

    public void deleteButtonClick()
    {
        AppManager.Prefabs[0].gameObject.SetActive(true);
        Swipable.allow_swipe = true;
        EditingAlarm.X_button_click();
        // Destroy(gameObject);
        OnExiting();
    }

    public void returnButtonClick()
    {
        AppManager.Prefabs[0].gameObject.SetActive(true);
        Swipable.allow_swipe = true;
        OnExiting();
        // Destroy(gameObject);
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
