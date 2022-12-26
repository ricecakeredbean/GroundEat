using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PixelState
{
    none,
    moveLine,
    fill,
    border
}

public class PixelManager : MonoSingleTon<PixelManager>
{
    [SerializeField] private SpriteRenderer mainSprite;

    [SerializeField] private Texture2D beforeTexture;
    [SerializeField] private Texture2D AfterTexture;

    private float PerX;
    private float PerY;

    PixelState[,] pixelStates;

    protected override void Awake()
    {
        base.Awake();
        PerX = 1920f / mainSprite.sprite.texture.width;
        PerY = 1080f / mainSprite.sprite.texture.height;
        Texture2D copyTexture = Instantiate(beforeTexture);
        mainSprite.sprite = Sprite.Create(copyTexture, new Rect(0, 0, copyTexture.width, copyTexture.height), Vector2.one * 0.5f, mainSprite.sprite.pixelsPerUnit);
    }
    private void Start()
    {
        BorderSet();
    }

    private void BorderSet()
    {
        for (int x = 0; x < mainSprite.sprite.texture.width; x++)
        {
            for (int y = 0; y < mainSprite.sprite.texture.height; y++)
            {
                if (x == 0 || y == 0 || x == mainSprite.sprite.texture.width - 1 || y == mainSprite.sprite.texture.height - 1)
                {
                    pixelStates[x, y] = PixelState.border;
                }
            }
        }
    }

    public void SetPixel(Vector2Int pos,PixelState state)
    {
        pixelStates[pos.x,pos.y] = state;
    }    

    public Vector3 ChangePos(Vector2Int pos)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(pos.x * PerX, pos.y * PerY, 10));
    }

    public PixelState GetPixel(Vector2Int pos)
    {
        return pixelStates[pos.x,pos.y];
    }
}
