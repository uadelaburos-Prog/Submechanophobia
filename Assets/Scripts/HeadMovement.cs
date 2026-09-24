using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    [SerializeField] private bool _enable = true;

    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float _frequency = 10f;
    [SerializeField] private float _toggleSpeed = 3f;
    [SerializeField] private float _returnSpeed = 5f;

    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _cameraHolder;

    private Vector3 _startPos;
    private Rigidbody _rb;
    private PlayerMovement _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GetComponent<PlayerMovement>();
        _startPos = _camera.localPosition;
    }

    private void Update()
    {
        if (!_enable) return;

        if (IsWalking())
            _camera.localPosition = _startPos + FootStepMotion();
        else
            _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, _returnSpeed * Time.deltaTime);

        _camera.LookAt(FocusTarget());
    }

    private bool IsWalking()
    {
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        return flatVel.magnitude >= _toggleSpeed && _player.Grounded;
    }

    private Vector3 FootStepMotion()
    {
        float t = Time.time * _frequency;
        return new Vector3(
            Mathf.Cos(t * 0.5f) * _amplitude * 2f,
            Mathf.Sin(t) * _amplitude,
            0f);
    }

    private Vector3 FocusTarget()
    {
        return _cameraHolder.position + _cameraHolder.forward * 15f;
    }
}