using UnityEngine;

public class GravityCameraRig : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _target; 
    [SerializeField] private Transform _cameraTarget; 

    [Header("Orbit Settings")]
    [SerializeField] private float _defaultDistance = 6f;
    [SerializeField] private float _mouseSensitivity = 3f;
    [SerializeField] private Vector2 _pitchLimit = new Vector2(-40f, 80f);

    [Header("Collision Settings")]
    [SerializeField] private LayerMask _collisionLayers;
    [SerializeField] private float _collisionPadding = 0.2f;
    [SerializeField] private float _cameraRadius = 0.2f;

    [Header("Smoothing")]
    [SerializeField] private float _rotationSmoothSpeed = 15f;
    [SerializeField] private float _collisionSmoothSpeed = 20f;

    private float _yaw;
    private float _pitch;
    private float _currentDistance;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _currentDistance = _defaultDistance;
    }

    private void LateUpdate()
    {
        if (_target == null || _cameraTarget == null) return;

        transform.position = _target.position;

        _yaw += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, _pitchLimit.x, _pitchLimit.y);

        Quaternion orbitRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Quaternion gravityAlignment = Quaternion.FromToRotation(Vector3.up, _target.up);
        Quaternion targetRotation = gravityAlignment * orbitRotation;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSmoothSpeed * Time.deltaTime);

        HandleCameraCollision();
    }

    private void HandleCameraCollision()
    {
        Vector3 desiredLocalPos = new Vector3(0, 0, -_defaultDistance);
        Vector3 desiredWorldPos = transform.TransformPoint(desiredLocalPos);
        Vector3 rayDirection = (desiredWorldPos - transform.position).normalized;

        RaycastHit hit;
        float targetDist = _defaultDistance;

        if (Physics.SphereCast(transform.position, _cameraRadius, rayDirection, out hit, _defaultDistance, _collisionLayers))
        {
            targetDist = hit.distance - _collisionPadding;
        }

        targetDist = Mathf.Max(targetDist, 0.5f);
        _currentDistance = Mathf.Lerp(_currentDistance, targetDist, Time.deltaTime * _collisionSmoothSpeed);
        _cameraTarget.localPosition = new Vector3(0f, 0f, -_currentDistance);
    }
}