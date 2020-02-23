using System.Collections.Generic;
using UnityEngine;

namespace FreeMindRekru.Lines
{
    public class LineCreator : MonoBehaviour
    {
        [SerializeField] GameObject linePrefab = null;

        public Vector3[] bezierPoints { get; set; }
        protected List<Vector3> linePositions;
        private GameObject currentLine;

        private LineRenderer lineRenderer;
        private Camera mainCam;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            mainCam = Camera.main;
            linePositions = new List<Vector3>();
        }

        public Vector3[] GetLinePoints()
        {
            return linePositions.ToArray();
        }

        protected RaycastHit[] GetRayHits(Vector3 rayPosition)
        {
            Ray ray = mainCam.ScreenPointToRay(rayPosition);
            RaycastHit[] rayHits = Physics.RaycastAll(ray, 10);
            return rayHits;
        }

        protected bool CanDraw(RaycastHit[] rayHits)
        {
            foreach (RaycastHit rayHit in rayHits)
            {
                if (rayHit.collider != null && rayHit.collider.CompareTag("MassageArea"))
                {
                    return true;
                }
            }
            return false;
        }

        protected Vector3 rayHitToWorldPosition(RaycastHit[] rayHits)
        {
            Vector3 worldPosition = new Vector3(0, 0, 0);
            foreach (RaycastHit rayHit in rayHits)
            {
                if (rayHit.collider.CompareTag("MassageView"))
                {
                    worldPosition = rayHit.point;
                }
            }
            return worldPosition;
        }

        protected void CreateLine(Vector3 position)
        {
            currentLine = Instantiate(linePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            linePositions.Clear();
            linePositions.Add(position);
            lineRenderer.SetPosition(0, linePositions[0]);
            lineRenderer.SetPosition(1, linePositions[0]);
        }

        protected void UpdateLine(Vector3 newPosition)
        {
            linePositions.Add(newPosition);
            if (lineRenderer.GetPosition(0) != lineRenderer.GetPosition(1))
            {
                lineRenderer.positionCount++;
            }
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);
        }

        protected float GetLineLength()
        {
            float lineLength = 0;
            for(int i=1; i < linePositions.Count; i++)
            {
                lineLength += Vector3.Distance(linePositions[i], linePositions[i - 1]);
            }
            return lineLength;
        }

        public void DestroyLine()
        {
            if (currentLine != null)
            {
                Destroy(currentLine);
            }
        }

        protected void GenerateRandomBezierPoints()
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

        protected int GetFurthestPointIndex()
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

        protected void SortPoints(int furthestPointIndex)
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

        protected void SwapPoints(int i, int j)
        {
            Vector3 tmp = bezierPoints[i];
            bezierPoints[i] = bezierPoints[j];
            bezierPoints[j] = tmp;
        }

        protected void DrawBezierLine()
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