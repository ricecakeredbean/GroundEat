using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoSingleTon<GameManager>
{
    [SerializeField] PlayerMoveMent player;

    [SerializeField] private Text timeText;
    [SerializeField] private Text perText;
    [SerializeField] private Image[] lifeImage;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private Text gameStateText;

    private Func<bool> protectedFunc;

    #region value
    private float time;

    private int min;

    private float maxPixel;

    private float per;

    private int life = 3;

    private float protectedTime;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        protectedFunc = () => protectedTime > 0;
    }

    private void Start()
    {
        maxPixel = PixelManager.Instance.mainSprite.sprite.texture.width * PixelManager.Instance.mainSprite.sprite.texture.height;
    }

    private void Update()
    {
        TimeCountUp();
        LifeUpdate();
        timeText.text = $"Time : {min:D2}:{time:F2}";
        perText.text = $"ÁøÇàµµ{per:F2}%";
    }

    private void TimeCountUp()
    {
        time += Time.deltaTime;
        if (time >= 60f)
        {
            min++;
            time -= 60f;
        }
    }

    private void LifeUpdate()
    {
        for (int i = 0; i < lifeImage.Length; i++)
        {
            lifeImage[i].enabled = false;
        }
        for (int i = 0; i < life; i++)
        {
            lifeImage[i].enabled = true;
        }
    }

    private void GameEnd()
    {
        Time.timeScale = 0;
        endPanel.SetActive(true);

    }

    public void PercentRefresh(float fillPixelCount)
    {
        per += fillPixelCount / maxPixel * 100;
        if (per > 85f)
        {
            GameEnd();
            gameStateText.text = "Game Clear";
            gameStateText.color = Color.yellow;
        }
    }
    public void LifeHit(int sumLife)
    {
        if (!protectedFunc.Invoke())
        {
            if (life > 1)
            {
                life -= sumLife;
                StartCoroutine(Player_Protected());
                player.BackToStart();
            }
            else
            {
                Destroy(player);
                GameEnd();
                gameStateText.text = "Game Over";
                gameStateText.color = Color.red;
            }
        }
    }

    private IEnumerator Player_Protected()
    {
        protectedTime = 0.85f;
        while (protectedFunc.Invoke())
        {
            Debug.Log(protectedTime);
            protectedTime -= Time.deltaTime;
            yield return null;
        }
    }
}
