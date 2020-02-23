using FreeMindRekru.Core;
using UnityEngine;

namespace FreeMindRekru.Lines
{
    public class LinesGenerator : LineCreator
    {
        GameManager gameManager;
        LineTimer lineTimer;

        protected override void OnAwake()
        {
            base.OnAwake();
            bezierPoints = new Vector3[4];
            lineTimer = GetComponent<LineTimer>();
            gameManager = FindObjectOfType<GameManager>();
            gameManager.onGameStarted += GenerateNewLine;
            gameManager.onPointGain += GenerateNewLine;
            gameManager.onLooseLife += GenerateNewLine;
        }

        public void GenerateNewLine()
        {
            DestroyLine();
            if (gameManager.isPlaying)
            {
                GenerateRandomBezierPoints();
                SortPoints(GetFurthestPointIndex());
                DrawBezierLine();
                float lineLength = GetLineLength();
                gameManager.ResetTimer(lineLength);
                lineTimer.StartTimer(linePositions.ToArray(), lineLength);
            }
        }
    }
}