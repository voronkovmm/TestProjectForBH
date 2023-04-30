using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreTouches : NetworkBehaviour
{
    public static ScoreTouches Instance;

    private int _timeToRestartLvl = 5;

    public const int HOST = 0;
    public const int CLIENT = 1;
    public const string CLIENT_NAME = "Клиент";
    public const string SERVER_NAME = "Сервер";

    private bool _isGameOver;

    private int _scoreHostPlayer;
    private int _scoreClientPlayer;

    [SerializeField] private TMP_Text _tmpScore;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start() => CmdUpdateScore();

    private void OnEnable() => PlayerColorManagementWhenTouch.OnPlayerWasTouched += RpcAddScore;

    private void OnDisable() => PlayerColorManagementWhenTouch.OnPlayerWasTouched -= RpcAddScore;

    [Server]
    [ClientRpc]
    private void RpcAddScore(int playerWhoWasTouched)
    {
        if (_isGameOver) return;

        switch (playerWhoWasTouched)
        {
            case HOST:
                _scoreClientPlayer++; break;
            case CLIENT:
                _scoreHostPlayer++; break;
        }

        _tmpScore.text = $"{SERVER_NAME} {_scoreHostPlayer} : {_scoreClientPlayer} {CLIENT_NAME}";

        if(_scoreClientPlayer == 3 || _scoreHostPlayer == 3)
        {
            StartCoroutine(ShowWinWindowAndRestartLvl(_scoreClientPlayer == 3 ? CLIENT_NAME : SERVER_NAME));
            _isGameOver = true;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateScore() => RpcUpdateScore();

    [ClientRpc]
    public void RpcUpdateScore() => _tmpScore.text = $"{SERVER_NAME} {_scoreHostPlayer} : {_scoreClientPlayer} {CLIENT_NAME}";

    private IEnumerator ShowWinWindowAndRestartLvl(string nameOfWinner)
    {
        int timeBeforeToRestartLvl = _timeToRestartLvl;

        string text = $"{nameOfWinner} победил! Уровень начнется через {timeBeforeToRestartLvl}";

        _tmpScore.text = text;

        while (true)
        {
            yield return new WaitForSeconds(1);

            timeBeforeToRestartLvl--;

            if(timeBeforeToRestartLvl == 0)
            {
                NetworkManager.singleton.ServerChangeScene(SceneNames.GAME);
            }

            _tmpScore.text = $"{nameOfWinner} победил! Уровень начнется через {timeBeforeToRestartLvl}";
        }
    }
}
