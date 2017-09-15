using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RootPath : MonoBehaviour {

    private LineRenderer _lr;
    public Vector3 _endPoint { get; private set; }
    public float _lastAngle { get; private set; }

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
        _lr.positionCount = 1;
        _lr.SetPosition( 0, point );
        _endPoint = point;
    }

    private RaycastHit2D CheckForObstacles( float angle )
    {
        Vector2 direction = GetPointAtAngle( angle ) - _endPoint;
        return Physics2D.Raycast(_endPoint, direction, GameConstants.UNIT_LENGTH, LayerMask.GetMask("Obstacles"));
    }

    private RaycastHit2D CheckForObstacles( Vector3 point )
    {
        Vector2 direction = point - _endPoint;
        return Physics2D.Raycast(_endPoint, direction, GameConstants.UNIT_LENGTH, LayerMask.GetMask("Obstacles"));
    }

    public void ExtendRoot()
    {
        AddPointAtAngle (_lastAngle);
    }

    private void BendRoot()
    {
        // Prioritize the points at 60* (more natural looking bends)
        List<Vector3> clearPaths = new List<Vector3>();
        for (int i = 1; i < _potentialPoints.Length; i++)
        {
            if (CheckForObstacles(_potentialPoints[i]).collider == null)
                clearPaths.Add(_potentialPoints[i]);
        }

        if (clearPaths.Count > 0)
        {
            AddPoint(clearPaths[Random.Range(0, clearPaths.Count)]);
        }

        // If a more drastic bend is needed, search for a 120* turn
        else
        {
            Vector2[] points = new Vector2[2];
            points[0] = GetPointAtAngle((_lastAngle + 120) % 360);
            points[1] = GetPointAtAngle((_lastAngle - 120) % 360);
            for (int i = 0; i < 2; i++ )
            {
                if (CheckForObstacles(_potentialPoints[i]).collider == null)
                    clearPaths.Add(_potentialPoints[i]);
            }
            if ( clearPaths.Count > 0)
                AddPoint(clearPaths[Random.Range(0, clearPaths.Count)]);
            else
                Debug.Log("Couldn't find an unobstructed path for root");
        }
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

    public void AddPointAtAngle(float degrees)
    {
        RaycastHit2D collision = CheckForObstacles(degrees);
        if (collision.collider != null)
        {
            BendRoot();
        }
        else
        {
            Vector3 newPoint = GetPointAtAngle(degrees);
            _lr.positionCount++;
            _lr.SetPosition(_lr.positionCount - 1, newPoint);

            _endPoint = newPoint;
            _lastAngle = degrees;

            _potentialPoints[0] = GetPointAtAngle(degrees);
            _potentialPoints[1] = GetPointAtAngle((degrees + 60) % 360);
            _potentialPoints[2] = GetPointAtAngle((degrees - 60) % 360);
        }
    }

    private void AddPoint(Vector3 newPoint)
    {
        float angle = deg( Mathf.Atan2(newPoint.y - _endPoint.y, newPoint.x - _endPoint.x) );
        AddPointAtAngle(angle);
    }

    public void AddPotentialPoint( int index )
    {
        AddPoint(_potentialPoints[index]);
    }
    
    public void AddAllPoints(Vector3[] newPoints, float lastAngle)
    {
        if (newPoints != null)
        {
            _lr.positionCount = newPoints.Length;

            _lr.SetPositions(newPoints);

            _endPoint = _lr.GetPosition(_lr.positionCount - 1);
            _lastAngle = lastAngle;
        }
        
    }

    public Vector3 GetPointAtAngle( float degrees )
    {
        Vector3 newPoint = _endPoint;
        newPoint.x += Mathf.Cos(rad(degrees)) * GameConstants.UNIT_LENGTH;
        newPoint.y += Mathf.Sin(rad(degrees)) * GameConstants.UNIT_LENGTH;
        return newPoint;
    }

    public Vector3[] GetLRPositions ()
    {
        Vector3[] positions = new Vector3[_lr.positionCount];
        _lr.GetPositions(positions);
        return positions;
    }

    private float rad( float degrees )
    {
        return degrees * Mathf.PI / 180;
    }

    private float deg( float radians )
    {
        return radians * 180 / Mathf.PI;
    }
}
