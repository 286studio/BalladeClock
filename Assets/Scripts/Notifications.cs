using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.iOS;
using UnityEngine.UI;

public class Notifications : MonoBehaviour
{
    public static string prefix = "286studio_alarm_";
    public static int Id;
    public static AudioSource SoundPlayer;
    [SerializeField] AudioClip[] Ringers;

    static string[] characterNames =
    {
        "陈星瑶",
        "谈明美"
    };

    static string[] soundFileNames =
    {
        "cxy_alarm1.wav",
        "cxy_alarm2.wav",
        "tmm_alarm1.wav"
    };

    static string[] CategoryIdentifiers =
    {
        "cxy",
        "tmm"
    };

    string curRespondNotificationIdentifier;

    static string[] bodyStrings = {
        "太阳晒屁股了！",
        "到点了到点了还不起来！",
        "快迟到啦！",
        "懒猪猪快起来！",
        "早餐给你做好了，快起来吃把。",
        "再不起来我就把你橱柜里点小人都砸了。",
    };

    bool alarming;
    int alarm_hr = -1;
    int alarm_min = -1;
    float vibrate_interval = 0.5f;
    float vibrate_count;

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        iOSNotificationCenter.OnNotificationReceived += OnNotificationReceived;
        curRespondNotificationIdentifier = "";
        SoundPlayer = gameObject.AddComponent<AudioSource>();
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
                OpenAppViaNotification(ln);
            }
        }

        if (alarming) AlarmUpdate();
        if (System.DateTime.Now.Hour != alarm_hr || System.DateTime.Now.Minute != alarm_min) EndAlarm();
    }

    void StartAlarm(int hr, int min)
    {
        alarming = true;
        alarm_hr = hr;
        alarm_min = min;
        vibrate_count = Time.time;
    }

    void AlarmUpdate()
    {
        if (!SoundPlayer.isPlaying) SoundPlayer.Play();
        if (Time.time - vibrate_count > vibrate_interval)
        {
            Handheld.Vibrate();
            vibrate_count = Time.time;
        }
    }

    void EndAlarm()
    {
        alarming = false;
        alarm_hr = -1;
        alarm_min = -1;
        if (SoundPlayer.isPlaying) SoundPlayer.Stop();
    }

    // This function execautes once when user open the app via notificaiton
    void OpenAppViaNotification(iOSNotification ln)
    {
        // continue to play the notification sound
        iOSNotificationCalendarTrigger t = (iOSNotificationCalendarTrigger)ln.Trigger;
        // Debug.Log(System.DateTime.Now.Hour + "==" + t.Hour + "?");
        // Debug.Log(System.DateTime.Now.Minute + "==" + t.Minute + "?");
        if (System.DateTime.Now.Hour == t.Hour && System.DateTime.Now.Minute == t.Minute)
        {
            getRingerClip(ln.Data);
            float curSec = System.DateTime.Now.Second + System.DateTime.Now.Millisecond / 1000f;
            if (curSec < SoundPlayer.clip.length)
            {
                SoundPlayer.time = curSec;
                SoundPlayer.Play();
                StartAlarm((int)t.Hour, (int)t.Minute);
            }
        }

        // go to character's page
        CharacterSetting._ins.switchCharacter(ln.CategoryIdentifier == "cxy" ? 0 : 1);
        Swipable.external_swipe_right = true;
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
    }

    // This function execautes once when user receive notificaiton when app is already open
    void OnNotificationReceived(iOSNotification ln)
    {

        // play character voice
        getRingerClip(ln.Data);
        SoundPlayer.Play();
        StartAlarm(System.DateTime.Now.Hour, System.DateTime.Now.Minute);
        CharacterSetting._ins.switchCharacter(ln.CategoryIdentifier == "cxy" ? 0 : 1);
        Swipable.external_swipe_right = true;
        Handheld.Vibrate();
    }

    void getRingerClip(string clipName)
    {
        for (int i = 0; i < soundFileNames.Length; ++i)
        {
            if (soundFileNames[i] == clipName)
            {
                SoundPlayer.clip = Ringers[i];
                break;
            }
        }
    }

    public static int AddAlarm(int hr, int min, int ampm, bool repeat, string label, int ringer)
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
            Title = characterNames[CharacterSetting._ins.curCharacter],
            Subtitle = "你的闹钟【" + (label.Length > 0 ? label : "Alarm") + "】到点了！",
            Body = bodyStrings[Random.Range(0, bodyStrings.Length - 1)],
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.None,
            CategoryIdentifier = CategoryIdentifiers[CharacterSetting._ins.curCharacter],
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
            Data = soundFileNames[ringer]
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
