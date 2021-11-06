// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StagesDataAuthoring))]
public class StagesEditor : Editor
{
    private readonly float hueShift = Mathf.Sqrt(0.03f);

    protected virtual void OnSceneGUI()
    {
        StagesDataAuthoring d = (StagesDataAuthoring)target;

        if (d.Stages == null)
            return;

        List<List<Vector3>> stages = new List<List<Vector3>>(d.Stages.Count);

        for (int i = 0; i < d.Stages.Count; i++)
        {
            if (d.Stages[i].Destinations == null)
                stages.Add(null);
            else
                stages.Add(new List<Vector3>(d.Stages[i].Destinations.Count));
        }

        
        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < d.Stages.Count; i++)
        {
            if (stages[i] == null)
                continue;

            Handles.color = Color.HSVToRGB(i * hueShift, 1f, 1f);

            for (int j = 0; j < d.Stages[i].Destinations.Count; j++)
            {
                stages[i].Add(Handles.FreeMoveHandle(d.Stages[i].Destinations[j], Quaternion.identity, 0.1f, Vector3.one * 0.1f, Handles.DotHandleCap));
                Handles.DrawAAPolyLine(2f, stages[i].ToArray());
            }

        }
        
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(d, "Change destination");

            for (int i = 0; i < d.Stages.Count; i++)
            {
                if (stages[i] == null)
                    continue;

                for (int j = 0; j < d.Stages[i].Destinations.Count; j++)
                    d.Stages[i].Destinations[j] = stages[i][j];
            }
        }
    }
}
#endif
