using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleTon<GameManager>
{
    [SerializeField] private SpriteRenderer mainSprite;

    Vector2 indexPer;
    Vector2 worldPer;

    protected override void Awake()
    {
        base.Awake();
        indexPer.x = 1920f / mainSprite.sprite.texture.width;
        indexPer.y = 1080f / mainSprite.sprite.texture.height;
        worldPer.x = mainSprite.sprite.texture.width / 1920;
        worldPer.y = mainSprite.sprite.texture.height / 1080;
    }

    public Vector3 IndexToPos(float x, float y)
    {
       return Camera.main.ScreenToWorldPoint(new Vector3(x * indexPer.x, y * indexPer.y, 10));
    }
}
