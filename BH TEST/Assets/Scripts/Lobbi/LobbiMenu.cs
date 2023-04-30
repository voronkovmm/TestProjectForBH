using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbiMenu : NetworkBehaviour
{
    [SerializeField] private Button _buttonBack;
    [SerializeField] private Button _buttonStartGame;
    [SerializeField] private Button _buttonSoloStartGame;

    private void Start()
    {
        _buttonBack.onClick.AddListener(OnClickButtonBack);
        _buttonStartGame.onClick.AddListener(OnClickStartGame);
        _buttonSoloStartGame.onClick.AddListener(OnClickSoloStartGame);
    }

    private void OnClickSoloStartGame() => NetworkManager.singleton.ServerChangeScene(SceneNames.GAME);

    private void OnClickStartGame() => NetworkManager.singleton.ServerChangeScene(SceneNames.GAME);

    private void OnClickButtonBack()
    {
        if (isClientOnly)
        {
            SceneManager.LoadScene(SceneNames.MENU);
            NetworkManager.singleton.StopClient();
        }
        else
        {           
            NetworkManager.singleton.StopHost();
            NetworkManager.singleton.ServerChangeScene(SceneNames.MENU);
        }
    }
}
