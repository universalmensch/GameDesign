using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Controller.Game
{
    public class GameController  : MonoBehaviour
    {
        public Entity.Player selectedPlayer;
        public PlayerInput playerAInput;
        public PlayerInput playerBInput;
        public PlayerInput playerCInput;
        
        public TextMeshProUGUI countText;
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI resultText;
        public TextMeshProUGUI jumpText;
        
        private int _count;
        private bool _isGameOver;
        private float _timeLeft;
        
        private int _jumpLeft;
        public int JumpLeft
        {
            get => _jumpLeft;
            set
            {
                if (value > _jumpLeft)
                {
                    _jumpLeft = 0;
                }
                else
                {
                    _jumpLeft -= value;
                }
                
                SetJump(_jumpLeft.ToString());
            }
        }

        private void Start()
        {
            selectedPlayer = Entity.Player.PlayerA;
            UpdateInputStates();
            
            SetResult(String.Empty);
            
            _count = 0;
            SetCount(_count.ToString());

            _timeLeft = 25f;
            SetTimer(Mathf.Ceil(_timeLeft).ToString(CultureInfo.CurrentCulture));
            
            _jumpLeft = 10;
            SetJump(_jumpLeft.ToString());
        }
        
        private void Update()
        {
            HandleTimer();
        }

        private void OnSwitchPlayer()
        {
            selectedPlayer = selectedPlayer switch
            {
                Entity.Player.PlayerA => Entity.Player.PlayerB,
                Entity.Player.PlayerB => Entity.Player.PlayerC,
                Entity.Player.PlayerC => Entity.Player.PlayerA,
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

        public void ItemCollected(Entity.Player player)
        {
            switch (player) 
            {
                case Entity.Player.PlayerA: HandlePLayerA();
                    break;
                case Entity.Player.PlayerB: HandlePLayerB();
                    break;
                case Entity.Player.PlayerC: HandlePLayerC();
                    break;
            }
            
            SetCount(_count.ToString());
            SetJump(_jumpLeft.ToString());

            if (_count < 15) return;
            
            SetResult("You Win!");
            StartCoroutine(EndGame());
        }
        
        private void UpdateInputStates()
        {
            playerAInput.enabled = selectedPlayer == Entity.Player.PlayerA;
            playerBInput.enabled = selectedPlayer == Entity.Player.PlayerB;
            playerCInput.enabled = selectedPlayer == Entity.Player.PlayerC;
        }

        private void HandlePLayerA()
        {
            _count += 3;
            _timeLeft += 7f;
            _jumpLeft += 3;
        }

        private void HandlePLayerB()
        {
            _count += 1;
            _timeLeft += 20f;
            _jumpLeft += 3;
        }

        private void HandlePLayerC()
        {
            _count += 1;
            _timeLeft += 7f;
            _jumpLeft += 10;
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

        private void SetJump(string  jumpValue)
        {
            jumpText.text = "Jumps: " + jumpValue;
        }
    }
}
