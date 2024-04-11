using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
    [SerializeField] private Vector2Int playerPos = new();

    private KeyCode[] moveInput = new KeyCode[5] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space };
    private Vector2Int[] moveVec = new Vector2Int[5] { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right, Vector2Int.zero };

    [SerializeField] private List<Vector2Int> dirVecList = new();
    [SerializeField] private List<Vector2Int> moveList = new();

    private void Start()
    {
        transform.position = PixelManager.Instance.IndextoWorldPoint(playerPos);
        StartCoroutine(Move_Cor());
    }

    private void PlayerPosMove(Vector2Int pos)
    {
        Texture2D mainTexture = PixelManager.Instance.mainSprite.sprite.texture;

        Vector2Int firstPos = playerPos + pos;
        Vector2Int originPos = playerPos + pos * 2;
        if (originPos.x >= 0 && originPos.x < mainTexture.width && originPos.y >= 0 && originPos.y < mainTexture.height)
        {
            if (PixelManager.Instance.GetPixel(firstPos) == PixelState.none && PixelManager.Instance.GetPixel(originPos) == PixelState.none)
            {
                moveList.Add(playerPos);
                moveList.Add(firstPos);
                moveList.Add(originPos);
                PixelManager.Instance.SetPixel(firstPos, PixelState.moveLine);
                PixelManager.Instance.SetPixel(originPos, PixelState.moveLine);
                playerPos = originPos;
            }
            else if (PixelManager.Instance.GetPixel(firstPos) == PixelState.none && PixelManager.Instance.GetPixel(originPos) == PixelState.border)
            {
                moveList.Add(firstPos);
                PixelManager.Instance.SetPixel(firstPos, PixelState.moveLine);
                if (moveList.Count > 0)
                {
                    PixelManager.Instance.FloodFill(moveList[1]);
                    PixelManager.Instance.BorderListIn(moveList);
                    for (int i = 0; i < moveList.Count; i++)
                    {
                        PixelManager.Instance.SetPixel(moveList[i], PixelState.border);
                    }
                    moveList.Clear();
                }
                playerPos = originPos;
            }
            else if (PixelManager.Instance.GetPixel(firstPos) == PixelState.border && PixelManager.Instance.GetPixel(originPos) == PixelState.border)
            {
                playerPos = originPos;
            }
        }
    }

    private void PlayerPosBackMove()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2Int dirVec = moveList[^1] - playerPos;
            if (PixelManager.Instance.GetPixel(moveList[^1]) == PixelState.moveLine)
                PixelManager.Instance.SetPixel(moveList[^1], PixelState.none);
            playerPos += dirVec;
            moveList.RemoveAt(moveList.Count - 1);
        }
    }

    private IEnumerator Move_Cor()
    {
        while (true)
        {
            do
            {
                for (int i = 0; i < moveInput.Length; i++)
                {
                    if (Input.GetKey(moveInput[i]))
                    {
                        if (!dirVecList.Contains(moveVec[i]))
                        {
                            dirVecList.Add(moveVec[i]);
                        }
                    }
                    else
                    {
                        dirVecList.Remove(moveVec[i]);
                    }
                    yield return null;
                }
            }
            while (dirVecList.Count == 0);
            if (dirVecList[^1] != Vector2Int.zero)
            {
                PlayerPosMove(dirVecList[^1]);
            }
            else if (moveList.Count > 0)
            {
                PlayerPosBackMove();
            }
            transform.position = PixelManager.Instance.IndextoWorldPoint(playerPos);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            GameManager.Instance.LifeHit(1);
        }
    }

    public void BackToStart()
    {
        for (int i = 0; i < moveList.Count; i++)
        {
            PixelManager.Instance.SetPixel(moveList[i], PixelState.none);
        }
        playerPos = moveList[0];
        moveList.Clear();
    }
}
