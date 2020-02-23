using System;
using UnityEngine;

namespace FreeMindRekru.Core
{
    public class GameManager : MonoBehaviour
    {
        public float timeTothink = .2f;
        public float baseTimeToDraw = 1f;
        public float baseLineLength = 3f;
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
                    onTimeEnd();
                }
            }
        }

        public void StartGame()
        {
            isPlaying = true;
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
        }

        public void LooseLife()
        {
            livesLeft--;
            if (livesLeft <= 0)
            {
                GameOver();
            }
            onLooseLife();
        }

        public void ResetTimer(float lineLength)
        {
            timeLeft = timeTothink + baseTimeToDraw * lineLength / baseLineLength;
        }

        public void GameOver()
        {
            isPlaying = false;
            if (onGameOver != null)
            {
                onGameOver(score);
            }
        }
    }
}