using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller.Game
{
    public class StartController :  MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Minigame");
        }
    }
}
