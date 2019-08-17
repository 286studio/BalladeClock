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
    Sprite[][] costumes;


    const int numCharacter = 2;
    [HideInInspector]
    public int curCharacter; // 0 = cxy, 1 = tmm
    int curBG;
    [SerializeField] Vector3[] BG_offsets;

    // background related
    [Header("Background")]
    public Sprite[] BG_sprites;

    //CharacterInitializtion
    public Character cxy = new Character();
    public Character tmm = new Character();
    Dictionary<string, Character> Choosing;
    [Header("SFX")]
    public AudioClip[] bgm = new AudioClip[2];
    AudioSource BGM;
    public AudioSource Voice;
    public int cur_bgm = 0;

    private void Awake()
    {
        if (_ins == null) _ins = this; else Destroy(gameObject);
        BGM = gameObject.AddComponent<AudioSource>();
        Voice = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        LoadSettingFromFile();

        CharacterInitialization();
        BGM.clip = bgm[cur_bgm];
        BGM.loop = true;
        BGM.volume = 0.5f;
        // BGM.Play();
        Voice.clip = curCharacter == 0 ? cxy.Voice_lib[0] : tmm.Voice_lib[0];
        Voice.loop = false;

        costumes = new Sprite[numCharacter][] { cxy_costumes, tmm_costumes };

        // new UI
        ButtonAdjuster();
        Buttons[0].onClick.AddListener(delegate {
            curBG = (curBG + 1) % BG_sprites.Length;
            Gamespace.Background.GetComponent<SpriteRenderer>().sprite = BG_sprites[curBG];
        });

        Buttons[1].onClick.AddListener(delegate {
            Swipable.external_swipe_left = true;
        });

        Buttons[2].onClick.AddListener(delegate {
            CharacterSelectUI.GetComponent<Animator>().SetBool("open", true);
            Buttons[2].gameObject.SetActive(false);
            Buttons[1].gameObject.SetActive(false);
	    });

        for (int i = 0; i < numCharacter; ++i)
        {
            int j = i;
            CharacterButtons[i].onClick.AddListener(delegate { CharacterButtonPressed(j); });
        }
        for (int i = 0; i < CostumeThumbs.Length; ++i)
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
    }

    public void CharacterInitialization()
    {
        cxy.name = "cxy";
        tmm.name = "tmm";
        for (int i = 0; i < 10; ++i)
        {
            cxy.Voice_lib.Add(i, Resources.Load<AudioClip>("Characters/Voices/cxy/x" + (i + 1).ToString()));
            tmm.Voice_lib.Add(i, Resources.Load<AudioClip>("Characters/Voices/tmm/m" + (i + 1).ToString()));
        }
    }

    public void Character_touched()
    {
        if (Voice.isPlaying) return; // cooling down
        Voice.clip = curCharacter == 0 ? cxy.Voice_lib[Random.Range(0, cxy.Voice_lib.Count - 1)] : tmm.Voice_lib[Random.Range(0, tmm.Voice_lib.Count - 1)];
        Voice.Play();
        voice_bool = true;
        Gamespace.Characters[curCharacter].GetComponentInChildren<CharacterSpriteManager>().show(-2, -1, 1);

        disappear();
    }

    public void switchCharacter(int idx)
    {
        curCharacter = idx;
        Gamespace.Characters[curCharacter].SetActive(true);
        for (int i = 0; i < numCharacter; ++i) if (curCharacter != i) Gamespace.Characters[i].SetActive(false);
    }

    public void CharacterButtonPressed(int idx)
    {
        switchCharacter(idx);
        for (int i = 0; i < CostumeThumbs.Length; ++i)
        {
            CostumeThumbs[i].GetComponent<Image>().sprite = costumes[idx][i];
        }
        CostumeSelectUI.GetComponent<Animator>().SetBool("open", true);
    }

    public void CostumeButtonPrtessed(int idx)
    {
        Gamespace.Characters[curCharacter].GetComponent<CharacterSpriteManager>().show(-1, idx, -1);
        disappear();
    }

    void SaveSettingToFile()
    {
        _SettingUserData ud = new _SettingUserData();
        ud.character = curCharacter;
        ud.bg = curBG;
        SaveSystem.SaveSettingUserData(ud);
    }

    void LoadSettingFromFile()
    {
        _SettingUserData ud = SaveSystem.LoadSettingUserData();
        if (ud == null)
        {
            Debug.Log("No file");
            ud = new _SettingUserData();
            ud.character = 0;
            ud.voice = new int[] { 0, 0 }; // no use
            ud.anim = new int[] { 0, 0 }; // no use
            ud.bg = 0;
            ud.bgm = 0; // no use
        }
        // apply changes
        switchCharacter(ud.character);
        curBG = ud.bg;
        Gamespace.Background.GetComponent<SpriteRenderer>().sprite = BG_sprites[curBG];
    }

    public void disappear()
    {
        CharacterSelectUI.GetComponent<Animator>().SetBool("open", false);
        CostumeSelectUI.GetComponent<Animator>().SetBool("open", false);
        Buttons[2].gameObject.SetActive(true);
        Buttons[1].gameObject.SetActive(true);
    }

    void ButtonAdjuster()
    {
        float spacer = 120f * AppManager.AdjustedWidth / AppManager.DefaultRes.x;
        var rt = Buttons[0].GetComponent<RectTransform>();
        var pos = rt.anchoredPosition;
        pos.x = -(AppManager.AdjustedWidth / 2 - spacer);
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
