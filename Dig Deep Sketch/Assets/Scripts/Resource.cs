using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public GameConstants.Resources ResourceType;
    public Sprite[] ResourceTiles;

    private int actionBonus;
    public int ActionBonus { get{ return actionBonus; } }

    private int heightBonus;
    public int HeightBonus{ get{ return heightBonus; } }

    private Sprite tileSprite;
    public Sprite TileSprite { get { return tileSprite; } }

	// Use this for initialization
	void Start () {
        if ( ResourceType == GameConstants.Resources.WATER )
        {
            actionBonus = 1;
            heightBonus = 1;
            tileSprite = ResourceTiles[0];
        }
		else if ( ResourceType == GameConstants.Resources.BAD )
        {
            actionBonus = -1;
            heightBonus = -1;
            tileSprite = ResourceTiles[1];
        }
        else if ( ResourceType == GameConstants.Resources.GOOD )
        {
            actionBonus = 1;
            heightBonus = 1;
            tileSprite = ResourceTiles[2];
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
