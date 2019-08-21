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
        "cxy",
        "tmm"
    };

    string curRespondNotificationIdentifier;

    static string[] bodyStrings = {
        "不忘列车经历的风景\r\n追逐前方的光影\r\n直到终点将来临\r\n看那时漫天落英",
        "啊—————— 啊——————",
        "你是不是天天都在看什么很黄很暴力都东西啊",
    };

    // alarm variables
    [SerializeField] GameObject _alarm_pfx;
    static GameObject alarm_pfx;
    static GameObject alarm_pfx_ins;
    public static bool alarming;
    static int alarm_hr;
    static int alarm_min;
    const float vibrate_interval = 0.5f;
    static float vibrate_count;

    private void Awake()
    {
        InitAlarm();
        alarm_pfx = _alarm_pfx;
    }

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

    static void InitAlarm()
    {
        alarming = false;
        alarm_hr = -1;
        alarm_min = -1;
        vibrate_count = -1;
        if (alarm_pfx_ins != null)
        {
            Destroy(alarm_pfx_ins);
            alarm_pfx_ins = null;
        }
    }

    public static void StartAlarm(int hr, int min)
    {
        alarming = true;
        alarm_hr = hr;
        alarm_min = min;
        vibrate_count = Time.time;
        alarm_pfx_ins = Instantiate(alarm_pfx, GameObject.Find("MainUI_Time").transform);
        alarm_pfx_ins.transform.localScale *= 2;
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

    public static void EndAlarm()
    {
        if (!alarming) return;
        alarming = false;
        alarm_hr = -1;
        alarm_min = -1;
        vibrate_count = -1;
        if (SoundPlayer.isPlaying) SoundPlayer.Stop();
        Destroy(alarm_pfx_ins);
        alarm_pfx_ins = null;

        Gamespace.Characters[CharacterSetting._ins.curCharacter].GetComponentInChildren<CharacterSpriteManager>().show(-1, -1, 0);
    }

    // This function execautes once when user open the app via notificaiton
    void OpenAppViaNotification(iOSNotification ln)
    {
        // continue to play the notification sound
        iOSNotificationCalendarTrigger t = (iOSNotificationCalendarTrigger)ln.Trigger;
        // Debug.Log(System.DateTime.Now.Hour + "==" + t.Hour + "?");
        // Debug.Log(System.DateTime.Now.Minute + "==" + t.Minute + "?");
        int r = getRinger(ln.Data);
        if (System.DateTime.Now.Hour == t.Hour && System.DateTime.Now.Minute == t.Minute)
        {
            SoundPlayer.clip = Ringers[r];
            float curSec = System.DateTime.Now.Second + System.DateTime.Now.Millisecond / 1000f;
            if (curSec < SoundPlayer.clip.length)
            {
                SoundPlayer.time = curSec;
                SoundPlayer.Play();
            }
            StartAlarm((int)t.Hour, (int)t.Minute);

            // go to character's page
            int curCharacter = ln.CategoryIdentifier == "cxy" ? 0 : 1;
            CharacterSetting._ins.switchCharacter(curCharacter);
            Gamespace.Characters[curCharacter].GetComponentInChildren<CharacterSpriteManager>().showAlarm(r);
            Swipable.external_swipe_right = true;
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
        }
    }

    // This function execautes once when user receive notificaiton when app is already open
    void OnNotificationReceived(iOSNotification ln)
    {

        // play character voice
        int r = getRinger(ln.Data);
        SoundPlayer.clip = Ringers[r];
        SoundPlayer.Play();
        StartAlarm(System.DateTime.Now.Hour, System.DateTime.Now.Minute);
        int curCharacter = ln.CategoryIdentifier == "cxy" ? 0 : 1;
        CharacterSetting._ins.switchCharacter(curCharacter);
        Gamespace.Characters[curCharacter].GetComponentInChildren<CharacterSpriteManager>().showAlarm(r);
        Swipable.external_swipe_right = true;
        Handheld.Vibrate();
    }

    int getRinger(string clipName)
    {
        for (int i = 0; i < soundFileNames.Length; ++i) if (soundFileNames[i] == clipName)return i;
        return 0;
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
            Title = characterNames[ringer],
            Subtitle = "你的闹铃【" + (label.Length > 0 ? label : "未命名闹铃") + "】到点了！",
            Body = bodyStrings[ringer],
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.None,
            CategoryIdentifier = CategoryIdentifiers[ringer],
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
