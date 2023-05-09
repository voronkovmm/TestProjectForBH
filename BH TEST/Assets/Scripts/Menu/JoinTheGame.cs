using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Mirror;
using TMPro;

class JoinTheGame : MonoBehaviour
{
    [SerializeField] private Button _joinGameButtonAfterInputIp;
    [SerializeField] private Button _joinGameButton;
    [SerializeField] private TMP_InputField _ipInputField;

    [SerializeField] private GameObject _messageFailedToJoin;

    private void Start() => _joinGameButtonAfterInputIp.onClick.AddListener(JoinGameButton);

    private void OnEnable()
    {
        CustomNetworkManager.Event_ClientDisconnect += OnFailedToConnect;
        CustomNetworkManager.Event_ClientConnect += OnSuccessToConnect;
    }
    private void OnDisable()
    {
        CustomNetworkManager.Event_ClientDisconnect -= OnFailedToConnect;
        CustomNetworkManager.Event_ClientConnect -= OnSuccessToConnect;
    }

    private void JoinGameButton()
    {
        _joinGameButton.enabled = false;

        string ip = _ipInputField.text;
        if (ip.Length == 0)
            NetworkManager.singleton.networkAddress = "localhost";
        else
            NetworkManager.singleton.networkAddress = ip;

        NetworkManager.singleton.StartClient();
    }

    private void OnFailedToConnect() => StartCoroutine(ShowMessageFailedToJoinGame());

    private void OnSuccessToConnect() => _joinGameButton.enabled = true;

    private IEnumerator ShowMessageFailedToJoinGame()
    {
        _messageFailedToJoin.SetActive(true);

        float timer = 2.5f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            yield return null;
        }

        _joinGameButton.enabled = true;
        _messageFailedToJoin.SetActive(false);
    }
}
