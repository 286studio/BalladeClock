using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSetting : MonoBehaviour
{
    public static CharacterSetting _ins;
    public GameObject Interactive;
    public GameObject CharacterSelectUI;
    public Button[] Buttons; // 0 = bg, 1 = alarm, 2 = character change

    const int numCharacter = 2;
    public int curCharacter; // 0 = cxy, 1 = tmm
    int curBG;

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

        // new UI
        Buttons[2].onClick.AddListener(delegate {
            CharacterSelectUI.GetComponent<Animator>().SetBool("open", true);
            Buttons[2].gameObject.SetActive(false);
            Buttons[1].gameObject.SetActive(false);
	    });
    }

    bool voice_bool = false;
    private void Update()
    {
        if (voice_bool && !Voice.isPlaying)
        {
            Gamespace.Characters[curCharacter].GetComponentInChildren<CharacterSpriteManager>().show(-1, -1, false);
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

    public void Voice_update()
    {

    }
    public void Character_touched()
    {
        if (Voice.isPlaying) return; // cooling down
        Voice.clip = curCharacter == 0 ? cxy.Voice_lib[Random.Range(0, cxy.Voice_lib.Count - 1)] : tmm.Voice_lib[Random.Range(0, tmm.Voice_lib.Count - 1)];
        Voice.Play();
        voice_bool = true;
        Gamespace.Characters[curCharacter].GetComponentInChildren<CharacterSpriteManager>().show(-2, -1, true);


        CharacterSelectUI.GetComponent<Animator>().SetBool("open", false);
        Buttons[2].gameObject.SetActive(true);
        Buttons[1].gameObject.SetActive(true);
    }

    public void switchCharacter(int idx)
    {
        curCharacter = idx;
        Gamespace.Characters[curCharacter].SetActive(true);
        for (int i = 0; i < numCharacter; ++i) if (curCharacter != i) Gamespace.Characters[i].SetActive(false);
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
        Buttons[2].gameObject.SetActive(true);
        Buttons[1].gameObject.SetActive(true);
    }
}
