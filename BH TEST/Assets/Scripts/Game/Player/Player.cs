using Mirror;
using System.Xml.Serialization;
using Unity.Burst.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : NetworkBehaviour
{
    #region States
    [Header("States")]
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _visibilitySensivity = 100;
    [SerializeField] private float _dashForce = 5;
    [SerializeField] private float _dashDistance = 0.5f;
    [SerializeField] private float _durationColorChangeWhenTouchedDash = 3;
    [SerializeField] private AnimationCurve _dashCurve;

    public float MoveSpeed { get => _moveSpeed; private set => _moveSpeed = value; }
    public float VisibilitySensivity { get => _visibilitySensivity; private set => _visibilitySensivity = value; }
    public float DashForce { get => _dashForce; private set => _dashForce = value; }
    public float DashDistance { get => _dashDistance; private set => _dashDistance = value; }
    public float DurationColorChangeWhenTouchedDash { get => _durationColorChangeWhenTouchedDash; private set => _durationColorChangeWhenTouchedDash = value; }
    public AnimationCurve DashCurve { get => _dashCurve; private set => _dashCurve = value; }

    #endregion

    #region Components
    public CharacterController CharacterController { get; private set; }
    public Camera Camera { get => _camera; private set => _camera = value; }

    [Header("Components")]
    [SerializeField] private Camera _camera;
    #endregion

    #region Dependencies

    public PlayerController PlayerController { get; private set; }

    #endregion


    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();

        PlayerController = new PlayerController(this);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        PlayerController.Update();
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer) return;

        PlayerController.LateUpdate();
    }
}
