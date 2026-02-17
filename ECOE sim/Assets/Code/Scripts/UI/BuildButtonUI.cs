using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildButtonUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Referencias Componentes Hijos")]
    [SerializeField] private ObjectInfoButton infoButton;

    private int currentID;
    public void Initialize(ObjectData data, int currentAmount)
    {
        currentID = data.ID;
        if (iconImage != null) iconImage.sprite = data.Icon;

        if (infoButton != null)
        {
            infoButton.Initialize(data);
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            var system = FindFirstObjectByType<PlacementSystem>();
            if (system != null)
            {
                system.StartPlacement(currentID);
            }
        });

        UpdateVisuals(currentAmount);
    }

    public void UpdateVisuals(int amount)
    {
        if (amountText != null) amountText.text = amount.ToString();

        if (amount > 0)
        {
            button.interactable = true;
            if (canvasGroup) canvasGroup.alpha = 1f;
        }
        else
        {
            button.interactable = false;
            if (canvasGroup) canvasGroup.alpha = 0.5f;
        }
    }

    public int GetID() => currentID;
}