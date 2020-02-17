using FreeMindRekru.Core;
using UnityEngine;

namespace FreeMindRekru.Lines
{
    public class LinesGenerator : LineCreator
    {
        Vector3[] bezierPoints;

        GameManager gameManager;

        protected override void OnAwake()
        {
            gameManager = FindObjectOfType<GameManager>();
            bezierPoints = new Vector3[4];
            gameManager.onGameStarted += GenerateNewLine;
            gameManager.onPointGain += GenerateNewLine;
            gameManager.onLooseLife += GenerateNewLine;
        }

        public void GenerateNewLine()
        {
            DestroyLine();
            GenerateRandomBezierPoints();
            SortPoints(GetFurthestPointIndex());
            DrawBezierLine();
        }

        private void GenerateRandomBezierPoints()
        {
            RaycastHit[] randomRayHits = new RaycastHit[0];
            for (int i = 0; i < 4; i++)
            {
                do
                {
                    Vector3 randomPosition = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0);
                    randomRayHits = GetRayHits(randomPosition);
                } while (!CanDraw(randomRayHits));
                bezierPoints[i] = rayHitToWorldPosition(randomRayHits);
            }
        }

        private int GetFurthestPointIndex()
        {
            float furthestDistance = 0;
            int oneOfFurthestPointsIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    if (Vector3.Distance(bezierPoints[i], bezierPoints[j]) > furthestDistance)
                    {
                        furthestDistance = Vector3.Distance(bezierPoints[i], bezierPoints[j]);
                        oneOfFurthestPointsIndex = i;
                    }
                }
            }
            return oneOfFurthestPointsIndex;
        }

        private void SortPoints(int furthestPointIndex)
        {
            SwapPoints(0, furthestPointIndex);
            for (int i = 1; i < 4; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    if (Vector3.Distance(bezierPoints[0], bezierPoints[i]) > Vector3.Distance(bezierPoints[0], bezierPoints[j]))
                    {
                        SwapPoints(i, j);
                    }
                }
            }
        }

        private void SwapPoints(int i, int j)
        {
            Vector3 tmp = bezierPoints[i];
            bezierPoints[i] = bezierPoints[j];
            bezierPoints[j] = tmp;
        }

        private void DrawBezierLine()
        {
            CreateLine(bezierPoints[0]);
            for (int i = 1; i <= 100; i++)
            {
                Vector3 p01 = Vector3.Lerp(bezierPoints[0], bezierPoints[1], i / 100f);
                Vector3 p12 = Vector3.Lerp(bezierPoints[1], bezierPoints[2], i / 100f);
                Vector3 p23 = Vector3.Lerp(bezierPoints[2], bezierPoints[3], i / 100f);
                Vector3 p0112 = Vector3.Lerp(p01, p12, i / 100f);
                Vector3 p1223 = Vector3.Lerp(p12, p23, i / 100f);
                Vector3 pFinal = Vector3.Lerp(p0112, p1223, i / 100f);
                UpdateLine(pFinal);
            }
        }
    }
}