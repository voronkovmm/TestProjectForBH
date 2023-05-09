using System.Collections.Generic;

public class ListPlayers
{
    public List<Player> Players = new();

    public void OnServerChangeSceneHandler_ClearList()
    {
        Players.Clear();
        CustomNetworkManager.Event_ServerChangeScene -= OnServerChangeSceneHandler_ClearList;
    }
}
