using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GravityBody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Camera Reference")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    [Header("Ground Detection")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    private Rigidbody _rb;
    private Vector2 _input;
    private bool _isGrounded;

    private readonly int _animSpeed = Animator.StringToHash("Speed");
    private readonly int _animGrounded = Animator.StringToHash("IsGrounded");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _input = Vector2.zero;
        
        if (Input.GetKey(KeyCode.W)) _input.y += 1f;
        if (Input.GetKey(KeyCode.S)) _input.y -= 1f;
        if (Input.GetKey(KeyCode.D)) _input.x += 1f;
        if (Input.GetKey(KeyCode.A)) _input.x -= 1f;

        _input = _input.normalized;
        CheckGrounded();
        UpdateAnimations();
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (_cameraTransform == null) return;

        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;

        camForward = Vector3.ProjectOnPlane(camForward, transform.up).normalized;
        camRight = Vector3.ProjectOnPlane(camRight, transform.up).normalized;

        Vector3 moveDirection = (camRight * _input.x + camForward * _input.y).normalized;

        Vector3 targetPosition = _rb.position + moveDirection * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(targetPosition);

        if (moveDirection != Vector3.zero) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, transform.up);
            _animator.transform.rotation = Quaternion.Slerp(_animator.transform.rotation, targetRotation, 15f * Time.fixedDeltaTime);
        }
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
    }

    private void UpdateAnimations()
    {
        if (_animator == null) return;

        _animator.SetFloat(_animSpeed, _input.magnitude);
        _animator.SetBool(_animGrounded, _isGrounded);
    }
}