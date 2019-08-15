using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSheetAnimator : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer _spriteRenderer;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            return _spriteRenderer;
        }
    }

    public void SetSprite(int index)
    {
        Debug.Log("Call");
        if (index < 0 || index >= sprites.Length)
            return;

        spriteRenderer.sprite = sprites[index];
    }
}