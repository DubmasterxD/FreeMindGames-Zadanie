using UnityEngine;

namespace FreeMindRekru.Lines.PatternCreator
{
    public class LinesPatternCreatorVisualizer : LineCreator
    {
        [SerializeField] BezierPoint[] bezierPointsObjects = new BezierPoint[4];

        Transform movingPoint;

        protected override void OnAwake()
        {
            base.OnAwake();
            bezierPoints = new Vector3[4];
            GenerateRandomBezierPoints();
            SetBezierPointsObjects();
        }

        private void Update()
        {
            DestroyLine();
            MovePoint();
            UpdateBezierPoints();
            DrawBezierLine();
        }

        private void MovePoint()
        {
            RaycastHit[] mouseRayHits = GetRayHits(Input.mousePosition);
            if (CanDraw(mouseRayHits))
            {
                Transform pointHitTransform = GetPointHitTransform(mouseRayHits);
                if (Input.GetMouseButtonDown(0))
                {
                    AttachMovingPoint(pointHitTransform);
                }
                if (Input.GetMouseButton(0))
                {
                    MoveAttachedPoint(rayHitToWorldPosition(mouseRayHits));
                }
                if (Input.GetMouseButtonUp(0))
                {
                    DeattachMovingPoint();
                }
            }
        }

        private void AttachMovingPoint(Transform pointHitTransform)
        {
            if (pointHitTransform != null)
            {
                movingPoint = pointHitTransform;
            }
        }

        private void MoveAttachedPoint(Vector3 mouseWorldPosition)
        {
            if (movingPoint != null)
            {
                movingPoint.position = mouseWorldPosition;
            }
        }

        private void DeattachMovingPoint()
        {
            movingPoint = null;
        }

        private static Transform GetPointHitTransform(RaycastHit[] mouseRayHits)
        {
            Transform pointHitTransform = null;
            foreach (RaycastHit mouseRayHit in mouseRayHits)
            {
                BezierPoint pointHit = mouseRayHit.transform.GetComponent<BezierPoint>();
                if (pointHit != null)
                {
                    pointHitTransform = pointHit.transform;
                }
            }
            return pointHitTransform;
        }

        private void SetBezierPointsObjects()
        {
            for(int i=0; i < bezierPoints.Length; i++)
            {
                bezierPointsObjects[i].transform.position = bezierPoints[i];
            }
        }

        private void UpdateBezierPoints()
        {
            for(int i=0; i < bezierPoints.Length; i++)
            {
                bezierPoints[i] = bezierPointsObjects[i].transform.position;
            }
        }

        //public void AssignBezierPoints(Vector3[] newPoints)
        //{
        //    points = newPoints;
        //    for(int i=0; i < points.Length; i++)
        //    {
        //        bezierPoints[i].transform.position = points[i];
        //    }
        //}
    }
}