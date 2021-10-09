using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class LeafController : MonoBehaviour
{
    public LeafPrefabInfo leafPrefabInfo;
    public GameObject leftButton;
    public GameObject rightButton;
    private Globals globals;
    private bool clicked = false;

    void Start()
    {
        globals = FindObjectOfType<Globals>();
        var autoDestroy = 20.0f;
        Destroy(gameObject, autoDestroy);
        Destroy(leftButton, autoDestroy);
        Destroy(rightButton, autoDestroy);

        leftButton.GetComponent<TextButtonController>().OnButtonClicked +=
            HandleLeftLeafButtonClicked;
        rightButton.GetComponent<TextButtonController>().OnButtonClicked +=
            HandleRightLeafButtonClicked;
    }

    void Update()
    {
        var speed = globals.speed;
        this.transform.Translate(Vector3.down * Time.deltaTime * speed);
        if (leftButton != null) {
            leftButton.transform.Translate(Vector3.down * Time.deltaTime * speed);
        }
        if (rightButton != null) {
            rightButton.transform.Translate(Vector3.down * Time.deltaTime * speed);
        }
    }

    public Bounds GetLeafBounds()
    {
        return GetComponent<Renderer>().bounds;
    }

    private void HandleLeftLeafButtonClicked()
    {
        HandleLeafButtonClicked(leftButton);
    }

    private void HandleRightLeafButtonClicked()
    {
        HandleLeafButtonClicked(rightButton);
    }

    private void HandleLeafButtonClicked(GameObject button)
    {
        if (clicked || globals.gameOver) {
            return;
        }
        clicked = true;
        var name = button.GetComponent<TextMeshPro>().text;
        var isCorrect = name == leafPrefabInfo.name;
        if (isCorrect) {
            Events.OnCorrectChoice.Invoke();
        } else {
            Events.OnIncorrectChoice.Invoke();
        }
        var fadeDuration = 0.1f;
        Animations.FadeOut(leftButton, fadeDuration).Forget();
        Animations.FadeOut(rightButton, fadeDuration).Forget();
    }
}
