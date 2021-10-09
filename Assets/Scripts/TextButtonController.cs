using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextButtonController : MonoBehaviour
{
    private TextMeshPro textMeshPro;
    private BoxCollider2D boxCollider2D;
    public event System.Action OnButtonClicked;
    
    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        textMeshPro = GetComponent<TextMeshPro>();
    }

    void Start()
    {
        UpdateBoxCollider2D();
    }

    public Bounds GetButtonBounds()
    {
        textMeshPro.ForceMeshUpdate();
        var bounds = textMeshPro.mesh.bounds;
        return bounds;
    }

    private void UpdateBoxCollider2D()
    {
        var bounds = GetButtonBounds();
        boxCollider2D.size = bounds.size;
        boxCollider2D.offset = bounds.center;
    }

    // Called by collider
    void OnMouseEnter()
    {
        var color = textMeshPro.fontMaterial.GetColor(ShaderUtilities.ID_GlowColor);
        color.a = 1.0f;
        textMeshPro.fontMaterial.SetColor(ShaderUtilities.ID_GlowColor, color);
    }

    // Called by collider
    void OnMouseExit()
    {
        var color = textMeshPro.fontMaterial.GetColor(ShaderUtilities.ID_GlowColor);
        color.a = 0.5f;
        textMeshPro.fontMaterial.SetColor(ShaderUtilities.ID_GlowColor, color);
    }

    // Called by collider
    void OnMouseDown()
    {
        OnButtonClicked();
    }
}
