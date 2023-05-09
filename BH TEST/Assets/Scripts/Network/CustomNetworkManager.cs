using Mirror;
using System;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public static CustomNetworkManager Singleton { get; private set; }

    #region Events

    public static event Action Event_ClientDisconnect;
    public static event Action Event_ClientConnect;
    public static event Action Event_ServerChangeScene;

    #endregion

    #region Dependencies

    private GettindRandomPositionForPlayer _randomPositionService;
    public ListPlayers _listPlayers;

    #endregion

    public override void Awake()
    {
        base.Awake();

        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _randomPositionService = new GettindRandomPositionForPlayer();

        _listPlayers = new();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Event_ClientDisconnect?.Invoke();
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Event_ClientConnect?.Invoke();
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);
        Event_ServerChangeScene += _listPlayers.OnServerChangeSceneHandler_ClearList;
        Event_ServerChangeScene?.Invoke();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform randomPosition = _randomPositionService.GetRandomPosition();
        
        GameObject player = randomPosition is not null
            ? Instantiate(playerPrefab, randomPosition.position, randomPosition.rotation)
            : Instantiate(playerPrefab);

        string name = $"Игрок_{numPlayers}";
        player.name = name;
        
        NetworkServer.AddPlayerForConnection(conn, player);

        Player playerComponent = player.GetComponent<Player>()
            .With(x => x.Name = name);
        _listPlayers.Players.Add(playerComponent);
    }
}