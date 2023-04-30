using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbiWaitingForPlayers : NetworkBehaviour
{
    private readonly string WAITING_GAME = "Ожидание игроков";
    private readonly string GAME_READY = "Ожидание старта игры";

    #region Components
    [SerializeField] private Button _startGame;
    [SerializeField] private TMP_Text _waitingForPlayersText;
    private GameObject _waitingForPlayersGameObject;
    #endregion

    private void Awake() => _waitingForPlayersGameObject = _waitingForPlayersText.transform.parent.gameObject;

    private void Update()
    {
        if (!isServer) return;

        WaitingForPlayers();
    }

    [Server]
    private void WaitingForPlayers()
    {
        int maxConnections = NetworkManager.singleton.maxConnections;

        if (NetworkManager.singleton.numPlayers == maxConnections)
        {
            if(!_startGame.gameObject.activeSelf)
            {
                _waitingForPlayersGameObject.SetActive(false);
                _startGame.gameObject.SetActive(true);

                RpcNotifyClients(isGameReady: true);
            }
        }
        else
        {
            if (!_waitingForPlayersGameObject.activeSelf)
            {
                _waitingForPlayersGameObject.SetActive(true);
                _startGame.gameObject.SetActive(false);

                RpcNotifyClients(isGameReady: false);
            }
        }
    }

    [ClientRpc]
    private void RpcNotifyClients(bool isGameReady)
    {
        if (isClientOnly)
        {
            if (isGameReady)
                _waitingForPlayersText.text = GAME_READY;
            else
                _waitingForPlayersText.text = WAITING_GAME;
        }
    }
}