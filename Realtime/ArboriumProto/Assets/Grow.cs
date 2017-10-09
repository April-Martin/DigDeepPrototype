using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour {

	Vector3 clickedPoint;
	public float speed = 30f;
	void Start () {
		transform.up = Vector3.down;
	}

	void Update () {
		
		transform.Translate (Vector3.up * Time.deltaTime * speed/100f);
	}
	void OnMouseDown()
	{
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
		transform.up = clickedPoint - transform.position;
	}
}
