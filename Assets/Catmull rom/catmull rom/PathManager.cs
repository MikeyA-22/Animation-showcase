using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] public List<waypoint> path;



    //public GameObject prefab;
    int currentPointIndex = 0;
    
    //public List<Gam    eObject> prefabPoints;

    public List<waypoint> GetPath()
    {
        
        if (path == null)
            path = new List<waypoint>();
        
        
        return path;
    }

    public void CreateAddPoint()
    {
        waypoint go = new waypoint();
        path.Add(go);
    }

    public waypoint GetNextTarget()
    {
        int nextPointIndex = (currentPointIndex+1) % path.Count;
        currentPointIndex = nextPointIndex;
        Debug.Log(currentPointIndex);
        return path[currentPointIndex];
    }

    private void Start()
    {
        //prefabPoints = new List<GameObject>();
        foreach (waypoint p in path)
        {
            //GameObject go = Instantiate(prefab);
            //go.transform.position = p.pos;
            //prefabPoints.Add(go);
        }
    }

    public void Update()
    {
        for (int i = 0; i < path.Count; i++)
        {
            waypoint p = path[i];
            
            
            
            

        }
    }
    
    public List<Vector3> GetCatmullRomPath(float resolution = 0.1f)
    {
        List<Vector3> interpolatedPoints = new List<Vector3>();
        if (path.Count < 2) return interpolatedPoints;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 p0 = path[(i - 1 + path.Count) % path.Count].GetPos();
            Vector3 p1 = path[i].GetPos();
            Vector3 p2 = path[(i + 1) % path.Count].GetPos();
            Vector3 p3 = path[(i + 2) % path.Count].GetPos();

            for (float t = 0; t < 1; t += resolution)
            {
                interpolatedPoints.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }
        return interpolatedPoints;
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

}
