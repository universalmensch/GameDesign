using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameController  : MonoBehaviour
    {
        public Player selectedPlayer;
        public PlayerInput playerAInput;
        public PlayerInput playerBInput;
        public PlayerInput playerCInput;
        
        public TextMeshProUGUI countText;
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI resultText;
        
        private int _count;
        private bool _isGameOver;
        private float _timeLeft;

        private void Start()
        {
            selectedPlayer = Player.PlayerA;
            UpdateInputStates();
            
            _count = 0;
            SetCount(_count.ToString());

            _timeLeft = 45f;
            SetTimer(Mathf.Ceil(_timeLeft).ToString(CultureInfo.CurrentCulture));
        }
        
        private void Update()
        {
            HandleTimer();
        }

        private void OnSwitchPlayer()
        {
            selectedPlayer = selectedPlayer switch
            {
                Player.PlayerA => Player.PlayerB,
                Player.PlayerB => Player.PlayerC,
                Player.PlayerC => Player.PlayerA,
                _ => selectedPlayer
            };
            
            UpdateInputStates();
        }
        
        private void HandleTimer()
        {
            if (_isGameOver)
                return;
        
            _timeLeft -= Time.deltaTime;
            SetTimer(Mathf.Ceil(_timeLeft).ToString(CultureInfo.CurrentCulture));
        
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

        public void ItemCollected(Player player)
        {
            switch (player) 
            {
                case Player.PlayerA: HandlePLayerA();
                    break;
                case Player.PlayerB: HandlePLayerB();
                    break;
                case Player.PlayerC: HandlePLayerC();
                    break;
                default:
                    AddTime(1);
                    break;
            }
            
            SetCount(_count.ToString());
            
            if (_count >= 10)
            {
                SetResult("You Win!");
                StartCoroutine(EndGame());
            }
        }
        
        private void UpdateInputStates()
        {
            playerAInput.enabled = selectedPlayer == Player.PlayerA;
            playerBInput.enabled = selectedPlayer == Player.PlayerB;
            playerCInput.enabled = selectedPlayer == Player.PlayerC;
        }

        private void HandlePLayerA()
        {
            _count += 1;
            AddTime(1);
        }

        private void HandlePLayerB()
        {
            _count += 1;
            AddTime(5);
        }

        private void HandlePLayerC()
        {
            _count += 2;
        }
        
        private void AddTime(int factor)
        {
            _timeLeft += 5f *  factor;
        }
        
        private void SetResult(string text)
        {
            resultText.text = text;
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
