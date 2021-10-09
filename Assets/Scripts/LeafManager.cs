using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class LeafManager : MonoBehaviour
{
    private static LeafPrefabInfo[] leafPrefabInfos = {
        new LeafPrefabInfo("Prefabs/ash-leaf", "Ash"),
        new LeafPrefabInfo("Prefabs/birch-leaf", "Birch"),
        new LeafPrefabInfo("Prefabs/cherry-leaf", "Cherry"),
        new LeafPrefabInfo("Prefabs/field-maple-leaf", "Maple"),
        new LeafPrefabInfo("Prefabs/hazel-leaf", "Hazel"),
        new LeafPrefabInfo("Prefabs/hornbeam-leaf", "Hornbeam"),
        new LeafPrefabInfo("Prefabs/linden-leaf", "Linden"),
        new LeafPrefabInfo("Prefabs/maple-leaf", "Maple"),
        new LeafPrefabInfo("Prefabs/poplar-leaf", "Poplar"),
        new LeafPrefabInfo("Prefabs/silver-maple-leaf", "Maple"),
        new LeafPrefabInfo("Prefabs/swedish-whitebeam-leaf-3", "Whitebeam"),
    };

    private Globals globals;

    private float xMarginRatio = 0.0f;
    private float yMarginRatio = -0.2f;

    private float timeBetweenSpawn = 5.0f;
    private float elapsedTimeSinceSpawn = float.MaxValue;

    private int lastLeafIndex = -1;

    void Start()
    {
        globals = FindObjectOfType<Globals>();
    }

    void Update()
    {
        if (globals.gameOver)
        {
            return;
        }
        elapsedTimeSinceSpawn += Time.deltaTime;
        var adjustedTimeBetweenSpawn = timeBetweenSpawn * 1 / globals.speed;
        if (elapsedTimeSinceSpawn > adjustedTimeBetweenSpawn)
        {
            elapsedTimeSinceSpawn = 0.0f;
            SpawnLeaf();
        }
    }

    private void SpawnLeaf()
    {
        // Leaf
        var leafPrefabInfo = leafPrefabInfos[GetNextNonRepeatingLeafIndex()];
        var leafPrefab = leafPrefabInfo.prefab;
        var leaf = Instantiate<GameObject>(leafPrefab);
        var leafController = leaf.AddComponent<LeafController>();
        leafController.leafPrefabInfo = leafPrefabInfo;

        // Buttons
        var buttonPrefab = Resources.Load<GameObject>("Prefabs/text-button");

        var leafName = leafPrefabInfo.name;
        var otherLeafName = GetOtherLeafName(leafPrefabInfo.name);
        var random = Random.Range(0.0f, 1.0f) < 0.5f;

        var leftButton = Instantiate<GameObject>(buttonPrefab);
        var leftTextMeshPro = leftButton.GetComponent<TextMeshPro>();
        leftTextMeshPro.text = random ? leafName : otherLeafName;
        leftTextMeshPro.alignment = TextAlignmentOptions.Right;
        leafController.leftButton = leftButton;

        var rightButton = Instantiate<GameObject>(buttonPrefab);
        var rightTextMeshPro = rightButton.GetComponent<TextMeshPro>();
        rightTextMeshPro.text = random ? otherLeafName : leafName;
        rightTextMeshPro.alignment = TextAlignmentOptions.Left;
        leafController.rightButton = rightButton;

        // Position
        var leafHalfWidth = leaf.GetComponent<LeafController>().GetLeafBounds().size.x / 2.0f;
        var leftButtonWidth = leftButton.GetComponent<TextButtonController>().GetButtonBounds().size.x;
        var rightButtonWidth = rightButton.GetComponent<TextButtonController>().GetButtonBounds().size.x;

        var spawnArea = GetWorldRect(xMarginRatio, yMarginRatio);
        var minX = spawnArea.min.x + leftButtonWidth + leafHalfWidth;
        var maxX = spawnArea.max.x - rightButtonWidth - leafHalfWidth;
        var x = Random.Range(minX, maxX);
        var y = spawnArea.max.y;
        var z = -1.0f;

        leaf.transform.position = new Vector3(x, y, z);
        leftButton.transform.position = new Vector3(x - leafHalfWidth, y, z);
        rightButton.transform.position = new Vector3(x + leafHalfWidth, y, z);
    }

    private static Rect GetWorldRect(float xMarginRatio, float yMarginRatio)
    {
        var minWorld = Camera.main.ScreenToWorldPoint(AspectUtility.screenRect.min);
        var maxWorld = Camera.main.ScreenToWorldPoint(AspectUtility.screenRect.max);
        var xMin = minWorld.x;
        var xMax = maxWorld.x;
        var xMargin = (xMax - xMin) * xMarginRatio;
        xMin += xMargin;
        xMax -= xMargin;
        var yMin = minWorld.y;
        var yMax = maxWorld.y;
        var yMargin = (yMax - yMin) * yMarginRatio;
        yMin += yMargin;
        yMax -= yMargin;
        return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }

    private int GetNextNonRepeatingLeafIndex()
    {
        int index;
        do {
            index = Random.Range(0, leafPrefabInfos.Length);
        } while (index == lastLeafIndex);
        lastLeafIndex = index;
        return index;
    }

    private string GetOtherLeafName(string leafName)
    {
        var otherLeafNames = 
            (from info in leafPrefabInfos
            where info.name != leafName
            select info.name).Distinct();
        var rnd = 
            from name in otherLeafNames
            orderby System.Guid.NewGuid()
            select name;
        return rnd.First();
    }
}