using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PixelState
{
    none,
    fill,
    boundary,
    moveLine
}

public class GameManager : MonoSingleTon<GameManager>
{
    [SerializeField] private SpriteRenderer mainSprite;

    Vector2 indexPer;
    Vector2 worldPer;
    Vector2Int textureSize;

    PixelState[,] pixelStates;

    public Texture2D decureTexture;
    public Texture2D cureTexture;

    protected override void Awake()
    {
        Texture2D mainTexture = Instantiate(decureTexture);
        mainSprite.sprite = Sprite.Create(mainTexture, new Rect(0, 0, mainTexture.width, mainTexture.height), Vector2.one * 0.5f, mainSprite.sprite.pixelsPerUnit);
        base.Awake();
        textureSize = new Vector2Int(mainSprite.sprite.texture.width, mainSprite.sprite.texture.height);
        pixelStates = new PixelState[mainSprite.sprite.texture.width, mainSprite.sprite.texture.height];
        indexPer.x = 1920f / mainSprite.sprite.texture.width;
        indexPer.y = 1080f / mainSprite.sprite.texture.height;
        worldPer.x = mainSprite.sprite.texture.width / 1920;
        worldPer.y = mainSprite.sprite.texture.height / 1080;
    }

    private void FixedUpdate()
    {
        mainSprite.sprite.texture.Apply();
    }

    public Vector3 IndexToPos(float x, float y)
    {
       return Camera.main.ScreenToWorldPoint(new Vector3(x * indexPer.x, y * indexPer.y, 10));
    }

    public Vector2Int ClampedIndex(Vector2Int index)
    {
        if (index.x >= textureSize.x)
            index.x = textureSize.x-1;
        else if (index.x < 0)
            index.x = 0;

        if (index.y >= textureSize.y)
            index.y = textureSize.y-1;
        else if (index.y < 0)
            index.y = 0;
        return index;
    }

    public void SetPixel(Vector2Int vec, PixelState state)
    {
        mainSprite.sprite.texture.SetPixel(vec.x, vec.y, Color.red);
        pixelStates[vec.x, vec.y] = state;
    }

    public PixelState GetPixel(Vector2Int vec)
    {
        return pixelStates[vec.x, vec.y];
    }
}
