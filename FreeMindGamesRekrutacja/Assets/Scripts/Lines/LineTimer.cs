using FreeMindRekru.Core;
using UnityEngine;

namespace FreeMindRekru.Lines
{
    public class LineTimer : LineCreator
    {
        float timeToDraw = 0;
        float lineLength = 0;
        int lastVisitedPointIndex = 0;
        float lastVisitedPointLength = 0;
        Vector3[] modelLinePositions;

        GameManager gameManager;

        protected override void OnAwake()
        {
            base.OnAwake();
            gameManager = FindObjectOfType<GameManager>();
            gameManager.onPointGain += DestroyLine;
            gameManager.onLooseLife += DestroyLine;
        }

        private void Update()
        {
            if (gameManager.isPlaying)
            {
                UpdateLineFromTime(timeToDraw - gameManager.timeLeft);
            }
        }

        public void StartTimer(Vector3[] line, float newLineLength)
        {
            timeToDraw = gameManager.timeLeft;
            modelLinePositions = line;
            lineLength = newLineLength;
            lastVisitedPointIndex = 0;
            lastVisitedPointLength = 0;
            CreateLine(line[0]);
        }

        private void UpdateLineFromTime(float time)
        {
            float targetLength = lineLength * time / timeToDraw;
            float nextPointLength = lastVisitedPointLength + Vector3.Distance(modelLinePositions[lastVisitedPointIndex], modelLinePositions[lastVisitedPointIndex + 1]);
            while (nextPointLength <= targetLength)
            {
                UpdateLine(modelLinePositions[lastVisitedPointIndex]);
                lastVisitedPointIndex++;
                lastVisitedPointLength = nextPointLength;
                nextPointLength += Vector3.Distance(modelLinePositions[lastVisitedPointIndex], modelLinePositions[lastVisitedPointIndex + 1]);
            }
        }
    }
}