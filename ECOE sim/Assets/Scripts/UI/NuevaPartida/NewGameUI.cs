using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NewGameUI : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void ConfirmName()
    {
        string enteredName = nameInput.text;

        if (string.IsNullOrEmpty(enteredName))
        {
            Debug.Log("El nombre no puede estar vacío");
            return;
        }

        GameManager.Instance.playerName = enteredName;
        SceneManager.LoadScene("Partida");
    }
}