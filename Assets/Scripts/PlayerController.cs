using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
        if (currentPosition.y < -10.0f)
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
        
        if (currentPosition.y < 30.0f)
            movementZ = 1.5f;
    }

    void FixedUpdate() {
        Vector3 movement = new Vector3(movementX, movementZ, movementY);
        rigidbody.AddForce(movement * speed);
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
