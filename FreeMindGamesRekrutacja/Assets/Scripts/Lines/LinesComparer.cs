using FreeMindRekru.Core;
using UnityEngine;

namespace FreeMindRekru.Lines
{
    [RequireComponent(typeof(LineDrawer))]
    public class LinesComparer : MonoBehaviour
    {
        [SerializeField] float maxDistance = 4;
        [SerializeField] float maxAverageDistance = .5f;
        [SerializeField] LineCreator lineModeler = null;
        
        LineDrawer lineDrawer;
        GameManager gameManager;

        private void Awake()
        {
            lineDrawer = GetComponent<LineDrawer>();
            gameManager = FindObjectOfType<GameManager>();
            lineDrawer.onFinishDrawLine += CompareLines;
        }

        public void CompareLines()
        {
            Vector3[] drawnLinePositions = lineDrawer.GetLinePoints();
            if (drawnLinePositions.Length > 0)
            {
                Vector3[] modelLinePositions = lineModeler.GetLinePoints();
                float averageDistancesBetweenLines = GetAverageDistancesBetweenLines(drawnLinePositions, modelLinePositions);
                if (AreLinesSimilarEnough(drawnLinePositions, modelLinePositions, averageDistancesBetweenLines))
                {
                    gameManager.AddPoint();
                }
                else
                {
                    gameManager.LooseLife();
                }
            }
            else
            {
                gameManager.LooseLife();
            }
        }

        private float GetAverageDistancesBetweenLines(Vector3[] comparingLine, Vector3[] modelLine)
        {
            float[] drawnPointsToGeneratedLineDistances = GetDistancesBetweenLines(comparingLine, modelLine);
            float sum = 0;
            for (int currentDistanceIndex = 0; currentDistanceIndex < drawnPointsToGeneratedLineDistances.Length; currentDistanceIndex++)
            {
                float currentDistance = drawnPointsToGeneratedLineDistances[currentDistanceIndex];
                if (currentDistance > maxDistance)
                {
                    sum += comparingLine.Length * maxAverageDistance; //make sure it's too high to pass
                }
                sum += currentDistance;
            }
            return sum / drawnPointsToGeneratedLineDistances.Length;
        }

        private float[] GetDistancesBetweenLines(Vector3[] comparingLine, Vector3[] modelLine)
        {
            float[] distancesBetweenLines = new float[comparingLine.Length];
            for (int currentPointIndex = 0; currentPointIndex < comparingLine.Length; currentPointIndex++)
            {
                Vector3 comparingPoint = comparingLine[currentPointIndex];
                int closestModelPointIndex = FindClosestModelPointIndex(comparingPoint, modelLine);
                int secondModelPointIndex = FindSecondModelPointIndex(comparingPoint, modelLine, closestModelPointIndex);
                distancesBetweenLines[currentPointIndex] = GetDistanceToLine(comparingPoint, modelLine, closestModelPointIndex, secondModelPointIndex);
            }
            return distancesBetweenLines;
        }

        private static int FindClosestModelPointIndex(Vector3 comparingPoint, Vector3[] modelLine)
        {
            int closestPointIndex = 0;
            float shortestDistance = float.MaxValue;
            for (int currentPointIndex = 0; currentPointIndex < modelLine.Length; currentPointIndex++)
            {
                float distanceFromCurrentPoint = Vector3.Distance(comparingPoint, modelLine[currentPointIndex]);
                if (distanceFromCurrentPoint < shortestDistance)
                {
                    closestPointIndex = currentPointIndex;
                    shortestDistance = distanceFromCurrentPoint;
                }
            }
            return closestPointIndex;
        }

        private static int FindSecondModelPointIndex(Vector3 comparingPoint, Vector3[] modelLine, int closestModelPointIndex)
        {
            int secondModelPointIndex;
            if (closestModelPointIndex == 0)
            {
                secondModelPointIndex = 1;
            }
            else if (closestModelPointIndex == modelLine.Length - 1)
            {
                secondModelPointIndex = modelLine.Length - 2;
            }
            else
            {
                int previousPointIndex = closestModelPointIndex - 1;
                int nextPointIndex = closestModelPointIndex + 1;
                float distanceToPreviousPoint = Vector3.Distance(comparingPoint, modelLine[previousPointIndex]);
                float distanceToNextPoint = Vector3.Distance(comparingPoint, modelLine[nextPointIndex]);
                if (distanceToPreviousPoint < distanceToNextPoint)
                {
                    secondModelPointIndex = previousPointIndex;
                }
                else
                {
                    secondModelPointIndex = nextPointIndex;
                }
            }
            return secondModelPointIndex;
        }

        //Doesn't actually return distance but kind of approximation based on basic triangles knowledge
        private static float GetDistanceToLine(Vector3 comparingPoint, Vector3[] modelLine, int closestModelPointIndex, int secondModelPointIndex)
        {
            Vector3 closestModelPoint = modelLine[closestModelPointIndex];
            Vector3 secondModelPoint = modelLine[secondModelPointIndex];
            float modelLineLength = Vector3.Distance(closestModelPoint, secondModelPoint);
            float distanceToClosest = Vector3.Distance(comparingPoint, closestModelPoint);
            float distanceToSecond = Vector3.Distance(comparingPoint, secondModelPoint);
            float approximateDistanceToLine = (distanceToClosest + distanceToSecond) - modelLineLength;
            return approximateDistanceToLine;
        }

        private bool AreLinesSimilarEnough(Vector3[] comparingLine, Vector3[] modelLine, float averageDistancesBetweenLines)
        {
            return averageDistancesBetweenLines <= maxAverageDistance && AreEndsCloseEnough(comparingLine, modelLine);
        }

        private bool AreEndsCloseEnough(Vector3[] comparingLine, Vector3[] modelLine)
        {
            return Vector3.Distance(comparingLine[0], modelLine[0]) <= maxDistance && Vector3.Distance(comparingLine[comparingLine.Length - 1], modelLine[modelLine.Length - 1]) <= maxDistance;
        }
    }
}