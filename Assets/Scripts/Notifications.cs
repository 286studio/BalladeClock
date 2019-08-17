using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.iOS;
using UnityEngine.UI;

public class Notifications : MonoBehaviour
{
    public GameObject testObj;
    public GameObject testObj2;
    public static string prefix = "286studio_alarm_";
    public static int Id;
    public AudioSource SoundPlayer;

    string curRespondNotificationIdentifier;

    static string[] bodyStrings = {
        "太阳晒屁股了！",
        "到点了到点了还不起来！",
        "快迟到啦！",
        "懒猪猪快起来！",
        "早餐给你做好了，快起来吃把。",
        "再不起来我就把你橱柜里点小人都砸了。",
    };

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        iOSNotificationCenter.OnNotificationReceived += OnNotificationReceived;
        curRespondNotificationIdentifier = "";
        SoundPlayer = CharacterSetting._ins.Voice;
    }

    private void Update()
    {
        var ln = iOSNotificationCenter.GetLastRespondedNotification();
        if (ln != null)
        {
            if (ln.Identifier != curRespondNotificationIdentifier)
            {
                // enter app via notification
                curRespondNotificationIdentifier = ln.Identifier;

                // continue to play the notification sound
                iOSNotificationCalendarTrigger t = (iOSNotificationCalendarTrigger)ln.Trigger;
                Debug.Log(System.DateTime.Now.Hour + "==" + t.Hour + "?");
                Debug.Log(System.DateTime.Now.Minute + "==" + t.Minute + "?");
                if (System.DateTime.Now.Hour == t.Hour && System.DateTime.Now.Minute == t.Minute)
                {
                    float curSec = System.DateTime.Now.Second + System.DateTime.Now.Millisecond / 1000f;
                    Debug.Log("Second: " + curSec);
                    Debug.Log("Sound Length: " + SoundPlayer.clip.length);
                    if (curSec < SoundPlayer.clip.length)
                    {
                        SoundPlayer.time = curSec;
                        SoundPlayer.Play();
                    }
                }

                // go to character's page
                CharacterSetting._ins.switchCharacter(ln.CategoryIdentifier == "cxy" ? 0 : 1);
                iOSNotificationCenter.RemoveAllDeliveredNotifications();

            }
        }
    }

    void OnNotificationReceived(iOSNotification ln)
    {
        //var testIns = Instantiate(testObj, GameObject.Find("Canvas").transform);
        //testIns.GetComponentInChildren<Text>().text = ln.Identifier + "Received";

        // play character voice
        SoundPlayer.Play();
    }

    public static int AddAlarm(int hr, int min, int ampm, bool repeat, string label)
    {
        int _hour = ampm == 0 ? hr : hr + 12;
        if (ampm == 0 && hr == 12) _hour = 0; // 12am
        if (ampm == 1 && hr == 12) _hour = 12; // 12pm

        var timeTrigger = new iOSNotificationCalendarTrigger()
        {
            Hour = _hour,
            Minute = min,
            Repeats = repeat
        };
        var notification = new iOSNotification()
        {
            // You can optionally specify a custom Identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = prefix + Id.ToString(),
            Title = CharacterSetting._ins.curCharacter == 0 ? "陈星瑶" : "谭明美",
            Subtitle = "你的闹钟【" + (label.Length > 0 ? label : "Alarm") + "】到点了！",
            Body = bodyStrings[Random.Range(0, bodyStrings.Length - 1)],
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.None,
            CategoryIdentifier = CharacterSetting._ins.curCharacter == 0 ? "cxy" : "tmm",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
            Data = "" // TODO
        };
        iOSNotificationCenter.ScheduleNotification(notification);
        return Id++;
    }

    public static void RemoveAlarm(int _Id)
    {
        iOSNotificationCenter.RemoveScheduledNotification(prefix + _Id.ToString());
    }

    public static int NIdentifierToId(string Identifier)
    {
        return int.Parse(Identifier.Substring(prefix.Length));
    }
}
