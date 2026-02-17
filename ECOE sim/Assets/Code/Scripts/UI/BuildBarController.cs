using UnityEngine;
using System.Collections.Generic;
using Code.Scripts.Systems.Levels;

public class BuildBarController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private GameObject buildButtonPrefab;
    [SerializeField] private PlacementSystem placementSystem;
    private Dictionary<int, BuildButtonUI> activeButtons = new Dictionary<int, BuildButtonUI>();

    private void Start()
    {
        if (placementSystem != null)
        {
            placementSystem.OnInventoryUpdated += UpdateButtonState;
        }
    }

    private void OnDestroy()
    {
        if (placementSystem != null)
            placementSystem.OnInventoryUpdated -= UpdateButtonState;
    }

    public void InitializeButtons(List<LevelObjectConfig> levelObjects)
    {
        foreach (Transform child in buttonsContainer)
        {
            Destroy(child.gameObject);
        }
        activeButtons.Clear();

        foreach (var config in levelObjects)
        {
            if (config.objectData == null) continue;

            GameObject newBtn = Instantiate(buildButtonPrefab, buttonsContainer);
            BuildButtonUI uiScript = newBtn.GetComponent<BuildButtonUI>();

            if (uiScript != null)
            {
                uiScript.Initialize(config.objectData, config.amount);
                activeButtons.Add(config.objectData.ID, uiScript);
            }
        }
    }
    private void UpdateButtonState(int id, int newAmount)
    {
        if (activeButtons.ContainsKey(id))
        {
            activeButtons[id].UpdateVisuals(newAmount);
        }
    }
}