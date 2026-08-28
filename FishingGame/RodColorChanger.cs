using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodColorChanger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _rodSprite;

    public void ChangeRodColor()
    {
        _rodSprite.color = new Color(_rodSprite.color.r + 0.02f, _rodSprite.color.g + 0.02f, _rodSprite.color.b + 0.02f, 1);
    }
}
