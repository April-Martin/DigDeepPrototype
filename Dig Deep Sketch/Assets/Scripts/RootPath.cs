using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RootPath : MonoBehaviour {

    private LineRenderer _lr;
    private Vector3 _endPoint;
    private int _lastAngle;

    private Vector3[] _potentialPoints;

	// Use this for initialization
	void Awake () 
    {
        _lr = GetComponent<LineRenderer>();
        _lr.startWidth = .2f;
        _potentialPoints = new Vector3[3];
	}
	
	public void SetRootPoint( Vector3 point )
    {
        _lr.numPositions = 1;
        _lr.SetPosition( 0, point );
        _endPoint = point;
    }

    public void ExtendRoot()
    {
        AddPointAtAngle(_lastAngle);
    }

    public void OnRootSelected()
    {
        _lr.startColor = _lr.endColor = new Color(.5f, .8f, .2f);    }

    public void OnRootDeselected()
    {
        _lr.startColor = _lr.endColor = new Color(1, 1, 1);
    }

    public Vector3[] GetPotentialPoints()
    {
        return _potentialPoints;
    }

    public void AddPointAtAngle( int degrees )
    {
        Vector3 newPoint = GetPointAtAngle(degrees);
        _lr.numPositions++;
        _lr.SetPosition( _lr.numPositions - 1, newPoint);

        _endPoint = newPoint;
        _lastAngle = degrees;

        _potentialPoints[0] = GetPointAtAngle(degrees);
        _potentialPoints[1] = GetPointAtAngle((degrees + 60) % 360);
        _potentialPoints[2] = GetPointAtAngle((degrees - 60) % 360);
    }

    public void AddPotentialPoint( int index )
    {
        if (index == 0)
            AddPointAtAngle(_lastAngle);
        else if (index == 1)
            AddPointAtAngle((_lastAngle + 60) % 360);
        else
            AddPointAtAngle((_lastAngle - 60) % 360);
    }

    public Vector3 GetPointAtAngle( int degrees )
    {
        Vector3 newPoint = _endPoint;
        newPoint.x += Mathf.Cos(rad(degrees)) * GameConstants.UNIT_LENGTH;
        newPoint.y += Mathf.Sin(rad(degrees)) * GameConstants.UNIT_LENGTH;
        return newPoint;
    }

    private float rad( int degrees )
    {
        return degrees * Mathf.PI / 180;
    }
}
