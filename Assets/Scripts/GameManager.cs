using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static Helpers;

public class GameManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 cursorHotSpot = Vector2.zero;
    public GameObject clockWidget;
    public GameObject scoreWidget;
    public GameObject startButton;
    public GameObject playAgainButton;

    private Globals globals;

    public float totalGameTime;
    float currentGameTime = 0.0f;

    void Start()
    {
        globals = GetComponent<Globals>();
        Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.ForceSoftware);

        startButton.GetComponent<TextButtonController>()
            .OnButtonClicked += StartGame;

        playAgainButton.GetComponent<TextButtonController>()
            .OnButtonClicked += UniTask.Action(RestartGame);
    }

    void Update()
    {
        currentGameTime += Time.deltaTime;

        if (currentGameTime >= totalGameTime)
        {
            if (!globals.gameOver) {
                globals.gameOver = true;
                EndGame().Forget();
            }
        }
        else if (!globals.gameOver)
        {
            var timeLeft = (totalGameTime - currentGameTime) / totalGameTime;
            clockWidget.GetComponent<MeterScript>().SetHealth(timeLeft);
            globals.speed = 1.0f + 3.0f * (currentGameTime / totalGameTime);
        }
    }

    private void StartGame()
    {
        currentGameTime = 0.0f;
        globals.gameOver = false;
        Events.OnGameStart.Invoke();

        var fadeDuration = 0.5f;

        UniTask.Void(async () => {
            await Animations.FadeOut(startButton, fadeDuration);
            Destroy(startButton);
        });

        var renderers = GameObject.Find("Logo").GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in renderers)
        {
            UniTask.Void(async () => {
                await UniTask.Delay(Seconds(Random.Range(0.0f, 1.0f)));
                await Animations.FadeOut(renderer.gameObject, fadeDuration);
                Destroy(renderer.gameObject);
            });
        }
    }

    private async UniTaskVoid EndGame()
    {
        clockWidget.GetComponent<MeterScript>().SetHealth(0);
        Events.OnGameEnd.Invoke();
        await UniTask.Delay(Seconds(3));
        playAgainButton.SetActive(true);
        await Animations.FadeIn(playAgainButton, 0.5f);
    }

    private async UniTaskVoid RestartGame()
    {
        currentGameTime = 0.0f;
        globals.gameOver = false;
        Events.OnGameRestart.Invoke();
        await Animations.FadeOut(playAgainButton, 0.5f);
        playAgainButton.SetActive(false);
    }
}
