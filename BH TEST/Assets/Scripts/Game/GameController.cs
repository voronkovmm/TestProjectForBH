using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : NetworkBehaviour
{
    [SerializeField] private GameObject _exitMenu;
    [SerializeField] private Button _backToMenuButton;

    private void Start() => _backToMenuButton.onClick.AddListener(BackToMenuButton);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_exitMenu.activeSelf)
            {
                _exitMenu.SetActive(true);
            }
            else
            {
                _exitMenu.SetActive(false);
            }
        }
    }

    private void BackToMenuButton()
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
