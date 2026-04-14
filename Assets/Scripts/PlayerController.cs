using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _movementX;
    private float _movementY;
    private float _movementZ;
    private float _timeLeft = 15f;
    private int _count;
    private TextMeshProUGUI _resultText;
    private bool _isGameOver = false;
    
    private static readonly Vector3 ResetVector = new(0.0f, 15.0f, 0.0f);
    
    public float speed;

    public TextMeshProUGUI countText;
    public TextMeshProUGUI timerText;
        
    public GameObject result;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _count = 0;
        SetCount(_count.ToString());
        
        result.SetActive(false);
        _resultText = result.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        HandleTimer();
        HandleMovement();
    }

    private void HandleMovement()
    {
        _rigidbody.transform.GetPositionAndRotation(out var currentPosition, out _);
        
        if (!(currentPosition.y < -30.0f)) return;
        
        _rigidbody.transform.SetPositionAndRotation(ResetVector, Quaternion.identity);
        _rigidbody.linearVelocity = Vector3.zero;
    }

    private void HandleTimer()
    {
        if (_isGameOver)
            return;
        
        _timeLeft -= Time.deltaTime;
        SetTimer(Mathf.Ceil(_timeLeft).ToString());
        
        if (_timeLeft < 0)
        {
            _isGameOver = true;
            SetResult("You Lose!");
            StartCoroutine(EndGame());
        }
    }
        
    private static IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Start");
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVectorInput = movementValue.Get<Vector2>();
        _movementX = movementVectorInput.x;
        _movementY = movementVectorInput.y;

        _rigidbody.transform.GetPositionAndRotation(out var currentPosition, out _);
        
        if (currentPosition.y < 50.0f)
            _movementZ = 
                5.0f;
    }

    private void FixedUpdate() {
        Camera cam = Camera.main;

        if (!cam)
        {
            return;
        }
        
        Vector3 input = new Vector3(_movementX, 0.0f, _movementY);
        
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 forwardMovement = speed * input.z * camForward;
        Vector3 sidewayMovement  = 2 * speed * input.x * camRight;
        
        _rigidbody.AddForce(forwardMovement);
        _rigidbody.AddForce(sidewayMovement);
        
        if (_movementZ > 0)
        {
            _rigidbody.AddForce(Vector3.up * _movementZ, ForceMode.Impulse);
        }
        
        _movementZ = 0.0f;
        _movementX = 0.0f;
        _movementY = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            ResetTimer();
            _count += 1;
            SetCount(_count.ToString());
            
            if (_count >= 10)
            {
                SetResult("You Win!");
                result.SetActive(true);
            }
        }
    }
    
    private void ResetTimer()
    {
        _timeLeft = 15f;
    }

    private void SetResult(string resultText)
    {
        _resultText.text = resultText;
        result.gameObject.SetActive(true);
    }

    private void SetTimer(string timerValue)
    {
        timerText.text = timerValue;
    }

    private void SetCount(string countValue)
    {
        countText.text = "Score: " + countValue;
    }
}
