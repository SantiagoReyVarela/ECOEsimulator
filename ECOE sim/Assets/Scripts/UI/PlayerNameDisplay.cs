using UnityEngine;
using TMPro;

public class PlayerNameDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            playerNameText.text = GameManager.Instance.playerName;
        }
        else
        {
            playerNameText.text = "Jugador";
        }
    }
}