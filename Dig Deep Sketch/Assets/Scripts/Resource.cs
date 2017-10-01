using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public GameConstants.Resources ResourceType;
    public Sprite[] ResourceTiles;

    private int actionBonus;
    public int ActionBonus { get{ return actionBonus; } }

    private float heightBonus;
    public float HeightBonus{ get{ return heightBonus; } }

    public int defaultActionBonusValue = 1;
    public float defaultHeightBonusValue = 0.1f;

    private Sprite tileSprite;
    public Sprite TileSprite { get { return tileSprite; } }

	// Use this for initialization
	void Start () {
        if ( ResourceType == GameConstants.Resources.WATER )
        {
            actionBonus = defaultActionBonusValue;
            heightBonus = defaultHeightBonusValue;
            tileSprite = ResourceTiles[0];
        }
		else if ( ResourceType == GameConstants.Resources.BAD )
        {
            actionBonus = -defaultActionBonusValue;
            heightBonus = -defaultHeightBonusValue;
            tileSprite = ResourceTiles[1];
        }
        else if ( ResourceType == GameConstants.Resources.GOOD )
        {
            actionBonus = defaultActionBonusValue;
            heightBonus = defaultHeightBonusValue;
            tileSprite = ResourceTiles[2];
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
