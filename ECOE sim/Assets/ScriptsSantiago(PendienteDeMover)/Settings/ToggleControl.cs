using UnityEngine;

public class ToggleControls : MonoBehaviour
{
    public GameObject controlsImage;

    private bool isVisible = false;

    public void Toggle()
    {
        isVisible = !isVisible;
        controlsImage.SetActive(isVisible);

        Debug.Log("Controles visibles: " + isVisible);
    }
}