using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region set
    [SerializeField] private Vector2Int playerPos;

    private KeyCode[] moveInput = new KeyCode[5] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space };
    private Vector2Int[] moveInputVec = new Vector2Int[5] { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right, Vector2Int.zero };
    [SerializeField] private List<Vector2Int> InputDirList = new();

    [SerializeField] private List<Vector2Int> moveList = new();

    #endregion
    private void Start()
    {
        StartCoroutine(Move_Cor());
    }

    private void PlayerPosMove(Vector2Int dirVec)
    {
        Vector2Int pPos = playerPos;
        pPos += dirVec * 2;
        pPos = new Vector2Int(Mathf.Clamp(pPos.x, 0, 960), Mathf.Clamp(pPos.y, 0, 540));
        if (PixelManager.instance.GetPixel(pPos) == PixelState.none || PixelManager.instance.GetPixel(pPos) == PixelState.border)
        {
            playerPos = pPos;
            moveList.Add(playerPos);
        }
    }

    private IEnumerator Move_Cor()
    {
        while (true)
        {
            for (int i = 0; i < moveInput.Length; i++)
            {
                if (Input.GetKey(moveInput[i]))
                {
                    if (!InputDirList.Contains(moveInputVec[i]))
                    {
                        InputDirList.Add(moveInputVec[i]);
                    }
                }
                else if (InputDirList.Contains(moveInputVec[i]))
                {
                    InputDirList.Remove(moveInputVec[i]);
                }
                if (InputDirList.Count > 0)
                {
                    PlayerPosMove(InputDirList[^1]);
                }
                yield return null;
            }
            transform.position = PixelManager.instance.ChangePos(playerPos);
        }
    }
}