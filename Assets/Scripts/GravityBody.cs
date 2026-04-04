using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 10f;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotation; 
    }

    private void FixedUpdate()
    {
        ApplyCustomGravity();
        AlignToGravity();
    }

    private void ApplyCustomGravity()
    {
        if (GravityManager.Instance != null)
        {
            _rb.AddForce(GravityManager.Instance.Gravity, ForceMode.Acceleration);
        }
    }

    private void AlignToGravity()
    {
        if (GravityManager.Instance == null) return;

        Vector3 gravityUp = -GravityManager.Instance.Gravity.normalized;
        Vector3 localUp = transform.up;

        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * _rb.rotation;

        _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime));
    }
}