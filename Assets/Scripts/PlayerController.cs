using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly Vector3 ResetVector = new(0.0f, 15.0f, 0.0f);
    private const float Speed = 100f;
    private Camera _cam;
    private Rigidbody _rigidbody;
    
    private float _movementX;
    private float _movementY;
    private float _movementZ;
    
    public GameController gameController;
    
    protected Player ControlledPlayer;

    protected virtual void Start()
    {
        _cam = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        HandlePosition();
    }

    private void HandlePosition()
    {
        _rigidbody.transform.GetPositionAndRotation(out var currentPosition, out _);
        
        if (!(currentPosition.y < -30.0f)) return;
        
        _rigidbody.transform.SetPositionAndRotation(ResetVector, Quaternion.identity);
        _rigidbody.linearVelocity = Vector3.zero;
    }
    
    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVectorInput = movementValue.Get<Vector2>();
        _movementX = movementVectorInput.x;
        _movementY = movementVectorInput.y;
    }

    private void OnJump() {
        _movementZ = 25.0f;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Pickup")) return;
        
        other.gameObject.SetActive(false);
        gameController.ItemCollected(ControlledPlayer);
    }
    
    private void FixedUpdate()
    {
        if (!_cam) return;
        
        HandleMovement();
        HandleJump();
        ResetMovementInput();
    }

    private void ResetMovementInput()
    {
        _movementZ = 0.0f;
        _movementX = 0.0f;
        _movementY = 0.0f;
    }

    private void HandleJump()
    {
        if (_movementZ > 0)
        {
            _rigidbody.AddForce(Vector3.up * _movementZ, ForceMode.Impulse);
        }
    }

    private void HandleMovement()
    {
        Vector3 camForward = _cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();
        
        Vector3 camRight = _cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 input = new Vector3(_movementX, 0.0f, _movementY);
        Vector3 forwardMovement = Speed * input.z * camForward;
        Vector3 sidewayMovement  = 2 * Speed * input.x * camRight;
        
        _rigidbody.AddForce(forwardMovement);
        _rigidbody.AddForce(sidewayMovement);
    }
}
