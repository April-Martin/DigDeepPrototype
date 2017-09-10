using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public GameObject dotPrefab;

	// Use this for initialization
	void Start () {
        //DrawDots();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DrawDots()
    {
        for (int y = 0; y < GameConstants.DOTS_HIGH; y+=2 )
        {
            for (int x = -GameConstants.DOTS_WIDE/2; x < GameConstants.DOTS_WIDE/2; x++ )
            {
                Vector3 posA = new Vector3(x * GameConstants.DOT_DELTA_X, -y * GameConstants.DOT_DELTA_Y);
                Vector3 posB = new Vector3(x + .5f * GameConstants.DOT_DELTA_X, -(y + 1) * GameConstants.DOT_DELTA_Y);

                Instantiate(dotPrefab, posA, Quaternion.identity);
                Instantiate(dotPrefab, posB, Quaternion.identity);
            }
        }
    }
}
