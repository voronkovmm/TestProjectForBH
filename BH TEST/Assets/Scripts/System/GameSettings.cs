using Mirror;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private GameObject _networkManagerMenu;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        EnableNetworkManager();
    }

    private void EnableNetworkManager()
    {
        if(NetworkManager.singleton == null) _networkManagerMenu.SetActive(true);
    }
}
