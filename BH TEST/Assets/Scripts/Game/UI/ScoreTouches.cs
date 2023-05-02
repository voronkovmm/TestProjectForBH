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
    public const string CLIENT_NAME = "������";
    public const string SERVER_NAME = "������";

    private bool _isGameOver;

    [SyncVar(hook = nameof(OnSetScoreHost))]
    private int _scoreHostPlayer;
    [SyncVar(hook = nameof(OnSetScoreClient))]
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

    private void OnSetScoreHost(int oldValue, int newValue) => _scoreHostPlayer = newValue;

    private void OnSetScoreClient(int oldValue, int newValue) => _scoreClientPlayer = newValue;

    private IEnumerator ShowWinWindowAndRestartLvl(string nameOfWinner)
    {
        int timeBeforeToRestartLvl = _timeToRestartLvl;

        string text = $"{nameOfWinner} �������! ������� �������� ����� {timeBeforeToRestartLvl}";

        _tmpScore.text = text;

        while (true)
        {
            yield return new WaitForSeconds(1);

            timeBeforeToRestartLvl--;

            if(timeBeforeToRestartLvl == 0)
            {
                NetworkManager.singleton.ServerChangeScene(SceneNames.GAME);
            }

            _tmpScore.text = $"{nameOfWinner} �������! ������� �������� ����� {timeBeforeToRestartLvl}";
        }
    }
}
