using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] GameObject linePrefab = null;

    GameObject currentLine;
    List<Vector3> positions;

    LineRenderer lineRenderer;
    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        positions = new List<Vector3>();
    }

    private void Update()
    {
        RaycastHit[] mouseRayHits = GetMouseRayHits();
        if (CanDraw(mouseRayHits))
        {
            Vector3 mouseWorldPosition = rayHitToWorldPosition(mouseRayHits);
            if (Input.GetMouseButtonDown(0))
            {
                CreateLine(mouseWorldPosition);
            }
            if (Input.GetMouseButton(0))
            {
                if (positions.Count > 0 && Vector3.Distance(mouseWorldPosition, positions[positions.Count - 1]) > .01f)
                {
                    UpdateLine(mouseWorldPosition);
                }
            }
        }
    }

    private RaycastHit[] GetMouseRayHits()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hit = Physics.RaycastAll(mouseRay, 10);
        return hit;
    }

    private bool CanDraw(RaycastHit[] mouseRayHits)
    {
        foreach (RaycastHit mouseRayHit in mouseRayHits)
        {
            if (mouseRayHit.collider != null && mouseRayHit.collider.CompareTag("MassageArea"))
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 rayHitToWorldPosition(RaycastHit[] mouseRayHits)
    {
        Vector3 worldPosition = new Vector3(0, 0, 0);
        foreach(RaycastHit mouseRayHit in mouseRayHits)
        {
            if (mouseRayHit.collider.CompareTag("MassageView"))
            {
                worldPosition = mouseRayHit.point;
            }
        }
        return worldPosition;
    }

    private void CreateLine(Vector3 mousePosition)
    {
        currentLine = Instantiate(linePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        positions.Clear();
        positions.Add(mousePosition);
        positions.Add(mousePosition);
        lineRenderer.SetPosition(0, positions[0]);
        lineRenderer.SetPosition(1, positions[1]);
    }

    private void UpdateLine(Vector3 newFingerPosition)
    {
        positions.Add(newFingerPosition);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPosition);
    }
}
