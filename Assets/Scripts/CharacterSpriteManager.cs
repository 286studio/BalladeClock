using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteManager : MonoBehaviour
{
    [SerializeField] GameObject Figure;
    [SerializeField] GameObject Face;

    [Header("Figure")]
    [SerializeField] Sprite[] middle_sprites1;
    [SerializeField] Sprite[] middle_sprites2;
    [SerializeField] Sprite[] left_sprites;
    [SerializeField] Sprite[] right_sprites;
    Sprite[][] sprites;

    [Header("Face")]
    [SerializeField] Sprite[] middle_face1;
    [SerializeField] Sprite[] middle_face2;
    [SerializeField] Sprite[] left_face;
    [SerializeField] Sprite[] right_face;
    [SerializeField] Sprite[] middle_face1_facered;
    [SerializeField] Sprite[] middle_face2_facered;
    [SerializeField] Sprite[] left_face_facered;
    [SerializeField] Sprite[] right_face_facered;
    [SerializeField] Sprite[] shout_face;
    Sprite[][][] faces;
    SpriteRenderer face_sr;
    float[] keyframes = {12, 2, 2, 4, 190, 2, 2, 4, 190, 2, 2, 4};
    float sample_rate = 30;
    int expression = 0;
    int keyframe_idx = 0;
    int anim_idx = 0;
    int pose_idx = 0;
    int costumes_idx = 0;
    float anim_time;


    [SerializeField] Vector2[] facePositions;

    // Start is called before the first frame update
    void Awake()
    {
        sprites = new Sprite[4][] { middle_sprites1, middle_sprites2, left_sprites, right_sprites };

        var normal_faces = new Sprite[4][] { middle_face1, middle_face2, left_face, right_face };
        var red_faces = new Sprite[4][] { middle_face1_facered, middle_face2_facered, left_face_facered, right_face_facered };
        var shout_faces = new Sprite[1][] { shout_face };
        faces = new Sprite[3][][] { normal_faces, red_faces, shout_faces };

        face_sr = Face.GetComponent<SpriteRenderer>();
        anim_time = Time.time;
    }

    private void Update()
    {
        // animate face
        face_sr.sprite = faces[expression][pose_idx][anim_idx];
        if (Time.time - anim_time > keyframes[keyframe_idx] / sample_rate)
        {
            anim_time = Time.time;
            anim_idx = (anim_idx + 1) % 4;
            keyframe_idx = (keyframe_idx + 1) % keyframes.Length;
        }
    }

    public int show(int pose, int costumes, int touched)
    {
        if (touched >= 0) expression = touched;
        if (pose >= 0) pose_idx = pose;
        if (pose == -2) pose_idx = (pose_idx + Random.Range(1, 3)) % 4;
        if (costumes >= 0) costumes_idx = costumes;
        Figure.GetComponent<SpriteRenderer>().sprite = sprites[pose_idx][costumes_idx % sprites[pose_idx].Length];
        Face.GetComponent<RectTransform>().anchoredPosition = facePositions[pose_idx];
        return pose_idx;
    }

    public void showAlarm(int ringer)
    {
        // make sure you switch to the right character before calling this function
        switch(ringer)
        {
            case 0: // 陈星瑶唱OP
                show(0, -1, 1);
                break;
            case 1: // 陈星瑶呐喊
                show(0, -1, 2);
                break;
            case 2: // 谭明美：很黄很暴力
                show(3, -1, 1);
                break;
            case 3: // 李若瑜：哈哈哈哈
                show(2, -1, 1);
                break;
            case 4: // 程可：时间很晚了
                show(2, -1, 1);
                break;
        }
    }
}
