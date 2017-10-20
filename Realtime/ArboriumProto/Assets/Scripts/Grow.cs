using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour {

	Vector3 clickedPoint;
	public float speed = 30f;
	public float pathSpacing = .01f;
	List<Vector3> pathPts;
	public GameObject Cir;
	GameObject rootPath;

	void Start () {
		rootPath = GameObject.Find ("RootPath");
		transform.up = Vector3.down;
		pathPts = new List<Vector3> ();
	}

	void Update () {	
		
		transform.Translate (Vector3.up * Time.deltaTime * speed/100f);

		if(pathPts.Count > 0)
		{
			if (Vector3.Distance (pathPts [0], transform.position) < .1f) {
				pathPts.RemoveAt (0);
			}
			transform.up = pathPts[0] - transform.position;
		}
	}
	void OnMouseDown()
	{
		pathPts.Clear ();
		clickedPoint = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y));
		clickedPoint.z = 0;
	}
	void OnMouseDrag()
	{
		
		if (Input.GetKeyDown (KeyCode.B)) {
			GameObject.Instantiate (gameObject);
		}

		clickedPoint = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y));
		clickedPoint.z = 0;
		if (pathPts.Count == 0) {
			pathPts.Add (clickedPoint);
		}
		if (Vector3.Distance (clickedPoint, pathPts [pathPts.Count-1]) > pathSpacing) {
			pathPts.Add (clickedPoint);
			Cir.transform.position = clickedPoint;
			GameObject.Instantiate (Cir).transform.parent = rootPath.transform;
		}
	}
	void OnMouseUp()
	{
		//erase trace path after mouseup
		for (int x = 0; x < rootPath.transform.childCount; x++) {
			GameObject.Destroy (rootPath.transform.GetChild (x).gameObject);
		}
	}
}
