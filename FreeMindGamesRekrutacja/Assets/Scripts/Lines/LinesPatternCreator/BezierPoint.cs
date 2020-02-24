using UnityEngine;

namespace FreeMindRekru.Lines.PatternCreator
{
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
}