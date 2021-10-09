using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class ScoreController : MonoBehaviour
{
    private TextMeshPro scoreText;

    private static float halfAnimationTime = 0.4f;

    private static Color red = new Color(0.7490196f, 0.0f, 0.03921569f, 0.5f);
    private static Color green = new Color(0.0627451f, 0.7490196f, 0.0f, 0.5f);
    private static Color redHighlighted = GetHighlightedColor(red);
    private static Color greenHighlighted = GetHighlightedColor(green);
    private static Color baseColor = red;

    void Start()
    {
        scoreText = GetComponent<TextMeshPro>();

        Events.OnCorrectChoice.AddListener(UniTask.UnityAction(HandleCorrectChoice));
        Events.OnIncorrectChoice.AddListener(UniTask.UnityAction(HandleIncorrectChoice));
        Events.OnGameEnd.AddListener(UniTask.UnityAction(HandleGameEnd));
        Events.OnGameRestart.AddListener(UniTask.UnityAction(HandleGameRestart));
    }

    private static Color GetHighlightedColor(Color color, float intensity = 3.5f)
    {
        var factor = Mathf.Pow(2, intensity);
        return new Color(color.r * factor, color.g * factor, color.b * factor, color.a);
    }

    private void IncreaseScore()
    {
        var scoreValue = int.Parse(scoreText.text);
        scoreValue += 1;
        scoreText.text = scoreValue.ToString();
    }

    private async UniTaskVoid HandleCorrectChoice()
    {
        IncreaseScore();
        await TweenColor(baseColor, greenHighlighted, halfAnimationTime);
        await TweenColor(greenHighlighted, baseColor, halfAnimationTime);
    }

    private async UniTaskVoid HandleIncorrectChoice()
    {
        await TweenColor(baseColor, redHighlighted, halfAnimationTime);
        await TweenColor(redHighlighted, baseColor, halfAnimationTime);
    }

    private async UniTaskVoid HandleGameEnd()
    {
        await TweenColor(baseColor, greenHighlighted, halfAnimationTime);
    }

    private async UniTaskVoid HandleGameRestart()
    {
        scoreText.text = "0";
        await TweenColor(greenHighlighted, baseColor, halfAnimationTime);
    }

    private async UniTask TweenColor(Color startColor, Color endColor, float duration)
    {
        var startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            var ratio = (Time.time - startTime) / duration;
            ratio = Mathf.SmoothStep(0.0f, 1.0f, ratio);
            var color = Color.Lerp(startColor, endColor, ratio);
            scoreText.fontMaterial.SetColor(ShaderUtilities.ID_GlowColor, color);
            await UniTask.NextFrame();
        }
    }
}
