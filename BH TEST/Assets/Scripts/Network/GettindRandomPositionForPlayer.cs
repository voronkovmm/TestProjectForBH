using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Mirror;

public class GettindRandomPositionForPlayer
{
    public bool isStartNewScene;

    private List<int> existingPositionIndexes = new(4);

    public GettindRandomPositionForPlayer()
    {
        CustomNetworkManager.Event_ServerChangeScene += EventHandler_ServerChangeScene;
    }

    ~GettindRandomPositionForPlayer()
    {
        CustomNetworkManager.Event_ServerChangeScene -= EventHandler_ServerChangeScene;
    }

    public Transform GetRandomPosition()
    {
        Transform returnPosition = null;

        if (isStartNewScene)
        {
            isStartNewScene = false;
            existingPositionIndexes.Clear();
        }

        int randomIndexPosition = Random.Range(0, NetworkManager.startPositions.Count);

        if (existingPositionIndexes.Count > 0)
        {
            for (int i = 0, maxIteration = 0; i < existingPositionIndexes.Count;)
            {
                if (existingPositionIndexes[i] == randomIndexPosition)
                {
                    randomIndexPosition++;
                    if (randomIndexPosition == NetworkManager.startPositions.Count) randomIndexPosition = 0;
                    i = 0;
                    continue;
                }

                i++;
                maxIteration++;
                if (maxIteration >= NetworkManager.singleton.maxConnections * 3) break;
            }
        }

        existingPositionIndexes.Add(randomIndexPosition);
        returnPosition = NetworkManager.startPositions[randomIndexPosition];

        return returnPosition;
    }

    [ClientRpc]
    private void EventHandler_ServerChangeScene()
    {
        isStartNewScene = true;
        existingPositionIndexes.Clear();
    }
}
