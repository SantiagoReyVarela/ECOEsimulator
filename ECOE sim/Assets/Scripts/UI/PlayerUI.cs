using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;

    void Start()
    {
        playerNameText.text = GameManager.Instance.playerName;
    }
}