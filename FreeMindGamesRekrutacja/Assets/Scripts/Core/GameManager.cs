using System;
using UnityEngine;

namespace FreeMindRekru.Core
{
    public class GameManager : MonoBehaviour
    {
        public float timeToDraw = 1f;
        public int startingLives = 3;

        public int score { get; private set; } = 0;
        public int livesLeft { get; private set; } = 3;
        public float timeLeft { get; private set; } = 0;
        public bool isPlaying { get; private set; } = false;

        public delegate void OnGameOver(int score);
        public event OnGameOver onGameOver;
        public event Action onGameStarted;
        public event Action onPointGain;
        public event Action onTimeEnd;
        public event Action onLooseLife;

        private void Update()
        {
            if (isPlaying)
            {
                timeLeft -= Time.deltaTime;

                if (timeLeft <= 0)
                {
                    timeLeft = 0;
                    onTimeEnd();
                }
            }
        }

        public void StartGame()
        {
            isPlaying = true;
            ResetTimer();
            livesLeft = startingLives;
            score = 0;
            if (onGameStarted != null)
            {
                onGameStarted();
            }
        }

        public void AddPoint()
        {
            score++;
            onPointGain();
            ResetTimer();
        }

        public void LooseLife()
        {
            livesLeft--;
            onLooseLife();
            if (livesLeft <= 0)
            {
                GameOver();
            }
            else
            {
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            timeLeft = timeToDraw;
        }

        public void GameOver()
        {
            if (onGameOver != null)
            {
                onGameOver(score);
            }
        }
    }
}