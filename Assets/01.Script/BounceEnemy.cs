using UnityEngine;

public class BounceEnemy : Enemy
{
    private float rot = 0;
    private Vector3 dirVec = Vector2.one;

    protected override void Start()
    {
        base.Start();
        SetRot();
    }

    private void Update()
    {
        BoundMove();
    }
    private void BoundMove()
    {
        enemyPos = PixelManager.Instance.WorldtoIndexPoint(transform.position);
        Vector2Int nextPos = PixelManager.Instance.WorldtoIndexPoint(transform.position + dirVec*Time.deltaTime*2f);
        Vector2Int firstPos = (PixelManager.Instance.WorldtoIndexPoint(transform.position) + nextPos) / 2;
        if (CheckPixel(nextPos, PixelState.none) || CheckPixel(firstPos, PixelState.none))
        {
            enemyPos = nextPos;
        }
        else
        {
            if (CheckPixel(nextPos, PixelState.moveLine) || CheckPixel(firstPos, PixelState.moveLine))
            {
                GameManager.Instance.LifeHit(1);
                SetRot();
            }
            else
            {
                SetRot();
            }
        }
        transform.position = PixelManager.Instance.IndextoWorldPoint(enemyPos);

    }

    private void SetRot()
    {
        rot = Random.Range(0, 360);
        dirVec = Quaternion.Euler(0, 0, rot) * Vector2.one;
    }
}

