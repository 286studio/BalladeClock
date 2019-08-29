using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image[] Buttons;
    public Button Done;
    public GameObject[] flashing;
    public GameObject Finger;

    Vector3 FingerStartPoint;
    Vector3 FingerEndPoint;

    bool isDead;

    public static bool playedTutorial;
    public Image[] visibleImages;
    public Text[] visibleTexts;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(AppManager.AdjustedWidth, AppManager.DefaultRes.y);
        Done.GetComponent<RectTransform>().sizeDelta = new Vector2(AppManager.AdjustedWidth, AppManager.DefaultRes.y);
        ButtonAdjuster();
        Done.onClick.AddListener(delegate {
            playedTutorial = true;
            isDead = true;
            Swipable.allow_swipe = true;
        });
        Swipable.allow_swipe = false;
    }

    float flashingCount;
    float flashingRate = 1f;
    bool isFlashing;
    private void Update()
    {
        if (Time.time - flashingCount > flashingRate)
        {
            foreach (var f in flashing) f.SetActive(isFlashing);
            isFlashing = !isFlashing;
            flashingCount = Time.time;
        }
    }

    float startTime;
    bool isWaiting;
    float waitTime;

    int tCount = 155;
    private void FixedUpdate()
    {
        if (isDead)
        {
            tCount -= 5;
            if (tCount == 0) Destroy(gameObject);
            foreach (var i in visibleImages) i.color *= new Color(1, 1, 1, tCount / 255f / i.color.a);
            foreach (var t in visibleTexts) t.color *= new Color(1, 1, 1, tCount / 255f / t.color.a);
        }
        else
        {
            if (isWaiting)
            {
                if (Time.time - waitTime > 2)
                {
                    Finger.SetActive(true);
                    waitTime = Time.time;
                    isWaiting = false;
                }
            }
            else
            {
                var rt = Finger.GetComponent<RectTransform>();
                rt.anchoredPosition = Vector3.MoveTowards(rt.anchoredPosition, FingerEndPoint, 500 * Time.fixedDeltaTime);
                if (System.Math.Abs(FingerEndPoint.x - rt.anchoredPosition.x) < float.Epsilon)
                {
                    startTime = 0;
                    rt.anchoredPosition = FingerStartPoint;
                    isWaiting = true;
                    Finger.SetActive(false);
                }
            }
        }
    }

    void ButtonAdjuster()
    {
        float spacer = 120f * AppManager.AdjustedWidth / AppManager.DefaultRes.x;
        var rt = Buttons[0].GetComponent<RectTransform>();
        var pos = rt.anchoredPosition;
        pos.x = -(AppManager.AdjustedWidth / 2 - 2 * spacer - 120);
        pos.y = -(AppManager.DefaultRes.y / 2 - spacer);
        rt.anchoredPosition = pos;

        rt = Buttons[1].GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
        pos.x = AppManager.AdjustedWidth / 2 - spacer;
        pos.y = -(AppManager.DefaultRes.y / 2 - spacer);
        rt.anchoredPosition = pos;

        rt = Buttons[2].GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
        pos.x = -(AppManager.AdjustedWidth / 2 - spacer);
        pos.y = -(AppManager.DefaultRes.y / 2 - spacer);
        rt.anchoredPosition = pos;

        rt = Buttons[3].GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
        pos.x = -(AppManager.AdjustedWidth / 2 - spacer);
        pos.y = 0;
        rt.anchoredPosition = pos;
        //pos.x += 200;
        Buttons[3].gameObject.SetActive(false);
        FingerEndPoint = pos;

        rt = Buttons[4].GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
        pos.x = AppManager.AdjustedWidth / 2 - spacer;
        pos.y = 0;
        rt.anchoredPosition = pos;

        rt = Finger.GetComponent<RectTransform>();
        pos = rt.anchoredPosition;
        pos.x = AppManager.AdjustedWidth / 2 - spacer - 200;
        pos.y = 0;
        rt.anchoredPosition = pos;
        FingerStartPoint = pos;
    }
}
