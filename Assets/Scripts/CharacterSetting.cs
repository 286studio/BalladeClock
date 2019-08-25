using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSetting : MonoBehaviour
{
    public static CharacterSetting _ins;
    public GameObject Interactive;
    public GameObject CharacterSelectUI;
    public RectTransform CharacterSelectUI_RT;
    public GameObject CostumeSelectUI;
    public RectTransform CostumeSelectUI_RT;
    public Button[] Buttons; // 0 = bg, 1 = alarm, 2 = character change
    public Button[] CharacterButtons;
    public GameObject[] CostumeThumbs;
    [SerializeField] Sprite[] cxy_costumes;
    [SerializeField] Sprite[] tmm_costumes;
    [SerializeField] Sprite[] swimsuits_locked;
    Sprite[][] costumes;


    const int numCharacter = 2;
    [HideInInspector]
    public int curCharacter; // 0 = cxy, 1 = tmm
    public int curCostume;
    int curBG;
    [SerializeField] Vector3[] BG_offsets;

    // background related
    [Header("Background")]
    public Sprite[] BG_sprites;

    // voices
    [Header("SFX")]
    [SerializeField] AudioClip[] cxt_m1_touched;
    [SerializeField] AudioClip[] cxt_m2_touched;
    [SerializeField] AudioClip[] cxt_l_touched;
    [SerializeField] AudioClip[] cxt_r_touched;
    [SerializeField] AudioClip[] tmm_m1_touched;
    [SerializeField] AudioClip[] tmm_m2_touched;
    [SerializeField] AudioClip[] tmm_l_touched;
    [SerializeField] AudioClip[] tmm_r_touched;
    [SerializeField] AudioClip[][][] voiceClips;
    [HideInInspector]public AudioSource Voice;

    bool unlockSwimsuit;
    public void UnlockSwimsuit()
    {
        unlockSwimsuit = true;
        CostumeThumbs[4].GetComponent<Button>().onClick.AddListener(delegate { CostumeButtonPrtessed(4); });
    }

    private void Awake()
    {
        if (_ins == null) _ins = this; else Destroy(gameObject);

        Voice = gameObject.AddComponent<AudioSource>();
        var cxt_voices_touched = new AudioClip[4][] { cxt_m1_touched, cxt_m2_touched, cxt_l_touched, cxt_r_touched };
        var tmm_voices_touched = new AudioClip[4][] { tmm_m1_touched, tmm_m2_touched, tmm_l_touched, tmm_r_touched };
        voiceClips = new AudioClip[2][][] { cxt_voices_touched, tmm_voices_touched };
    }

    private void Start()
    {
        LoadSettingFromFile();
        Buttons[3].gameObject.SetActive(false);

        costumes = new Sprite[numCharacter][] { cxy_costumes, tmm_costumes };

        // new UI
        ButtonAdjuster();
        Buttons[0].onClick.AddListener(delegate {
            switchBG();
            SaveSettingToFile();
        });

        Buttons[1].onClick.AddListener(delegate {
            Swipable.external_swipe_left = true;
        });

        Buttons[2].onClick.AddListener(delegate {
            CharacterSelectUI.GetComponent<Animator>().SetBool("open", true);
            Swipable.allow_swipe = false;
            Buttons[3].gameObject.SetActive(true);
            Buttons[2].gameObject.SetActive(false);
            Buttons[1].gameObject.SetActive(false);
            Buttons[0].gameObject.SetActive(false);
        });
        Buttons[3].onClick.AddListener(delegate {
            CharacterSelectUI.GetComponent<Animator>().SetBool("open", false);
            CostumeSelectUI.GetComponent<Animator>().SetBool("open", false);
            Swipable.allow_swipe = true;
            Buttons[3].gameObject.SetActive(false);
            Buttons[2].gameObject.SetActive(true);
            Buttons[1].gameObject.SetActive(true);
            Buttons[0].gameObject.SetActive(true);
        });
        for (int i = 0; i < numCharacter; ++i)
        {
            int j = i;
            CharacterButtons[i].onClick.AddListener(delegate { CharacterButtonPressed(j); });
        }
        for (int i = 0; i < (unlockSwimsuit ? CostumeThumbs.Length : CostumeThumbs.Length - 1); ++i)
        {
            int k = i;
            CostumeThumbs[i].GetComponent<Button>().onClick.AddListener(delegate { CostumeButtonPrtessed(k); });
        }
    }

    bool voice_bool = false;
    private void Update()
    {
        if (voice_bool && !Voice.isPlaying)
        {
            Gamespace.Characters[curCharacter].GetComponentInChildren<CharacterSpriteManager>().show(-1, -1, 0);
            voice_bool = false;
        }

        // if (Input.GetKeyDown(KeyCode.U)) UnlockSwimsuit();
    }

    public void Character_touched()
    {
        if (Voice.isPlaying) return; // cooling down
        if (Notifications.alarming)
        {
            Notifications.EndAlarm();
            return;
        }
        var cur_pose_idx = Gamespace.Characters[curCharacter].GetComponentInChildren<CharacterSpriteManager>().show(-2, -1, 1);
        Voice.clip = voiceClips[curCharacter][cur_pose_idx][Random.Range(0, voiceClips[curCharacter][cur_pose_idx].Length)];
        Voice.Play();
        voice_bool = true;

        disappear();
    }

    public void switchBG(int idx = -1)
    {
        if (idx < 0) curBG = (curBG + 1) % BG_sprites.Length;
        else curBG = idx;
        Swipable.BG_EndPoint.x += BG_offsets[curBG].x;
        Swipable.BG_StartPoint.x += BG_offsets[curBG].x;
        Swipable.BG_EndPoint.y = BG_offsets[curBG].y;
        Swipable.BG_StartPoint.y = BG_offsets[curBG].y;
        Gamespace.Background.GetComponent<SpriteRenderer>().sprite = BG_sprites[curBG];
        Gamespace.Background.transform.position = Swipable.BG_StartPoint;
    }

    public void switchCharacter(int idx)
    {
        curCharacter = idx;
        Gamespace.Characters[curCharacter].SetActive(true);
        for (int i = 0; i < numCharacter; ++i) if (curCharacter != i) Gamespace.Characters[i].SetActive(false);
        SaveSettingToFile();
    }

    public void CharacterButtonPressed(int idx)
    {
        switchCharacter(idx);
        for (int i = 0; i < CostumeThumbs.Length; ++i)
        {
            CostumeThumbs[i].GetComponent<Image>().sprite = costumes[idx][i];
        }
        if (!unlockSwimsuit) CostumeThumbs[4].GetComponent<Image>().sprite = swimsuits_locked[idx];
        CostumeSelectUI.GetComponent<Animator>().SetBool("open", true);
    }

    public void CostumeButtonPrtessed(int idx)
    {
        curCostume = idx;
        Gamespace.Characters[curCharacter].GetComponent<CharacterSpriteManager>().show(-1, idx, -1);
        SaveSettingToFile();
        Character_touched();
    }

    void SaveSettingToFile()
    {
        _SettingUserData ud = new _SettingUserData();
        ud.character = curCharacter;
        ud.costume = curCostume;
        ud.bg = curBG;
        SaveSystem.SaveSettingUserData(ud);
    }

    void LoadSettingFromFile()
    {
        _SettingUserData ud = SaveSystem.LoadSettingUserData();
        if (ud == null)
        {
            ud = new _SettingUserData();
            ud.character = 0;
            ud.costume = 0;
            ud.bg = 6;
        }
        // apply changes
        curCostume = ud.costume;
        switchBG(ud.bg);
        switchCharacter(ud.character);
        Gamespace.Characters[curCharacter].GetComponent<CharacterSpriteManager>().show(-1, curCostume, -1);
    }

    public void disappear()
    {
        CharacterSelectUI.GetComponent<Animator>().SetBool("open", false);
        CostumeSelectUI.GetComponent<Animator>().SetBool("open", false);
        Swipable.allow_swipe = true;
        Buttons[2].gameObject.SetActive(true);
        Buttons[1].gameObject.SetActive(true);
        Buttons[0].gameObject.SetActive(true);
    }

    void ButtonAdjuster()
    {
        float spacer = 120f * AppManager.AdjustedWidth / AppManager.DefaultRes.x;
        var rt = Buttons[0].GetComponent<RectTransform>();
        var pos = rt.anchoredPosition;
        pos.x = AppManager.AdjustedWidth / 2 - 2 * spacer - 150;
        pos.y = -(AppManager.DefaultRes.y / 2 - spacer);
        rt.anchoredPosition = pos;

        rt = Buttons[1].GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
        pos.x = -(AppManager.AdjustedWidth / 2 - spacer);
        pos.y = -(AppManager.DefaultRes.y / 2 - spacer);
        rt.anchoredPosition = pos;

        rt = Buttons[2].GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
        pos.x = AppManager.AdjustedWidth / 2 - spacer;
        pos.y = -(AppManager.DefaultRes.y / 2 - spacer);
        rt.anchoredPosition = pos;

        CharacterSelectUI_RT.localScale *= (AppManager.AdjustedWidth / AppManager.DefaultRes.x);
        pos = CharacterSelectUI_RT.anchoredPosition;
        pos.y += AppManager.DefaultRes.y * (AppManager.AdjustedWidth / AppManager.DefaultRes.x - 1) / 2;
        CharacterSelectUI_RT.anchoredPosition = pos;
        CostumeSelectUI_RT.localScale *= (AppManager.AdjustedWidth / AppManager.DefaultRes.x);
        pos = CostumeSelectUI_RT.anchoredPosition;
        pos.y += AppManager.DefaultRes.y * (AppManager.AdjustedWidth / AppManager.DefaultRes.x - 1) / 2;
        CostumeSelectUI_RT.anchoredPosition = pos;
    }
}
