using FreeMindRekru.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FreeMindRekru.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] Text score = null;

        GameManager gameManager;

        public void StartGame()
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.StartGame();
            gameManager.onGameOver += GameOver;
            gameObject.SetActive(false);
        }

        public void GameOver(int finalScore)
        {
            gameObject.SetActive(true);
            UpdateScore(finalScore);
        }

        public void UpdateScore(float newScore)
        {
            score.text = newScore.ToString();
        }
    }
}