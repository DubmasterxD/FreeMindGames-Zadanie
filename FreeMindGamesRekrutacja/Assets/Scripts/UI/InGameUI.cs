using FreeMindRekru.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FreeMindRekru.UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] Text score = null;
        [SerializeField] Text lives = null;
        [SerializeField] Slider timer = null;

        GameManager gameManager;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            SetTimer(gameManager.timeToDraw);
            gameManager.onGameStarted += GameStarted;
            gameManager.onLooseLife += LooseLife;
            gameManager.onPointGain += AddPoint;
        }

        private void Update()
        {
            UpdateTimer(gameManager.timeLeft);
        }

        private void GameStarted()
        {
            UpdateLives(gameManager.startingLives);
            UpdateScore(0);
        }

        private void LooseLife()
        {
            UpdateLives(gameManager.livesLeft);
        }

        private void AddPoint()
        {
            UpdateScore(gameManager.score);
        }

        public void UpdateScore(float newScore)
        {
            score.text = newScore.ToString();
        }

        public void UpdateLives(float newLives)
        {
            lives.text = newLives.ToString();
        }

        public void SetTimer(float maxTime)
        {
            timer.maxValue = maxTime;
        }

        public void UpdateTimer(float newTime)
        {
            timer.value = newTime;
        }
    }
}