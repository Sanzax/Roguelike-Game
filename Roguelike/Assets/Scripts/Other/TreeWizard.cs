using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreeWizard : ScriptableWizard
{
    public GameObject prefab;
    public Transform parent;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public float collisionRadius = 1f;
    public float groundDetectingnRadius = .1f;

    public Vector2 regionSize = new Vector2(10, 10);
    public Vector2 regionPosition = new Vector2();
    public float radius = .5f;
    public int rejectionSamples = 10;

    List<Vector2> points;
    List<GameObject> gameobjects = new List<GameObject>();

    bool ready = false;

    [MenuItem("Metropolia/Create Trees")]
    private static void CreateWizard()
    {
        DisplayWizard<TreeWizard>("Tree setup");
    }

    private void OnWizardCreate()
    {
        ready = true;
    }

    private void OnDestroy()
    {
        if(!ready)
        {
            for (int i = 0; i < gameobjects.Count; i++)
            {
                DestroyImmediate(gameobjects[i]);
                gameobjects[i] = null;
            }
        }
    }

    private void OnWizardUpdate()
    {
        if (prefab != null)
        {
            for (int i = 0; i < gameobjects.Count; i++)
            {
                DestroyImmediate(gameobjects[i]);
                gameobjects[i] = null;
            }
            gameobjects = new List<GameObject>();
            points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);

            for (int i = 0; i < points.Count; i++)
            {

                Vector3 goPos = new Vector3(points[i].x - regionSize.x / 2 + regionPosition.x, 0, points[i].y - regionSize.y / 2 + regionPosition.y); ;

                GameObject go = Instantiate(prefab, parent);
                go.transform.position = goPos;
                gameobjects.Add(go);
                
            }
        }
    }
}
