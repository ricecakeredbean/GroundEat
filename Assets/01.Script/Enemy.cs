using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Vector2Int enemyPos;
    protected Texture2D mainTexture { get; private set; }

    protected virtual void Start()
    {
        mainTexture = PixelManager.Instance.mainSprite.sprite.texture;
    }

    protected virtual bool CheckPixel(Vector2Int checkPos,PixelState state)
    {
        return checkPos.x > 0 && checkPos.y > 0 && checkPos.x < mainTexture.width - 1 && checkPos.y < mainTexture.height - 1 && PixelManager.Instance.GetPixel(checkPos) == state;
    }
}
