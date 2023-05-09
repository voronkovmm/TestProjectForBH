using Mirror;
using UnityEngine;

public class CreateGame : MonoBehaviour
{
    public void ButtonHandler_ChoiceCountPlayers(int countPlayers)
    {
        NetworkManager.singleton.maxConnections = countPlayers;
        NetworkManager.singleton.StartHost();
    }
}
