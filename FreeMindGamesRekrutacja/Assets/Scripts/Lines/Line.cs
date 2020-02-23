using UnityEngine;

namespace FreeMindRekru.Lines
{
    [CreateAssetMenu(fileName = "Line",menuName = "Lines/Line", order =0)]
    public class Line : ScriptableObject
    {
        [SerializeField] GameObject linePrefab = null;
        public Vector3[] bezierPoints = new Vector3[4];
    }
}