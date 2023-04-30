using Mirror;
using System;

public class NetworkManagerMenu : NetworkManager
{
    public static event Action OnClientDisconnected;
    public static event Action OnClientConnected;

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
}