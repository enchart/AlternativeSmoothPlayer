using UnityEngine;

namespace AlternativeSmoothPlayer;

public class VgPlayerFix : MonoBehaviour
{
    private VGPlayer _player;
    private Transform _core;

    private void Start()
    {
        _player = GetComponent<VGPlayer>();
        _core = _player.transform.Find("Player");

        Destroy(transform.Find("Player").GetComponent<DelayTracker>());
    }

    private void Update()
    {
        const float multiplier = 40.0f;
        
        var target = _player.Player_Rigidbody.transform;
        
        // Time.deltaTime or Time.smoothDeltaTime
        _core.position = Vector3.Lerp(_core.position, target.position, Time.smoothDeltaTime * multiplier);
        _core.rotation = Quaternion.Lerp(_core.rotation, Quaternion.Euler(0f, 0f, target.eulerAngles.z), Time.smoothDeltaTime * multiplier);
    }

    private void FixedUpdate()
    {
        if (_player.CanMove)
            _player.Player_Rigidbody.velocity = _player.internalVelocity;
    }
}