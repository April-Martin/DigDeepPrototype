using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSystem : MonoBehaviour {

    public RootPath Root;
    public SpriteRenderer Highlight;
    public SpriteRenderer BranchHighlight;

    private int _endNum;
    private List<RootPath> _roots;
    private List<RootPath> _activeRoots;
    private SpriteRenderer[] _highlights;
    private SpriteRenderer _branchHighlight;
    private int _selectedRoot;
    private int _selectedPoint;

    private bool branch;
    private int _selectedBranchPosition;

    // Use this for initialization
    void Start () {

        _roots = new List<RootPath>();
        _activeRoots = new List<RootPath>();
        _highlights = new SpriteRenderer[3];
        _selectedRoot = 0;
        _selectedPoint = 0;
        _endNum = 2;
        branch = false;

        for (int i = 0; i < _endNum; i++ )
        {
            _roots.Add(GameObject.Instantiate(Root));
            _roots[i].SetRootPoint(Vector3.zero);
            _roots[i].rootSys = this;
        }

        for (int i = 0; i < 3; i++)
        {
            _highlights[i] = GameObject.Instantiate(Highlight);
            _highlights[i].enabled = false;
        }

        _branchHighlight = GameObject.Instantiate(BranchHighlight);
        _branchHighlight.enabled = false;

        _roots[0].AddPointAtAngle(240);
        _roots[1].AddPointAtAngle(300);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            HideHighlights();
            _branchHighlight.enabled = false;

            for (int i=0; i<_roots.Count; i++)
            {
                if ( i != _selectedRoot)
                {
                    
                    _roots[i].ExtendRoot();
                }
                else
                {
                    if (branch && _selectedPoint != _selectedBranchPosition)
                    {
                        CreateNewBranch(_roots[i]);
                        _roots[i].ExtendRoot();
                    }
                    else
                    {
                        _roots[i].AddPotentialPoint(_selectedPoint);
                    }   
                }
            }
            _selectedPoint = 0;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _roots[_selectedRoot].OnRootDeselected();
            _branchHighlight.enabled = false;
            branch = false;

            _selectedRoot++;
            if (_selectedRoot >= _roots.Count)
            {
                _selectedRoot = 0;
            }

            _roots[_selectedRoot].OnRootSelected();
            HighlightSelectedPoint(_roots[_selectedRoot].GetPotentialPoints());
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _roots[_selectedRoot].OnRootDeselected();
            _branchHighlight.enabled = false;
            branch = false;

            _selectedRoot--;
            if (_selectedRoot < 0)
            {
                _selectedRoot = _roots.Count - 1;
            }

            _roots[_selectedRoot].OnRootSelected();
            HighlightSelectedPoint(_roots[_selectedRoot].GetPotentialPoints());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _selectedPoint = (_selectedPoint + 1) % 3;
            HighlightSelectedPoint(_roots[_selectedRoot].GetPotentialPoints());
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            branch = true;
            _selectedBranchPosition = _selectedPoint;
            HighlightSelectedBranchPoint(_roots[_selectedRoot].GetPotentialPoints());
        }
	}
     /// <summary>
     /// Hide the highlight sprites associated with the this object
     /// </summary>
    public void HideHighlights()
    {
        foreach ( SpriteRenderer h in _highlights )
        {
            h.enabled = false;
        }
    }

    /// <summary>
    /// Show all highlight sprite positions associated with the this object
    /// </summary>
    public void HighlightPoints( Vector3[] points )
    {
        HideHighlights();
        for ( int i=0; i< points.Length; i++ )
        {
            _highlights[i].transform.position = points[i];
            _highlights[i].enabled = true;
        }
    }

    /// <summary>
    /// Show the selected highlight sprites associated with the this object
    /// </summary>
    private void HighlightSelectedPoint( Vector3[] points )
    {
        HideHighlights();
        _highlights[0].transform.position = points[_selectedPoint];
        _highlights[0].enabled = true;
    }

    /// <summary>
    /// Show the selected branch highlight sprites associated with the this object
    /// </summary>
    private void HighlightSelectedBranchPoint(Vector3[] points)
    {

        _branchHighlight.transform.position = points[_selectedPoint];
        _branchHighlight.enabled = true;
    }


    public void CreateNewBranch(RootPath chosenRoot)
    {
        Vector3[] oldRootPath = chosenRoot.GetLRPositions();
        float branchAngle = chosenRoot._branchLastAngle;
        RootPath newBranch = GameObject.Instantiate(Root);
        newBranch.SetRootPoint(Vector3.zero);
        newBranch.AddAllPoints(oldRootPath, branchAngle);

        newBranch.AddPotentialPoint(_selectedBranchPosition);

        _roots.Add(newBranch);

        newBranch.rootSys = this;

        branch = false;
    }

    public void MarkRootActive( RootPath root )
    {
        _activeRoots.Add(root);
    }
    
    public void GrowInactiveRoots()
    {
        Debug.Log("Hi!");
        foreach ( RootPath root in _roots )
        {
            if ( !_activeRoots.Contains ( root) )
                root.ExtendRoot();
        }
        _activeRoots.Clear();
    }
}
