using UnityEngine;
using UnityEngine.UI;

public class ExitFromGame : MonoBehaviour
{
    [SerializeField] private Button _exitButton;

    private void Start() => _exitButton.onClick.AddListener(ExitButton);

    private void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }
}