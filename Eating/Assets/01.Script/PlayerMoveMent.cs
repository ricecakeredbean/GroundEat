using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveMent : MonoSingleTon<PlayerMoveMent>
{
    //KeyCode[] moveKey = new KeyCode[4] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    //Vector2Int[] dirVec = new Vector2Int[4] { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right };
    //List<Vector2Int> inputDirList = new();

    //public Vector2Int playerPos;
    //public float moveT;

    //private void Start()
    //{
    //    StartCoroutine(MoveCor());
    //}
    //IEnumerator MoveCor()
    //{
    //    transform.position = GameManager.instance.IndexToPos(playerPos.x, playerPos.y);
    //    while (true)
    //    {
    //        do
    //        {

    //            for (int i = 0; i < moveKey.Length; i++)
    //            {
    //                if (Input.GetKey(moveKey[i]))
    //                {
    //                    if (!inputDirList.Contains(dirVec[i]))
    //                        inputDirList.Add(dirVec[i]);
    //                }
    //                else if (inputDirList.Contains(dirVec[i]))
    //                {
    //                    inputDirList.Remove(dirVec[i]);
    //                }
    //                yield return null;
    //            }
    //        }
    //        while (inputDirList.Count == 0);

    //        if (inputDirList[0] != Vector2Int.zero)
    //        {
    //            Vector2Int firstPos = GameManager.instance.ClampedIndex(playerPos + inputDirList[inputDirList.Count - 1]);
    //            Vector2Int secondPos = GameManager.instance.ClampedIndex(playerPos + inputDirList[inputDirList.Count - 1] * 2);

    //            PixelState firstState = GameManager.instance.GetPixel(firstPos);
    //            PixelState secondState = GameManager.instance.GetPixel(secondPos);

    //            bool check = secondState != PixelState.moveLine && secondState != PixelState.fill && firstState != PixelState.moveLine && firstState != PixelState.fill;

    //            if (check)
    //            {
    //                if(GameManager.instance.GetPixel(playerPos)==PixelState.none)
    //                    Ga
    //            }
    //        }
    //        Vector3 pos = GameManager.instance.IndexToPos(playerPos.x, playerPos.y);
    //        Vector3 orzPos = transform.position;
    //        float per = 1 / moveT;
    //        for (float t = 0; t < moveT; t += Time.deltaTime)
    //        {
    //            transform.position = Vector3.Lerp(orzPos, pos, t * per);
    //            yield return null;
    //        }
    //        transform.position = pos;
    //    }
    //}
    KeyCode[] moveKey = new KeyCode[4] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    Vector2Int[] vecDir = new Vector2Int[4] { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right };
    List<Vector2Int> inputDirList = new();

    public List<Vector2Int> moveLineList = new();

    public float moveInterval;

    public Vector2Int playerPos;
    public Vector2Int PlayerPos
    {
        get => playerPos;
        set
        {
            value = GameManager.instance.ClampedIndex(value);
            playerPos = value;
        }
    }

    private void Start()
    {
        StartCoroutine(Move_Cor());
    }
    IEnumerator Move_Cor()
    {
        while (true)
        {
            do
            {
                for (int i = 0; i < moveKey.Length; i++)
                {
                    if (Input.GetKey(moveKey[i]))
                    {
                        if (!inputDirList.Contains(vecDir[i]))
                            inputDirList.Add(vecDir[i]);
                    }
                    else if (inputDirList.Contains(vecDir[i]))
                    {
                        inputDirList.Remove(vecDir[i]);
                    }
                    yield return null;
                }
            }
            while (inputDirList.Count == 0);
            if (inputDirList[0] != Vector2Int.zero)
            {
                PlayerPos += inputDirList[inputDirList.Count - 1];

                if (PixelState.none == GameManager.instance.GetPixel(playerPos))
                {
                    moveLineList.Add(playerPos);
                    GameManager.instance.SetPixel(playerPos, PixelState.moveLine);
                }
            }
            Vector3 pos = GameManager.instance.IndexToPos(playerPos.x, playerPos.y);
            Vector3 orzPos = transform.position;

            float per = 1 / moveInterval;
            for (float t = 0; t < moveInterval; t += Time.deltaTime)
            {
                Vector3.Lerp(orzPos, pos, t * per);
            }
            transform.position = pos;
        }
    }
}
