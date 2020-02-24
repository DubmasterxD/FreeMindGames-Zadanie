using UnityEditor;
using UnityEngine;

namespace FreeMindRekru.Lines.PatternCreator
{
#if UNITY_EDITOR
    public class LinesPatternCreatorWindow : EditorWindow
    {
        LinesPatternCreatorVisualizer visualizer;
        Sequence sequence;
        Line line;

        [MenuItem("Window/Lines Pattern Creator")]
        static void OpenWindow()
        {
            LinesPatternCreatorWindow window = (LinesPatternCreatorWindow)GetWindow(typeof(LinesPatternCreatorWindow));
            window.minSize = new Vector2(300, 200);
            window.Show();
        }

        private void OnGUI()
        {
            if (CheckVisualizer())
            {
                sequence = (Sequence)EditorGUILayout.ObjectField("Sequence", sequence, typeof(Sequence), false);
                if (GUILayout.Button("Create new sequence"))
                {
                    CreateNewSequence();
                }                
                if (sequence != null)
                {
                    line = (Line)EditorGUILayout.ObjectField("Line", line, typeof(Line), false);
                    if (GUILayout.Button("Create new line"))
                    {
                        SaveLine();
                        CreateNewLine();
                        AddLineToSequence();
                    }
                    if (line != null)
                    {
                        line.bezierPoints = visualizer.bezierPoints;
                    }
                }
            }
        }

        private bool CheckVisualizer()
        {
            if (visualizer == null)
            {
                visualizer = FindObjectOfType<LinesPatternCreatorVisualizer>();
                if (visualizer == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private void CreateNewSequence()
        {
            sequence = CreateInstance<Sequence>();
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Sequences/Sequence");
            string name = path.Split('/')[2];
            AssetDatabase.CreateFolder("Assets/Sequences", name);
            AssetDatabase.CreateAsset(sequence, path + "/" + name + ".asset");
        }

        private void CreateNewLine()
        {
            line = CreateInstance<Line>();
        }

        private void SaveLine()
        {
            if (line != null)
            {
                string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Sequences/" + sequence.name + "/Line.asset");
                AssetDatabase.CreateAsset(line, path);
            }
        }

        private void AddLineToSequence()
        {
            sequence.lines.Add(line);
        }
    }
#endif
}