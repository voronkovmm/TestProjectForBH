using Mirror;
using System;
using UnityEngine;

public class PlayerColorManagementWhenTouch : NetworkBehaviour
{
    [SyncVar(hook = "OnIsTouch")]
    private bool _iWasTouched;
    private float _timerColorReturn = 0;

    private Player _player;
    private Renderer _playerRenderer;
    [SerializeField] private Material _playerNormalMaterial;
    [SerializeField] private Material _playerDashTouchMaterial;

    public static event Action<int> OnPlayerWasTouched;

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
            _timerColorReturn -= Time.deltaTime;
            if (_timerColorReturn < 0)
            {
                ReturnColorGreen();
            }
        }
    }
    
    private void TryTouchPlayerInDash(ControllerColliderHit hit)
    {
        if (_player.PlayerController.MovementController.IsDash && hit.collider.TryGetComponent(out PlayerColorManagementWhenTouch playerColliderController))
        {
            playerColliderController.ApplyTouchFromPlayer();
        }
    }

    [Command(requiresAuthority = false)]
    private void ApplyTouchFromPlayer()
    {
        if (_iWasTouched) return;
        //DebugText.Instance.Print($"isLocalPlayer {isLocalPlayer}, isServer {isServer}, isClient {isClient}, isClientOnly {isClientOnly}, ", isClientOnly ? "клиент: " : "сервер: ");
        OnPlayerWasTouched?.Invoke(isLocalPlayer ? 0 : 1);

        _iWasTouched = true;
    }

    private void OnIsTouch(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            _playerRenderer.material = _playerDashTouchMaterial;
            _timerColorReturn = _player.DurationColorChangeWhenTouchedDash;
        }
        else
        {
            _playerRenderer.material = _playerNormalMaterial;
        }
    }

    [Command]
    private void ReturnColorGreen() => _iWasTouched = false;
}