using UnityEngine;

public class PlayerVisibilityController
{
    private Vector3 _mousePosition;
    private float _cameraXAxisRotation;

    private float _clampCameraUp = 20f;
    private float _clampCameraDown = -5f;

    private Player _player;
    private Transform _playerTransform;
    private Camera _camera;

    public PlayerVisibilityController(Player player)
    {
        _player = player;
        _playerTransform = player.transform;
        _camera = player.Camera;
    }

    public void Update()
    {
        GetMousePosition();
        RotationBodyYAxis();
    }

    public void LateUpdate() => RotationCameraXAxis();

    private void GetMousePosition() => _mousePosition = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

    private void RotationCameraXAxis()
    {
        float rotateValue = _mousePosition.x * _player.VisibilitySensivity * Time.deltaTime;
        
        _cameraXAxisRotation += rotateValue;
        _cameraXAxisRotation = Mathf.Clamp(_cameraXAxisRotation, _clampCameraDown, _clampCameraUp);

        _camera.transform.localEulerAngles = new Vector3(_cameraXAxisRotation, 0);
    }

    private void RotationBodyYAxis() => _playerTransform.localEulerAngles += new Vector3(0, _mousePosition.y * _player.VisibilitySensivity * Time.deltaTime);
}

