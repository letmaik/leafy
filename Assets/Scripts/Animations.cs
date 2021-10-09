using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public static class Animations
{
    private static System.Action<float> GetAlphaSetter(GameObject gameObject)
    {
        var textMeshPro = gameObject.GetComponent<TextMeshPro>();
        if (textMeshPro != null) {
            return (float a) => {
                textMeshPro.alpha = a;
            };
        }
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            return (float a) => {
                var color = spriteRenderer.color;
                color.a = a;
                spriteRenderer.color = color;
            };
        }
        throw new System.Exception("No alpha setter found");
    }

    public static async UniTask FadeIn(GameObject gameObject, float duration)
    {
        var setAlpha = GetAlphaSetter(gameObject);
        float animationTime = 0.0f;
        while (animationTime < duration)
        {
            float alpha = animationTime / duration;
            setAlpha(alpha);
            await UniTask.NextFrame();
            animationTime += Time.deltaTime;
        }
        setAlpha(1.0f);
    }

    public static async UniTask FadeOut(GameObject gameObject, float duration)
    {
        var setAlpha = GetAlphaSetter(gameObject);
        float animationTime = 0.0f;
        while (animationTime < duration)
        {
            float alpha = 1.0f - animationTime / duration;
            setAlpha(alpha);
            await UniTask.NextFrame();
            animationTime += Time.deltaTime;
        }
        setAlpha(0.0f);
    }
}