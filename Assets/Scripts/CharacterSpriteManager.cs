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
    void Start()
    {
        sprites = new Sprite[4][] { middle_sprites1, middle_sprites2, left_sprites, right_sprites };

        var normal_faces = new Sprite[4][] { middle_face1, middle_face2, left_face, right_face };
        var red_faces = new Sprite[4][] { middle_face1_facered, middle_face2_facered, left_face_facered, right_face_facered };
        faces = new Sprite[2][][] { normal_faces, red_faces };

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

    public void show(int pose, int costumes, int touched)
    {
        if (touched >= 0) expression = touched;
        if (pose >= 0) pose_idx = pose;
        if (pose == -2) pose_idx = (pose_idx + Random.Range(1, 3)) % 4;
        if (costumes >= 0) costumes_idx = costumes;
        Figure.GetComponent<SpriteRenderer>().sprite = sprites[pose_idx][costumes_idx];
        Face.GetComponent<RectTransform>().anchoredPosition = facePositions[pose_idx];
    }
}
