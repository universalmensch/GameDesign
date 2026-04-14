using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private float movementX;
    private float movementY;
    private float movementZ;
    public float speed = 0;
    private int count;
    public TextMeshProUGUI countText;
    public GameObject winText;
    private static readonly Vector3 ResetVector = new Vector3(0.0f, 15.0f, 0.0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.SetActive(false);
    }

    void Update()
    {
        rigidbody.transform.GetPositionAndRotation(out var currentPosition, out _);
        if (currentPosition.y < -30.0f)
        {
            rigidbody.transform.SetPositionAndRotation(ResetVector, Quaternion.identity);
            rigidbody.linearVelocity = Vector3.zero;
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVectorInput = movementValue.Get<Vector2>();
        movementX = movementVectorInput.x;
        movementY = movementVectorInput.y;

        rigidbody.transform.GetPositionAndRotation(out var currentPosition, out _);
        
        if (currentPosition.y < 50.0f)
            movementZ = 
                5.0f;
    }

    void FixedUpdate() {
        Camera cam = Camera.main;

        if (!cam)
        {
            return;
        }
        
        Vector3 input = new Vector3(movementX, 0.0f, movementY);
        
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 forwardMovement = speed * input.z * camForward;
        Vector3 sidewayMovement  = 2 * speed * input.x * camRight;
        
        rigidbody.AddForce(forwardMovement);
        rigidbody.AddForce(sidewayMovement);
        
        if (movementZ > 0)
        {
            rigidbody.AddForce(Vector3.up * movementZ, ForceMode.Impulse);
        }
        
        movementZ = 0.0f;
        movementX = 0.0f;
        movementY = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); 
            winText.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            winText.gameObject.SetActive(true);
        }
    }

    void SetCountText()
    {
        countText.text = "Score: " + count;
        if (count >= 10)
        {
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            winText.SetActive(true);
        }
    }
}
