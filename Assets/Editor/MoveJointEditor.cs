// using UnityEditor;
// using UnityEngine;
// using System.Collections.Generic;
//
// [CustomEditor(typeof(movejoint))]
// public class MoveJointEditor : Editor
// {
//     private string inputString = "";
//
//     public override void OnInspectorGUI()
//     {
//         DrawDefaultInspector();
//
//         movejoint script = (movejoint)target;
//
//         inputString = EditorGUILayout.TextArea(inputString, GUILayout.Height(100));
//
//         if (GUILayout.Button("Parse Input String"))
//         {
//             script.angleSequences = ParseInputString(inputString);
//             EditorUtility.SetDirty(script); // Mark the script as dirty to save changes
//         }
//     }
//
//     private List<AngleSequence> ParseInputString(string input)
//     {
//         List<AngleSequen row contains "]" then it is the last row, move to next array
                           //             if (row.Contains("]"))
                           //             {
                           //                 string[] values = row.Replace("[", "").Replace("]", "").Split(new char[] { ' ', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                           //                 float[] angles = System.Array.ConvertAll(values, float.Parse);
                           //                 angleSequences.Add(new AngleSequence { angles = angles });
                           //                 continue;
                           //             } 
                           //             string[] values = row.Replace("[", "").Replace("]", "").Split(new char[] { ' ', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                           //             float[] angles = System.Array.ConvertAll(values, float.Parse);
                           //             angleSequences.Add(new AngleSequence { angles = angles });
                           //         }
                           //
                           //         return angleSequences;ce> angleSequences = new List<AngleSequence>();
//         string[] rows = input.Trim(new char[] { '[', ']' }).Split(new string[] { "], [" }, System.StringSplitOptions.None);
//
//         foreach (string row in rows)
//         {
//             // if a
//     }
// }