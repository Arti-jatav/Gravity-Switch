using UnityEngine;

public class HologramMechanic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _playerHead;
    [SerializeField] private GameObject _hologramPivotPrefab;

    [Header("Mechanic Settings")]
    [SerializeField] private float _gravityDelay = 0.8f;
    [SerializeField] private float _hologramSwingSpeed = 15f;

    private GameObject _hologramInstance;
    private bool _isAnticipating = false;
    private float _timer = 0f;
    private Vector3 _targetGravityDown;

    private void Update()
    {
        Vector3? rawInput = null;

        if (Input.GetKeyDown(KeyCode.UpArrow)) rawInput = _cameraTransform.forward;
        if (Input.GetKeyDown(KeyCode.DownArrow)) rawInput = -_cameraTransform.forward;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) rawInput = -_cameraTransform.right;
        if (Input.GetKeyDown(KeyCode.RightArrow)) rawInput = _cameraTransform.right;

        if (rawInput.HasValue)
        {
            Vector3 snappedDirection = GetSnappedOrthogonalDirection(rawInput.Value);
            SetHologramDirection(snappedDirection);
        }

        if (_isAnticipating)
        {
            UpdateHologramTransform();

            _timer += Time.deltaTime;
            if (_timer >= _gravityDelay)
            {
                ExecuteGravityShift();
            }
        }
    }

    private Vector3 GetSnappedOrthogonalDirection(Vector3 camDir)
    {
        Vector3 flattened = Vector3.ProjectOnPlane(camDir, transform.up).normalized;
        if (flattened == Vector3.zero) flattened = transform.forward;

        Vector3[] orthogonalAxes = new Vector3[]
        {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right
        };

        Vector3 bestAxis = transform.forward;
        float maxDot = -Mathf.Infinity;

        foreach (Vector3 axis in orthogonalAxes)
        {
            float dot = Vector3.Dot(flattened, axis);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestAxis = axis;
            }
        }

        return bestAxis;
    }

    private void SetHologramDirection(Vector3 newDirection)
    {
        _targetGravityDown = newDirection;
        _timer = 0f;
        _isAnticipating = true;

        if (_hologramInstance == null)
        {
            _hologramInstance = Instantiate(_hologramPivotPrefab);
            _hologramInstance.transform.rotation = transform.rotation;
        }
        
        _hologramInstance.SetActive(true);
    }

    private void UpdateHologramTransform()
    {
        if (_hologramInstance == null || _playerHead == null) return;
        
        _hologramInstance.transform.position = _playerHead.position;
        Vector3 targetHologramUp = -_targetGravityDown;
        Vector3 targetHologramForward = transform.up; 
        Quaternion targetRotation = Quaternion.LookRotation(targetHologramForward, targetHologramUp);

        _hologramInstance.transform.rotation = Quaternion.Slerp(
            _hologramInstance.transform.rotation, 
            targetRotation, 
            Time.deltaTime * _hologramSwingSpeed
        );
    }

    private void ExecuteGravityShift()
    {
        _isAnticipating = false;
        if (_hologramInstance != null)
        {
            _hologramInstance.SetActive(false);
        }
        GravityManager.Instance.SetGravityDirection(_targetGravityDown);
    }
}