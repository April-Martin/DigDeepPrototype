using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]

public class StageBoundaries : MonoBehaviour 
{
    private LineRenderer lr;
    private EdgeCollider2D edgeCollider;

	// Use this for initialization
	void Start () {

        // Set up points of edge collider
        lr = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        Vector2[] copiedPoints = new Vector2[lr.positionCount];
        for ( int i= 0; i < lr.positionCount; i++ )
        {
            Vector3 point = lr.GetPosition(i);
            copiedPoints[i].x = point.x;
            copiedPoints[i].y = point.y;
        }

        edgeCollider.points = copiedPoints;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
