using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootSystem : MonoBehaviour {

    public RootPath Root;
    public SpriteRenderer Highlight;
    public SpriteRenderer HighlightBranch;

    private int _endNum;
    private List<RootPath> _roots;
    private SpriteRenderer _highlight;
    private SpriteRenderer _highlightBranch;
    private int _selectedRoot;
    private Vector3 _selectedPoint;

    private bool _branchMode;
    [SerializeField]
    private Text branchStatus;
    private Vector3 _selectedBranchPoint;


	// Use this for initialization
	void Start () {

        _roots = new List<RootPath>();
        _selectedRoot = 0;
        _selectedPoint = Vector3.zero;
        _endNum = 2;
        _branchMode = false;
        branchStatus.text = "Branch Mode: OFF";

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

        _highlightBranch = GameObject.Instantiate(HighlightBranch);
        _highlightBranch.enabled = false;

        _roots[0].AddPointAtAngle(240);
        _roots[1].AddPointAtAngle(300);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            _highlight.enabled = false;

            for (int i=0; i<_roots.Count; i++)
            {
                if (i != _selectedRoot)
                {
                    if (_roots[i].IsBranching)
                    {
                        _roots[i].ExtendBranch();
                        DeHighlightSelectedBranchPoint(i);
                    }
                    else
                    {
                        _roots[i].ExtendRoot();
                    }
                }
                else
                {
                    if (_roots[i].IsBranching)
                    {
                        _roots[i].ExtendBranch();
                        DeHighlightSelectedBranchPoint(i);
                    }
                    else
                    {
                        // TODO: Add new point to root and extend roots of branches
                        _roots[i].AddPotentialPoint();
                    }
                }
               
            }            
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _roots[_selectedRoot].OnRootDeselected();

            _selectedRoot++;
            if (_selectedRoot >= _roots.Count)
            {
                _selectedRoot = 0;
            }

            _roots[_selectedRoot].OnRootSelected();
            HighlightSelectedPoint();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _roots[_selectedRoot].OnRootDeselected();

            _selectedRoot--;
            if (_selectedRoot < 0)
            {
                _selectedRoot = _roots.Count - 1;
            }

            _roots[_selectedRoot].OnRootSelected();
            HighlightSelectedPoint();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HighlightSelectedPoint();
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            HighlightSelectedBranchPoint();
        }
	}

    //private void HighlightSelectedPoint( Vector3[] points )
    //{
    //    _highlight.transform.position = points[_selectedPoint];
    //    _highlight.enabled = true;
    //}

    private void HighlightSelectedPoint()
    {
        _highlight.transform.position = _roots[_selectedRoot].TryToGetPotentialPoint();
        _highlight.enabled = true;
    }

    private void HighlightSelectedBranchPoint()
    {
        Vector3 _branchPointCandidate = _roots[_selectedRoot].TryToGetPotentialPoint();
        if (_selectedBranchPoint == _branchPointCandidate && _highlightBranch.enabled)
        {
            DeHighlightSelectedBranchPoint(_selectedRoot);
        }
        else
        {
            branchStatus.text = "Branch Mode: ON";
            _roots[_selectedRoot].IsBranching = true;
            _selectedBranchPoint = _branchPointCandidate;
            _highlightBranch.transform.position = _roots[_selectedRoot].TryToGetPotentialPoint();
            _highlightBranch.enabled = true;
        }
    }

    private void DeHighlightSelectedBranchPoint(int root)
    {
        _highlightBranch.enabled = false;
        branchStatus.text = "Branch Mode: OFF";
        _roots[root].IsBranching = false;
    }
}
