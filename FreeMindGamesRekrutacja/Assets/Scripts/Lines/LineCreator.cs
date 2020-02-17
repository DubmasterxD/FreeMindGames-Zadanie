using System.Collections.Generic;
using UnityEngine;

namespace FreeMindRekru.Lines
{
    public class LineCreator : MonoBehaviour
    {
        [SerializeField] GameObject linePrefab = null;

        protected List<Vector3> linePositions;
        private GameObject currentLine;

        private LineRenderer lineRenderer;
        private Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
            linePositions = new List<Vector3>();
            OnAwake();
        }

        protected virtual void OnAwake()
        {

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

        public void DestroyLine()
        {
            if (currentLine != null)
            {
                Destroy(currentLine);
            }
        }
    }
}