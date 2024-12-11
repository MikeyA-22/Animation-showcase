using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PathManager))]

public class PathManagerEditor : Editor
{
    [SerializeField] PathManager pathManager;
    [SerializeField] private List<waypoint> ThePath;
    List<int> toDelete;
    
    waypoint selectedPoint = null;
    bool doRepaint = true;

    private void OnSceneGUI()
    {
        ThePath = pathManager.GetPath();
        DrawPath(ThePath);
        
    }

    private void OnEnable()
    {
        pathManager = target as PathManager;
        toDelete = new List<int>();
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        ThePath = pathManager.GetPath();
        
        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        
        DrawGUIForPoints();

        if (GUILayout.Button("Add point to path"))
        {
            pathManager.CreateAddPoint();
        }
        
        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    private void DrawGUIForPoints()
    {
        if (ThePath != null && ThePath.Count > 0)
        {
            for (int i = 0; i < ThePath.Count; i++)
            {
                
                EditorGUILayout.BeginHorizontal();
                waypoint p = ThePath[i];
                    
                Color c = GUI.color;
                if(selectedPoint == p) GUI.color = Color.green;
                
                Vector3 oldPos = p.GetPos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);
                
                if (EditorGUI.EndChangeCheck()) p.SetPos(newPos);

                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i);
                }
                
                GUI.color = c;
                EditorGUILayout.EndHorizontal();
                
            }
            
        }

        if (toDelete.Count > 0)
        {
            foreach (int i in toDelete)
                ThePath.RemoveAt(i);
            toDelete.Clear();
        }
    }

    private void DrawPath(List<waypoint> path)
    {
        if (path == null || path.Count < 2) return;

        // Draw the Catmull-Rom spline
        List<Vector3> splinePath = pathManager.GetCatmullRomPath(0.1f);
        for (int i = 0; i < splinePath.Count - 1; i++)
        {
            Handles.color = Color.grey;
            Handles.DrawLine(splinePath[i], splinePath[i + 1]);
        }

        // Draw the waypoints and connect them with lines
        Handles.color = Color.green;
        for (int i = 0; i < path.Count; i++)
        {
            
            waypoint currentWaypoint = path[i];
            waypoint nextWaypoint = path[(i + 1) % path.Count];


            doRepaint = DrawPoint(currentWaypoint);
            
            // Draw individual waypoint positions
            bool changed = DrawPoint(currentWaypoint);

            // Draw direct line connections between waypoints (not spline-related)
            Handles.color = Color.yellow;
            Handles.DrawLine(currentWaypoint.GetPos(), nextWaypoint.GetPos());
        }
        if(doRepaint)Repaint();
    }



    private void DrawPathLine(waypoint p1, waypoint p2)
    {
        Color c = Handles.color;
        Handles.color = Color.grey;
        Handles.DrawLine(p1.GetPos(), p2.GetPos());
        Handles.color = c;
        
    }

    public bool DrawPoint(waypoint p)
    {
        bool isChanged = false;

        if (selectedPoint == p)
        {
            Color c = Handles.color;
            Handles.color = Color.green;
            
            EditorGUI.BeginChangeCheck();
            Vector3 oldpos = p.GetPos();
            Vector3 newpos = Handles.PositionHandle(oldpos, Quaternion.identity);
            
            float handleSize = HandleUtility.GetHandleSize(newpos);
            Handles.SphereHandleCap(-1, newpos, Quaternion.identity, 0.4f * handleSize, EventType.Repaint);

            if (EditorGUI.EndChangeCheck())
            {
                p.SetPos(newpos);
            }
            
            Handles.color = c;
        }
        else
        {
            Vector3 currPos = p.GetPos();
            float handleSize = HandleUtility.GetHandleSize(currPos);
            if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize, 0.25f * handleSize,
                    Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }
        }

        return isChanged;
    }
        
}
