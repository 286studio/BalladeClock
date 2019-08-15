using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteManager : MonoBehaviour
{
    [SerializeField] GameObject Figure;
    [SerializeField] GameObject Face;
    Sprite[][] sprites;
    [SerializeField] Sprite[] middle_sprites1;
    [SerializeField] Sprite[] middle_sprites2;
    [SerializeField] Sprite[] left_sprites;
    [SerializeField] Sprite[] right_sprites;

    [SerializeField] Vector2[] facePositions;

    // Start is called before the first frame update
    void Start()
    {
        sprites = new Sprite[4][];
        sprites[0] = middle_sprites1;
        sprites[1] = middle_sprites2;
        sprites[2] = left_sprites;
        sprites[3] = right_sprites;
    }

    public void show(int pose_idx, int costumes)
    {
        Figure.GetComponent<SpriteRenderer>().sprite = sprites[pose_idx][costumes];
        Face.GetComponent<RectTransform>().anchoredPosition = facePositions[pose_idx];
        Face.GetComponent<Animator>().SetInteger("Pose", pose_idx);
    }
}
