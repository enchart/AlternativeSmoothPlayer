using UnityEngine;

namespace AlternativeSmoothPlayer;

public class VgPlayerFix : MonoBehaviour
{
    public float rotateSpeed = 40.0f;
    public float followSpeed = 10.0f;
    public float edgeSpeed = 25.0f;
    public bool isFixEnabled;

    private VGPlayer _player;
    private Transform _rigidbodyTransform;

    private Transform _visual;
    private DelayTracker _visualDelayTracker;

    private GameObject _wrapper;
    private MeshRenderer _wrapperMeshRenderer;
    private MeshFilter _wrapperMeshFilter;

    private void Start()
    {
        _player = GetComponent<VGPlayer>();
        _rigidbodyTransform = _player.Player_Rigidbody.transform;

        _wrapper = _player.transform.Find("Player-Wrapper").gameObject;
        _wrapper.transform.localScale = Vector3.one * 0.75f;

        _wrapperMeshRenderer = _wrapper.AddComponent<MeshRenderer>();
        _wrapperMeshRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));
        _wrapperMeshRenderer.enabled = false;

        _wrapperMeshFilter = _wrapper.AddComponent<MeshFilter>();
        _wrapperMeshFilter.mesh = _player.transform.Find("Player/core").GetComponent<MeshFilter>().mesh;

        _visual = _player.transform.Find("Player");
        _visualDelayTracker = _visual.GetComponent<DelayTracker>();
        _visualDelayTracker.enabled = !isFixEnabled;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _wrapperMeshRenderer.enabled = !_wrapperMeshRenderer.enabled;
            _player.Player_Text.DisplayText($"Render <b>{(_wrapperMeshRenderer.enabled ? "enabled" : "disabled")}</b>", 1f);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isFixEnabled = !isFixEnabled;
            _player.Player_Text.DisplayText($"Fix <b>{(isFixEnabled ? "enabled" : "disabled")}</b>", 1f);
            _visualDelayTracker.enabled = !isFixEnabled;
        }

        if (isFixEnabled)
        {
            var positionDelta = (Vector3)_player.internalVelocity;
            var rotationDelta = Quaternion.Euler(0f, 0f, _rigidbodyTransform.eulerAngles.z);

            _visual.position += positionDelta * Time.deltaTime;
            _visual.rotation = Quaternion.Lerp(_visual.rotation, rotationDelta, Time.deltaTime * rotateSpeed);
        }
    }

    private void LateUpdate()
    {
        if (isFixEnabled)
        {
            _visual.position = Vector3.Lerp(_visual.position, _rigidbodyTransform.position, Time.deltaTime * followSpeed);

            var vector = _player.ObjectCamera.WorldToViewportPoint(_visual.position);
            var edgeOffset = VGPlayer.EDGE_OFFSET;
            var num = (float)Screen.height / Screen.width * edgeOffset;

            if (vector.x < num || vector.x > 1f - num || vector.y < edgeOffset || vector.y > 1f - edgeOffset)
            {
                vector.x = Mathf.Clamp(vector.x, num, 1f - num);
                vector.y = Mathf.Clamp(vector.y, edgeOffset, 1f - edgeOffset);
                _visual.position = Vector3.Lerp(_visual.position, _player.ObjectCamera.ViewportToWorldPoint(vector),
                    Time.deltaTime * edgeSpeed);
            }
        }
    }
}