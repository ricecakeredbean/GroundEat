using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
    KeyCode[] keyArr = new KeyCode[4] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    Vector2Int[] dir = new Vector2Int[4] { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right };
    
    public Vector2Int playerPos;
    public float moveT;

    private void Start()
    {
        StartCoroutine(MoveCor());
    }
    IEnumerator MoveCor()
    {
        while (true)
        {
            for (int i = 0; i < keyArr.Length; i++)
            {
                if (Input.GetKey(keyArr[i]))
                {
                    playerPos += dir[i];
                }
                yield return null;
            }
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(playerPos.x * playerPos.x / 1920, playerPos.y * playerPos.y / 1080, 10));
            Vector3 orzPos = transform.position;
            float per = 1 / moveT;
            for (float t = 0; t<moveT;t+=Time.deltaTime)
            {
                transform.position = Vector3.Lerp(orzPos, pos, t * per);
                yield return null;
            }
            transform.position = pos;
        }
    }
}
