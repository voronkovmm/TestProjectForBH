using Mirror;

public class PlayerNetworkController : NetworkBehaviour
{
    private Player _player;

    private void Awake() => _player = GetComponent<Player>();

    public override void OnStartAuthority() => EnableCamera();

    private void EnableCamera() => _player.Camera.enabled = true;
}
