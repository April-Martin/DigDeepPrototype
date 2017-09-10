using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSystem : MonoBehaviour {

    public RootPath Root;
    public SpriteRenderer Highlight;

    private int _endNum;
    private List<RootPath> _roots;
    private SpriteRenderer _highlight;
    private int _selectedRoot;
    private int _selectedPoint;

	// Use this for initialization
	void Start () {

        _roots = new List<RootPath>();
        _selectedRoot = 0;
        _selectedPoint = 0;
        _endNum = 2;

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
                if ( i != _selectedRoot)
                {
                    _roots[i].ExtendRoot();
                }
                else
                {
                    _roots[i].AddPotentialPoint(_selectedPoint);
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
            HighlightSelectedPoint(_roots[_selectedRoot].GetPotentialPoints());
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
            HighlightSelectedPoint(_roots[_selectedRoot].GetPotentialPoints());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _selectedPoint = (_selectedPoint + 1) % 3;
            HighlightSelectedPoint(_roots[_selectedRoot].GetPotentialPoints());
        }
        
	}

    private void HighlightSelectedPoint( Vector3[] points )
    {

        _highlight.transform.position = points[_selectedPoint];
        _highlight.enabled = true;
    }

}
