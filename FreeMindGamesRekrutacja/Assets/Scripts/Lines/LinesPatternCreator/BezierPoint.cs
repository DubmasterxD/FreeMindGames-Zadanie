using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        transform.LookAt(mainCam.transform);
    }
}
