using UnityEngine;
using System.Collections;
using UnityEngine.UI;

class MenuJoinHandler : MonoBehaviour
{
    [SerializeField] private Button _joinGameButtonAfterInputIp;
    [SerializeField] private Button _joinGameButton;

    [SerializeField] private GameObject _messageFailedToJoin;

    private void OnEnable()
    {
        NetworkManagerMenu.OnClientDisconnected += OnFailedToConnect;
        NetworkManagerMenu.OnClientConnected += OnSuccessToConnect;
    }
    private void OnDisable()
    {
        NetworkManagerMenu.OnClientDisconnected -= OnFailedToConnect;
        NetworkManagerMenu.OnClientConnected -= OnSuccessToConnect;
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
