using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _createGameButton;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Button _joinButtonAfterInputIp;
    [SerializeField] private Button _exitButton;

    [SerializeField] private TMP_InputField _ipInputField;

    private void Start()
    {
        _createGameButton.onClick.AddListener(CreateGameButton);
        _exitButton.onClick.AddListener(ExitButton);
        _joinButtonAfterInputIp.onClick.AddListener(JoinGameButton);
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    private void CreateGameButton() => NetworkManager.singleton.StartHost();
    private void JoinGameButton()
    {
        _joinButton.enabled = false;

        string ip = _ipInputField.text;
        if (ip.Length == 0)
            NetworkManager.singleton.networkAddress = "localhost";
        else
            NetworkManager.singleton.networkAddress = ip;

        NetworkManager.singleton.StartClient();
    }

    
}