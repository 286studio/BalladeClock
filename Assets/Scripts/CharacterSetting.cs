using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CharacterSetting : MonoBehaviour
{
    public static CharacterSetting _ins;

    // Reference to Setting button and Setting page
    [Header("Buttons")]
    public Button SettingButton;
    public Button AcceptButton;
    public Button CancelButton;
    public GameObject SettingPage;
    public GameObject Interactive;

    // background related
    [Header("Background")]
    public Sprite[] BG_sprites;
    public TMP_Dropdown BG_dropdown;

    //CharacterInitializtion
    public Character cxy = new Character();
    public Character tmm = new Character();
    Dictionary<string, Character> Choosing;
    [Header("SFX")]
    public AudioClip[] bgm = new AudioClip[2];
    AudioSource BGM;
    public AudioSource Voice;
    public int cur_bgm = 0;
    public TMP_Dropdown BGM_Dropdown;
    public TMP_Dropdown[] Voice_Dropdown;

    [Header("Profile")]
    public TMP_Dropdown Profile_Dropdown;
    public Image Profile_Image;
    public Sprite[] Profile_Sprites;

    [Header("Animation")]
    public TMP_Dropdown[] Anim_Dropdown;

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
        BGM.Play();
        Voice.clip = Profile_Dropdown.value == 0 ? cxy.Voice_lib[Voice_Dropdown[0].value] : tmm.Voice_lib[Voice_Dropdown[1].value];
        Voice.loop = false;

        print((Profile_Dropdown.value == 0 ? "x" : "m") + (Voice_Dropdown[Profile_Dropdown.value].value + 1).ToString() + ".wav");
        // page settings
        // setting page hidden at start
        // setting button shown at start
        SettingButton.gameObject.SetActive(true);
        SettingPage.gameObject.SetActive(false);

        // link setting buttons to functions
        SettingButton.onClick.AddListener(delegate {
            SettingPage.gameObject.SetActive(true);
            SettingButton.gameObject.SetActive(false);
        });

        AcceptButton.onClick.AddListener(delegate {
            // apply changes
            // background change
            Gamespace.Background.GetComponent<SpriteRenderer>().sprite = BG_sprites[BG_dropdown.value];
            if (Profile_Dropdown.value == 0)
            {
                Gamespace.Characters[0].SetActive(true);
                Gamespace.Characters[1].SetActive(false);
            }
            // profile change
            else if (Profile_Dropdown.value == 1)
            {
                Gamespace.Characters[0].SetActive(false);
                Gamespace.Characters[1].SetActive(true);
            }
            // voice change
            Voice.clip = Profile_Dropdown.value == 0 ? cxy.Voice_lib[Voice_Dropdown[0].value] : tmm.Voice_lib[Voice_Dropdown[1].value];

            SaveSettingToFile();
            SettingButton.gameObject.SetActive(true);
            SettingPage.gameObject.SetActive(false);
        });

        CancelButton.onClick.AddListener(delegate {
            SettingButton.gameObject.SetActive(true);
            SettingPage.gameObject.SetActive(false);
        });

        // when changing profile, change the color of the name
        Profile_Dropdown.onValueChanged.AddListener(delegate { onCharacterChange(); });

        // when changing voice, play it once
        Voice_Dropdown[0].onValueChanged.AddListener(delegate
        {
            var audsrc = gameObject.AddComponent<AudioSource>();
            audsrc.clip = cxy.Voice_lib[Voice_Dropdown[0].value];
            audsrc.Play();
            Destroy(audsrc, audsrc.clip.length);
        });
        Voice_Dropdown[1].onValueChanged.AddListener(delegate
        {
            var audsrc = gameObject.AddComponent<AudioSource>();
            audsrc.clip = tmm.Voice_lib[Voice_Dropdown[1].value];
            audsrc.Play();
            Destroy(audsrc, audsrc.clip.length);
        });
    }

    private void Update()
    {
        BGM_update();
        if (!Voice.isPlaying)
        {
            var animator = Gamespace.Characters[Profile_Dropdown.value].GetComponentInChildren<Animator>();
            if (animator.gameObject.activeInHierarchy) animator.SetBool("isTouched", false);
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

    public void Voice_update()
    {

    }
    public void Character_touched()
    {
        if (Voice.isPlaying) return; // cooling down
        Voice.clip = Profile_Dropdown.value == 0 ? cxy.Voice_lib[Random.Range(0, cxy.Voice_lib.Count - 1)] : tmm.Voice_lib[Random.Range(0, tmm.Voice_lib.Count - 1)];
        Voice.Play();
        Gamespace.Characters[Profile_Dropdown.value].GetComponentInChildren<Animator>().SetBool("isTouched", true);
    }

    public void BGM_update()
    {
        if(cur_bgm!= BGM_Dropdown.value)
        {
            cur_bgm = BGM_Dropdown.value;
            BGM.Stop();
            BGM.clip = bgm[cur_bgm];
            BGM.Play();
        }
    }

    public void switchCharacter(int idx)
    {
        Profile_Dropdown.value = idx;
        if (Profile_Dropdown.value == 0)
        {
            Gamespace.Characters[0].SetActive(true);
            Gamespace.Characters[1].SetActive(false);
        }
        // profile change
        else if (Profile_Dropdown.value == 1)
        {
            Gamespace.Characters[0].SetActive(false);
            Gamespace.Characters[1].SetActive(true);
        }
    }

    void onCharacterChange()
    {
        Profile_Image.sprite = Profile_Sprites[Profile_Dropdown.value];
        if (Profile_Dropdown.value == 0)
            Profile_Dropdown.GetComponent<Image>().color = new Color(1, 66f / 255f, 66f / 255f, 148f / 255f);
        else if (Profile_Dropdown.value == 1)
            Profile_Dropdown.GetComponent<Image>().color = new Color(66f / 255f, 66f / 255f, 1, 148f / 255f);


        Voice_Dropdown[Profile_Dropdown.value].gameObject.SetActive(true);
        Anim_Dropdown[Profile_Dropdown.value].gameObject.SetActive(true);
        Voice_Dropdown[Profile_Dropdown.value == 0 ? 1 : 0].gameObject.SetActive(false);
        Anim_Dropdown[Profile_Dropdown.value == 0 ? 1 : 0].gameObject.SetActive(false);
    }

    void SaveSettingToFile()
    {
        _SettingUserData ud = new _SettingUserData();
        ud.character = Profile_Dropdown.value;

        ud.voice = new int[Voice_Dropdown.Length];
        for (int i = 0; i < Voice_Dropdown.Length; ++i) ud.voice[i] = Voice_Dropdown[i].value;

        ud.anim = new int[Anim_Dropdown.Length];
        for (int i = 0; i < Anim_Dropdown.Length; ++i) ud.anim[i] = Anim_Dropdown[i].value;

        ud.bg = BG_dropdown.value;
        ud.bgm = BGM_Dropdown.value;
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
            ud.voice = new int[] { 0, 0 };
            ud.anim = new int[] { 0, 0 };
            ud.bg = 0;
            ud.bgm = 0;
        }
        // apply changes
        Profile_Dropdown.value = ud.character;
        onCharacterChange();
        for (int i = 0; i < Voice_Dropdown.Length; ++i) Voice_Dropdown[i].value = ud.voice[i];
        for (int i = 0; i < Anim_Dropdown.Length; ++i) Anim_Dropdown[i].value = ud.anim[i];
        BG_dropdown.value = ud.bg;
        BGM_Dropdown.value = ud.bgm;

        Gamespace.Background.GetComponent<SpriteRenderer>().sprite = BG_sprites[BG_dropdown.value];
        if (Profile_Dropdown.value == 0)
        {
            Gamespace.Characters[0].SetActive(true);
            Gamespace.Characters[1].SetActive(false);
        }
        // profile change
        else if (Profile_Dropdown.value == 1)
        {
            Gamespace.Characters[0].SetActive(false);
            Gamespace.Characters[1].SetActive(true);
        }
    }
}
