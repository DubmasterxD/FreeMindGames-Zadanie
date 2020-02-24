using FreeMindRekru.Core;
using System.Collections.Generic;
using UnityEngine;

namespace FreeMindRekru.Lines
{
    public class PatternsManager : LineCreator
    {
        [SerializeField] List<Sequence> sequences = null;
        GameManager gameManager;
        LineTimer lineTimer;

        int currSequenceIndex = 0;
        int currLineIndex = 0;

        protected override void OnAwake()
        {
            base.OnAwake();
            lineTimer = GetComponent<LineTimer>();
            gameManager = FindObjectOfType<GameManager>();
            gameManager.onGameStarted += ShowNextLine;
            gameManager.onPointGain += ShowNextLine;
            gameManager.onLooseLife += ShowNextLine;
        }

        public void ShowNextLine()
        {
            DestroyLine();
            if (gameManager.isPlaying)
            {
                bezierPoints = sequences[currSequenceIndex].lines[currLineIndex].bezierPoints;
                DrawBezierLine();
                float lineLength = GetLineLength();
                gameManager.ResetTimer(lineLength);
                lineTimer.StartTimer(linePositions.ToArray(), lineLength);
                ChangeIndexes();
            }
        }

        private void ChangeIndexes()
        {
            currLineIndex++;
            if (currLineIndex >= sequences[currSequenceIndex].lines.Count)
            {
                currLineIndex = 0;
                currSequenceIndex = Random.Range(0, sequences.Count);
            }
        }
    }
}