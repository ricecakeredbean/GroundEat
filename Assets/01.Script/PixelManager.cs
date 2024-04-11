using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PixelState
{
    none,
    moveLine,
    fill,
    border
}


public class PixelManager : MonoSingleTon<PixelManager>
{
    public SpriteRenderer mainSprite;

    [SerializeField] private Texture2D beforeTexture;
    [SerializeField] private Texture2D afterTexture;

    private float indexPerX;
    private float indexPerY;
    private float worldPerX;
    private float worldPerY;

    private int spriteX;
    private int spriteY;

    PixelState[,] pixelStates;

    private Color[] pixelColors = new Color[4] { Color.white, Color.red, Color.blue, Color.black };

    private List<Vector2Int> borders = new();


    protected override void Awake()
    {
        spriteX = mainSprite.sprite.texture.width;
        spriteY = mainSprite.sprite.texture.height;
        indexPerX = 1920f / spriteX;
        indexPerY = 1080f / spriteY;
        worldPerX = spriteX / 1920f;
        worldPerY = spriteY / 1080f;

        base.Awake();
        Texture2D copyTexture = Instantiate(beforeTexture);
        mainSprite.sprite = Sprite.Create(copyTexture, new Rect(0, 0, copyTexture.width, copyTexture.height), Vector2.one * 0.5f, mainSprite.sprite.pixelsPerUnit);
        pixelStates = new PixelState[spriteX, spriteY];
    }

    private void Start()
    {
        SetBorder();
    }

    private void FixedUpdate()
    {
        mainSprite.sprite.texture.Apply();
    }

    private void SetBorder()
    {
        for (int x = 0; x < spriteX; x++)
        {
            SetPixel(new Vector2Int(x, 0), PixelState.border);
            SetPixel(new Vector2Int(x, spriteY - 1), PixelState.border);
        }
        for (int y = 0; y < spriteY; y++)
        {
            SetPixel(new Vector2Int(0, y), PixelState.border);
            SetPixel(new Vector2Int(spriteX - 1, y), PixelState.border);
        }
    }

    public void SetPixel(Vector2Int pos, PixelState state)
    {
        pixelStates[pos.x, pos.y] = state;
        if (state != PixelState.fill && state != PixelState.none)
        {
            mainSprite.sprite.texture.SetPixel(pos.x, pos.y, pixelColors[(int)state]);
        }
        else if (state == PixelState.none)
        {
            mainSprite.sprite.texture.SetPixel(pos.x, pos.y, beforeTexture.GetPixel(pos.x, pos.y));
        }
        else if (state == PixelState.fill)
        {
            mainSprite.sprite.texture.SetPixel(pos.x, pos.y, afterTexture.GetPixel(pos.x, pos.y));
        }
    }

    public PixelState GetPixel(Vector2Int pos)
    {
        return pixelStates[pos.x, pos.y];
    }


    public void FloodFill(Vector2Int startPos)
    {
        Stack<Vector2Int> activePixelStack1 = new();
        HashSet<Vector2Int> closePixelHashSet1 = new();

        Stack<Vector2Int> activePixelStack2 = new();
        HashSet<Vector2Int> closePixelHashSet2 = new();

        Vector2Int operationPos;

        
        {

            operationPos = startPos + Vector2Int.right;
            if (pixelStates[startPos.x, startPos.y] == PixelState.moveLine && pixelStates[operationPos.x, operationPos.y] == PixelState.moveLine)
            {
                AddPixel_1(startPos + Vector2Int.up);
                AddPixel_2(startPos + Vector2Int.down);
            }
            else
            {
                AddPixel_1(startPos + Vector2Int.left);
                AddPixel_2(startPos + Vector2Int.right);
            }

            while (activePixelStack1.Count != 0 && activePixelStack2.Count != 0)
            {
                Vector2Int comparePos1 = activePixelStack1.Pop();
                Vector2Int comparePos2 = activePixelStack2.Pop();
                Vector2Int[] diractions = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

                for (int i = 0; i < 4; i++)
                {
                    Vector2Int operPos = comparePos1 + diractions[i];
                    if (FloodChekPixel(operPos, closePixelHashSet1))
                    {
                        AddPixel_1(operPos);
                        //activePixelStack1.Push(operPos);
                        //closePixelHashSet1.Add(comparePos1);
                    }
                    operPos = comparePos2 + diractions[i];
                    if (FloodChekPixel(operPos, closePixelHashSet2))
                    {
                        AddPixel_2(operPos);
                        //activePixelStack2.Push(operPos);
                        //closePixelHashSet2.Add(comparePos2);2
                    }
                }
            }

        }



        //operationPos = startPos + Vector2Int.right;
        //if (pixelStates[startPos.x, startPos.y] == PixelState.moveLine && pixelStates[operationPos.x, operationPos.y] == PixelState.moveLine)
        //{
        //    activePixelStack1.Push(startPos + Vector2Int.up);
        //    activePixelStack2.Push(startPos + Vector2Int.down);
        //}
        //else
        //{
        //    activePixelStack1.Push(startPos + Vector2Int.left);
        //    activePixelStack2.Push(startPos + Vector2Int.right);
        //}

        //while (activePixelStack1.Count != 0 && activePixelStack2.Count != 0)
        //{
        //    Vector2Int comparePos1 = activePixelStack1.Pop();
        //    Vector2Int comparePos2 = activePixelStack2.Pop();
        //    Vector2Int[] diractions = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        //    for (int i = 0; i < 4; i++)
        //    {
        //        Vector2Int operPos = comparePos1 + diractions[i];
        //        if (FloodChekPixel(operPos, closePixelHashSet1))
        //        {
        //            activePixelStack1.Push(operPos);
        //            closePixelHashSet1.Add(comparePos1);
        //        }
        //        operPos = comparePos2 + diractions[i];
        //        if (FloodChekPixel(operPos, closePixelHashSet2))
        //        {
        //            activePixelStack2.Push(operPos);
        //            closePixelHashSet2.Add(comparePos2);
        //        }
        //    }
        //}
        List<Vector2Int> fillList;
        if (activePixelStack1.Count == 0)
        {
            fillList = closePixelHashSet1.ToList();
        }
        else
        {
            fillList = closePixelHashSet2.ToList();
        }
        for (int i = 0; i < fillList.Count; i++)
        {
            SetPixel(fillList[i], PixelState.fill);
        }
        GameManager.Instance.PercentRefresh(fillList.Count);


        void AddPixel_1(Vector2Int index)
        {
            activePixelStack1.Push(index);
            closePixelHashSet1.Add(index);
        }
        void AddPixel_2(Vector2Int index)
        {
            activePixelStack2.Push(index);
            closePixelHashSet2.Add(index);
        }

        BorderFill();
    }

    public Vector3 IndextoWorldPoint(Vector2Int pos)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(pos.x * indexPerX, pos.y * indexPerY, 10));
    }
    public Vector2Int WorldtoIndexPoint(Vector3 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(Camera.main.WorldToScreenPoint(pos).x * worldPerX), Mathf.RoundToInt(Camera.main.WorldToScreenPoint(pos).y * worldPerY));
    }

    public void BorderListIn(List<Vector2Int> borderList)
    {
        for (int i = 0; i < borderList.Count; i++)
        {
            borders.Add(borderList[i]);
        }
    }

    private void BorderFill()
    {
        Vector2Int[] diractions = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };
        for (int i = 0; i < borders.Count; i++)
        {
            if (BorderChekPixel(borders[i] + diractions[0]) && BorderChekPixel(borders[i] + diractions[1]) && BorderChekPixel(borders[i] + diractions[2]) && BorderChekPixel(borders[i] + diractions[3]))
            {
                SetPixel(borders[i], PixelState.fill);
            }
        }
    }

    private bool FloodChekPixel(Vector2Int chekPos, HashSet<Vector2Int> hash)
    {
        return chekPos.x > 0 && chekPos.y > 0 && chekPos.x < spriteX && chekPos.y < spriteY && pixelStates[chekPos.x, chekPos.y] == PixelState.none && !hash.Contains(chekPos);
    }
    private bool BorderChekPixel(Vector2Int chekPos)
    {
        return chekPos.x > 0 && chekPos.y > 0 && chekPos.x < spriteX && chekPos.y < spriteY && pixelStates[chekPos.x, chekPos.y] != PixelState.none;
    }
}
