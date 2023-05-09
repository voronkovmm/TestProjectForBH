using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ScoreTouches : NetworkBehaviour
{
    private const int POINTS_TO_WIN = 3;
    private int _timeToRestartLvl = 5;
    private bool _isGameOver;    
    private readonly SyncDictionary<string, int> _scorePlayers = new();
    private StringBuilder _sb = new();
    [SerializeField] private TMP_Text _tmpScore;

    private void Start()
    {
        CmdFillPlayers();
        CmdUpdateScore();
    }

    private void OnEnable() => PlayerColorManagementWhenTouch.OnPlayerWasTouched += CmdAddScore;

    private void OnDisable() => PlayerColorManagementWhenTouch.OnPlayerWasTouched -= CmdAddScore;

    [Command(requiresAuthority = false)]
    private void CmdFillPlayers()
    {
        List<Player> players = CustomNetworkManager.Singleton._listPlayers.Players;

        for (int i = 0; i < players.Count; i++)
        {
            if (_scorePlayers.ContainsKey(players[i].Name))
                continue;

            _scorePlayers.Add(players[i].Name, 0);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdAddScore(string nameWhoTouched)
    {
        if (_isGameOver) return;

        _scorePlayers[nameWhoTouched] += 1;

        if (_scorePlayers[nameWhoTouched] == POINTS_TO_WIN)
        {
            _isGameOver = true;
            RpcShowTextWictoryAndRestartLvl(nameWhoTouched);
            return;
        }

        CmdUpdateScore();
    }

    [Command(requiresAuthority = false)]
    private void CmdUpdateScore()
    {
        foreach (var player in _scorePlayers)
        {
            _sb.Append($"\n{player.Key} - {player.Value}");
        }

        RpcShowScorePlayers(_sb.ToString());
        _sb.Clear();
    }

    [ClientRpc]
    private void RpcShowScorePlayers(string text) => _tmpScore.text = text;

    [ClientRpc]
    private void RpcShowTextWictoryAndRestartLvl(string nameOfWinner) => StartCoroutine(ShowWinWindowAndRestartLvl(nameOfWinner));

    private IEnumerator ShowWinWindowAndRestartLvl(string nameOfWinner)
    {
        int timeBeforeToRestartLvl = _timeToRestartLvl;

        string text = $"{nameOfWinner} победил! Уровень начнется через {timeBeforeToRestartLvl}";

        _tmpScore.text = text;

        while (true)
        {
            yield return new WaitForSeconds(1);

            timeBeforeToRestartLvl--;

            if (timeBeforeToRestartLvl == 0)
            {
                NetworkManager.singleton.ServerChangeScene(SceneNames.GAME);
            }

            _tmpScore.text = $"{nameOfWinner} победил! Уровень начнется через {timeBeforeToRestartLvl}";
        }
    }
}
