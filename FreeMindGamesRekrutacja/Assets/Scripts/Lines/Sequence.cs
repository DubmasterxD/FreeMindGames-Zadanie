using System.Collections.Generic;
using UnityEngine;

namespace FreeMindRekru.Lines
{
    [CreateAssetMenu(fileName = "Sequence", menuName ="Lines/Sequence", order =1)]
    public class Sequence : ScriptableObject
    {
        public List<Line> lines = new List<Line>();
    }
}