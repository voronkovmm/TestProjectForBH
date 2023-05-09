using Mirror;
using System;
using UnityEngine;

public class PlayerColorManagementWhenTouch : NetworkBehaviour
{
    [SyncVar(hook = "OnIsTouch")]
    private bool IsWasTouched;
    private double _timerColorReturn = 0;

    private Player _player;
    private Renderer _playerRenderer;
    [SerializeField] private Material _playerNormalMaterial;
    [SerializeField] private Material _playerDashTouchMaterial;

    public static event Action<string> OnPlayerWasTouched;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerRenderer = GetComponent<Renderer>();
    }

    private void Update() => CooldownAfterTouch();

    private void OnControllerColliderHit(ControllerColliderHit hit) => TryTouchPlayerInDash(hit);

    private void CooldownAfterTouch()
    {
        if (!isLocalPlayer) return;

        if (_timerColorReturn > 0)
        {
            if (_timerColorReturn < NetworkTime.localTime)
            {
                CmdReturnColorGreen();
            }
        }
    }
    
    private void TryTouchPlayerInDash(ControllerColliderHit hit)
    {
        if (_player.PlayerController.MovementController.IsDash && hit.collider.TryGetComponent(out PlayerColorManagementWhenTouch target))
            CmdTouchPlayer(target, _player.Name);
    }

    [Command]
    private void CmdTouchPlayer(PlayerColorManagementWhenTouch target, string nameWhoTouched)
    {
        if (target.IsWasTouched) return;
        
        OnPlayerWasTouched?.Invoke(nameWhoTouched);

        target.IsWasTouched = true;
    }

    private void OnIsTouch(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            _playerRenderer.material = _playerDashTouchMaterial;
            
            _timerColorReturn = _player.DurationColorChangeWhenTouchedDash + NetworkTime.localTime;
        }
        else
        {
            _playerRenderer.material = _playerNormalMaterial;

            _timerColorReturn = 0;
        }
    }

    [Command]
    private void CmdReturnColorGreen() => IsWasTouched = false;
}