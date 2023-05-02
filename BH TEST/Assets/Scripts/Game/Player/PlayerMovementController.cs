using UnityEngine;

public class PlayerMovementController
{
    private Vector3 _moveInputUser;
    private bool _isStopMoving;
    public bool IsDash;
    private float _timerDash;

    private Player _player;
    private Transform _playerTransform;
    private CharacterController _characterController;

    public PlayerMovementController(Player player)
    {
        _player = player;
        _playerTransform = player.transform;
        _characterController = player.CharacterController;
    }

    public void Update()
    {
        GetMovementInputUser();
        GetDashInputUser();

        if (!_isStopMoving)
            Move();
        
        if (IsDash)
            Dash();
    }

    private void GetMovementInputUser() => _moveInputUser = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

    private void GetDashInputUser()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isStopMoving = true;
            IsDash = true;

            float timeDash = _player.DashDistance / _player.DashForce;

            Keyframe[] newArrayKeys = _player.DashCurve.keys;
            newArrayKeys[^1].time = timeDash;
            _player.DashCurve.keys = newArrayKeys;

        }
    }

    private void Move()
    {
        Vector3 move = _playerTransform.right * _moveInputUser.x + _playerTransform.forward * _moveInputUser.z;
        move.y += Physics.gravity.y;

        _characterController.Move(move * _player.MoveSpeed * Time.deltaTime);
    }

    private void Dash()
    {
        float timeDashCurve = _player.DashCurve.keys[^1].time;

        if (_timerDash > timeDashCurve)
        {
            _timerDash = 0;
            IsDash = false;
            _isStopMoving = false;
            return;
        }

        _timerDash += Time.deltaTime;
        float currentPositionOnDashCurve = _player.DashCurve.Evaluate(_timerDash);

        Vector3 direction = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z).normalized;

        _characterController.Move(direction * _player.DashForce * currentPositionOnDashCurve * Time.deltaTime);
    }
}