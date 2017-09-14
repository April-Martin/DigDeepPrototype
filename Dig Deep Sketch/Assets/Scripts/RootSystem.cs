﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSystem : MonoBehaviour {

    public RootPath Root;
    public SpriteRenderer Highlight;
    public SpriteRenderer BranchHighlight;

    private int _endNum;
    private List<RootPath> _roots;
    private SpriteRenderer _highlight;
    private SpriteRenderer _branchHighlight;
    private int _selectedRoot;
    private int _selectedPoint;

    private bool branch;
    private int _selectedBranchPosition;

    // Use this for initialization
    void Start () {

        _roots = new List<RootPath>();
        _selectedRoot = 0;
        _selectedPoint = 0;
        _endNum = 2;
        branch = false;

        for (int i = 0; i < _endNum; i++ )
        {
            _roots.Add(GameObject.Instantiate(Root));
            _roots[i].SetRootPoint(Vector3.zero);
        }

        for (int i = 0; i < 3; i++)
        {
            _highlight = GameObject.Instantiate(Highlight);
            _highlight.enabled = false;
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
            _highlight.enabled = false;
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
                        Vector3[] oldRootPath = _roots[i].GetLRPositions();
                        int _lastOldRootAngle = _roots[i]._lastAngle;
                        RootPath firstRootBranch = GameObject.Instantiate(Root);
                        RootPath secondRootBranch = GameObject.Instantiate(Root);
                        firstRootBranch.SetRootPoint(Vector3.zero);
                        secondRootBranch.SetRootPoint(Vector3.zero);
                        firstRootBranch.AddAllPoints(oldRootPath, _lastOldRootAngle);
                        secondRootBranch.AddAllPoints(oldRootPath, _lastOldRootAngle);

                        firstRootBranch.AddPotentialPoint(_selectedPoint);
                        secondRootBranch.AddPotentialPoint(_selectedBranchPosition);

                        _roots[i] = firstRootBranch;
                        _roots.Add(secondRootBranch);

                        branch = false;
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

    private void HighlightSelectedPoint( Vector3[] points )
    {

        _highlight.transform.position = points[_selectedPoint];
        _highlight.enabled = true;
    }

    private void HighlightSelectedBranchPoint(Vector3[] points)
    {

        _branchHighlight.transform.position = points[_selectedPoint];
        _branchHighlight.enabled = true;
    }
}
