using FreeMindRekru.Core;
using System;
using UnityEngine;

namespace FreeMindRekru.Lines
{
    public class LineDrawer : LineCreator
    {
        [SerializeField] float minDistanceBetweenPoints = 0.01f;

        bool isDrawing = false;
        bool canDraw = false;
        public event Action onFinishDrawLine;

        GameManager gameManager;

        protected override void OnAwake()
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.onGameStarted += GameStarted;
            gameManager.onGameOver += GameOver;
            gameManager.onTimeEnd += FinishDrawLine;
            gameManager.onPointGain += DestroyLine;
            gameManager.onLooseLife += DestroyLine;
        }

        private void Update()
        {
            if (canDraw)
            {
                TryDrawLine();
            }
        }

        private void GameStarted()
        {
            canDraw = true;
        }

        private void GameOver(int finalScore)
        {
            canDraw = false;
        }

        private void TryDrawLine()
        {
            RaycastHit[] mouseRayHits = GetRayHits(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                isDrawing = true;
            }
            if (CanDraw(mouseRayHits) && isDrawing)
            {
                DrawLine(mouseRayHits);
            }
            if (Input.GetMouseButtonUp(0) && linePositions.Count != 0)
            {
                FinishDrawLine();
            }
        }

        private void DrawLine(RaycastHit[] mouseRayHits)
        {
            Vector3 mouseWorldPosition = rayHitToWorldPosition(mouseRayHits);
            if (Input.GetMouseButton(0))
            {
                if (!StartedDrawing())
                {
                    CreateLine(mouseWorldPosition);
                }
                else if (DistanceFromPreviousPoint(mouseWorldPosition) > minDistanceBetweenPoints)
                {
                    UpdateLine(mouseWorldPosition);
                }
            }
        }

        private bool StartedDrawing()
        {
            return linePositions.Count > 0;
        }

        private float DistanceFromPreviousPoint(Vector3 toPoint)
        {
            return Vector3.Distance(linePositions[linePositions.Count - 1], toPoint);
        }

        private void FinishDrawLine()
        {
            onFinishDrawLine();
            linePositions.Clear();
            isDrawing = false;
        }
    }
}