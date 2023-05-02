using Mirror;
using System;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    #region Events

    public static event Action OnClientDisconnected;
    public static event Action OnClientConnected;

    #endregion

    #region Dependencies

    private GettindRandomPositionForPlayerNetworkService _randomPositionService;

    #endregion

    public override void Awake()
    {
        base.Awake();
        _randomPositionService = new GettindRandomPositionForPlayerNetworkService();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);
        _randomPositionService.isStartNewScene = true;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform randomPosition = _randomPositionService.GetRandomPosition();
        GameObject player = randomPosition is not null
            ? Instantiate(playerPrefab, randomPosition.position, randomPosition.rotation)
            : Instantiate(playerPrefab);

        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
