using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RootPath : MonoBehaviour {

    public Vector3 Endpoint { get { return _endPoint; } }

    private LineRenderer _lr;
    private Vector3 _endPoint;
    private int _lastAngle;
    private int _selectedPoint;

    /// <summary>
    /// LinkedList of all branches off of this root path
    /// </summary>
    private List<RootPath> _branches;
    private int _lastBranch;

    public bool IsBranching { get { return _branching; } set { _branching = value; } }
    private bool _branching;

    private Vector3[] _potentialPoints;

	// Use this for initialization
	void Awake () 
    {
        _lr = GetComponent<LineRenderer>();
        _lr.startWidth = .2f;
        _potentialPoints = new Vector3[3];
        _branches = new List<RootPath>();
        _branching = false;
	}
	
	public void SetRootPoint( Vector3 point )
    {
        _lr.positionCount = 1;
        _lr.SetPosition( 0, point );
        _endPoint = point;
    }

    public void ExtendRoot()
    {
        //TODO: Add conditionals if branching needs to happen on this rootpath.
        if (_branches.Count > 0)
        {
            foreach(RootPath branch in _branches)
            {
                branch.ExtendRoot();
            }
        }
        else
        {
            AddPointAtAngle(_lastAngle);
        }      
    }

    /// <summary>
    /// Extends out the root from last angle and current position 
    /// </summary>
    /// <param name="_nextBranch"></param>
    public void ExtendBranch()
    {
        
        //TODO: Work on adding conditionals to branch creation if it is the first branch being created
        RootPath tempRoot = GameObject.Instantiate(this);
        _branches.Add(tempRoot);
        tempRoot.SetRootPoint(_lr.GetPosition(_lr.positionCount - 1));
        tempRoot.AddPointAtAngle(_lastAngle);

        _lastBranch = _branches.Count - 1;

        if (_branches.Count > 0)
        {
            _branches.Add(this);
            this.AddPointAtAngle(_lastAngle);
            _lastBranch = 0;
        }
        
    }

    public void OnRootSelected()
    {
        _lr.startColor = _lr.endColor = new Color(.5f, .8f, .2f);
    }

    public void OnRootDeselected()
    {
        _lr.startColor = _lr.endColor = new Color(1, 1, 1);
    }

    public Vector3[] GetPotentialPoints()
    {
        return _potentialPoints;
    }

    /// <summary>
    /// Will try to get the next point within the root path on whatever the next branch is
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 TryToGetPotentialPoint()
    {
        Vector3 point = this.GetNextPotentialPoint();

        if (_branches.Count > 0)
        {
            _lastBranch = (_lastBranch + 1 < _branches.Count) ? _lastBranch : 0;

            point = _branches[_lastBranch].GetNextPotentialPoint();
        }

        return point;
    }

    public Vector3 GetNextPotentialPoint()
    {
        _selectedPoint = (_potentialPoints.Length - 1 > _selectedPoint) ? _selectedPoint + 1 : 0;

        return _potentialPoints[_selectedPoint];
    }


    public Vector3 GetPotentialPoint(int _selectedPoint)
    {
        return _selectedPoint < _potentialPoints.Length && _selectedPoint >= 0 ? _potentialPoints[_selectedPoint] : Vector3.zero; 
    }

    public void AddPointAtAngle(int degrees)
    {
        Vector3 newPoint = GetPointAtAngle(degrees);
        _lr.positionCount++;
        _lr.SetPosition(_lr.positionCount - 1, newPoint);

        _endPoint = newPoint;
        _lastAngle = degrees;

        _potentialPoints[0] = GetPointAtAngle(degrees);
        _potentialPoints[1] = GetPointAtAngle((degrees + 60) % 360);
        _potentialPoints[2] = GetPointAtAngle((degrees - 60) % 360);

        _selectedPoint = 0;
    }

    public void AddPotentialPoint()
    {
        if (_branches.Count > 0)
        {
            for (int i = 0; i < _branches.Count; i++)
            {
                if (i != _lastBranch)
                {
                    _branches[i].ExtendRoot();
                }
                else
                {
                    _branches[i].AddPotentialPoint();
                }
            }
        }
        else
        {
            if (_selectedPoint == 0)
                AddPointAtAngle(_lastAngle);
            else if (_selectedPoint == 1)
                AddPointAtAngle((_lastAngle + 60) % 360);
            else
                AddPointAtAngle((_lastAngle - 60) % 360);
        }
    }

    public Vector3 GetPointAtAngle( int degrees )
    {
        Vector3 newPoint = _endPoint;
        newPoint.x += Mathf.Cos(rad(degrees)) * GameConstants.UNIT_LENGTH;
        newPoint.y += Mathf.Sin(rad(degrees)) * GameConstants.UNIT_LENGTH;
        return newPoint;
    }

    public IEnumerable<RootPath> GetRootPaths()
    {
        if (_branches.Count > 0)
        {
            foreach (RootPath path in _branches)
            {
                yield return path;
            }
        }
        else
        {
            yield return this;
        }
    }

    private float rad( int degrees )
    {
        return degrees * Mathf.PI / 180;
    }

}
