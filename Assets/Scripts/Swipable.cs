using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Moving { None, Left, Right }

public class Swipable : MonoBehaviour
{
    public GameObject Left;
    public GameObject Right;
    public GameObject GameSpace;
    public GameObject Touch_pfx;

    RectTransform rt;
    public Moving moving;
    public Moving mv_pos;

    float startTime = -1;
    bool click = false;
    public float speed = 3000F;


    float EndPoint;
    Vector3 GS_EndPoint;
    static public Vector3 BG_EndPoint;
    static public Vector3 BG_StartPoint;

    static public Vector3 _BG_EndPoint;
    static public Vector3 _BG_StartPoint;

    static public bool external_swipe_left;
    static public bool external_swipe_right;
    static public bool allow_swipe;


    private void Awake()
    {
        var p = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f));
        _BG_EndPoint = new Vector3(-(10.24f - p.x), 1.82f, 2f);
        _BG_StartPoint = new Vector3(0, 1.82f, 2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        EndPoint = -AppManager.AdjustedWidth;
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(-Screen.width, 0, 0));
        pos.y = 0;
        pos.z = 0;
        GS_EndPoint = pos;

        rt = GetComponent<RectTransform>();
        moving = Moving.None;
        mv_pos = Moving.Right;
        Right.GetComponent<RectTransform>().anchoredPosition = new Vector3(AppManager.AdjustedWidth, 0, 0);
        Left.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        external_swipe_left = false;
        external_swipe_right = false;
        allow_swipe = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving == Moving.None)
        {
            if (swipeRight() && mv_pos == Moving.Left)
            {
                moving = Moving.Right;
                startTime = Time.time;
                AppManager.Prefabs[0].GetComponent<AlarmList>().Return.gameObject.SetActive(false);
            }

            if (swipeLeft() && mv_pos == Moving.Right)
            {
                moving = Moving.Left;
                startTime = Time.time;
                AppManager.Prefabs[0].GetComponent<AlarmList>().Return.gameObject.SetActive(false);
                CharacterSetting._ins.disappear();
            }
        }

        // Touch particle effect
#if UNITY_EDITOR
        // mouse simulation
        if (Input.GetMouseButtonDown(0))
        {
            var pfx = Instantiate(Touch_pfx, GameObject.Find(mv_pos == Moving.Left ? "Swipable_right" : "Swipable_left").transform);
            pfx.transform.localScale = 120 * Vector3.one;
            pfx.GetComponent<RectTransform>().anchoredPosition = (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2)) * AppManager.DefaultRes.y / Screen.height;
        }
#endif
        // touches
        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                var pfx = Instantiate(Touch_pfx, GameObject.Find(mv_pos == Moving.Left ? "Swipable_right" : "Swipable_left").transform);
                pfx.transform.localScale = 120 * Vector3.one; // 放大点击特效 // 原来是200
                pfx.GetComponent<RectTransform>().anchoredPosition = (touch.position - new Vector2(Screen.width / 2, Screen.height / 2)) * AppManager.DefaultRes.y / Screen.height;
            }
        }
    }

    private void FixedUpdate()
    {
        if (startTime > 0)
        {
            if (moving==Moving.Left)
            {
                moveLeft();
            }

            if (moving==Moving.Right)
            {
                moveRight();
            }
        }
    }

    bool swipeLeft()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A)) return true;
#endif
        if (external_swipe_left)
        {
            external_swipe_left = false;
            return true;
        }
        if (!allow_swipe) return false;
        foreach (Touch touch in Input.touches)
        {
            return touch.deltaPosition.x / touch.deltaTime < -2000;
        }
        return false;
    }

    bool swipeRight()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S)) return true;
#endif
        if (external_swipe_right)
        {
            external_swipe_right = false;
            return true;
        }
        if (!allow_swipe) return false;
        foreach (Touch touch in Input.touches)
        {
            return touch.deltaPosition.x / touch.deltaTime > 2000;
        }
        return false;
    }

    public void moveLeft()
    {
        // deal with canvas space
        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / AppManager.AdjustedWidth;
        rt.anchoredPosition = Vector3.Lerp(Vector3.zero, new Vector3(EndPoint, 0, 0), fracJourney);


        if (System.Math.Abs(EndPoint - rt.anchoredPosition.x) < double.Epsilon)
        {
            startTime = -1;
            moving = Moving.None;
            mv_pos = Moving.Left;
            AppManager.Prefabs[0].GetComponent<AlarmList>().Return.gameObject.SetActive(true);
        }

        // deal with game space
        GameSpace.transform.position = Vector3.Lerp(Vector3.zero, GS_EndPoint, fracJourney);
        Gamespace.Background.transform.position = Vector3.Lerp(BG_StartPoint, BG_EndPoint, fracJourney);
    }

    void moveRight()
    {
        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / AppManager.AdjustedWidth;

        rt.anchoredPosition = Vector3.Lerp(new Vector3(EndPoint, 0, 0), Vector3.zero, fracJourney);

        if (System.Math.Abs(rt.anchoredPosition.x) < double.Epsilon)
        {
            startTime = -1;
            moving = Moving.None;
            mv_pos = Moving.Right;
            AppManager.Prefabs[0].GetComponent<AlarmList>().Return.gameObject.SetActive(true);
        }

        // deal with game space
        GameSpace.transform.position = Vector3.Lerp(GS_EndPoint, Vector3.zero, fracJourney);
        Gamespace.Background.transform.position = Vector3.Lerp(BG_EndPoint, BG_StartPoint, fracJourney);
    }
}
