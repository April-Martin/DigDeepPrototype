using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Main : MonoBehaviour
{

    public GameObject dotPrefab;
    public Sprite resourceImg;

    // Use this for initialization
    void Start()
    {
        DrawDots();
        DrawResources();
    }

    void DrawDots()
    {
        for (int y = 0; y < GameConstants.DOTS_HIGH; y += 2)
        {
            for (int x = -GameConstants.DOTS_WIDE / 2; x < GameConstants.DOTS_WIDE / 2; x++)
            {
                Vector3 posA = new Vector3(x * GameConstants.DOT_DELTA_X, -y * GameConstants.DOT_DELTA_Y);
                Vector3 posB = new Vector3((x + .5f) * GameConstants.DOT_DELTA_X, -(y + 1) * GameConstants.DOT_DELTA_Y);

                Instantiate(dotPrefab, posA, Quaternion.identity);
                Instantiate(dotPrefab, posB, Quaternion.identity);
            }
        }
    }

    void DrawResources()
    {
        Collider2D resource;

        for (int y = 0; y < GameConstants.DOTS_HIGH; y += 2)
        {
            for (int x = -GameConstants.DOTS_WIDE / 2; x < GameConstants.DOTS_WIDE / 2; x++)
            {
                Vector3 posA = new Vector3(x * GameConstants.DOT_DELTA_X, -y * GameConstants.DOT_DELTA_Y);
                Vector3 posB = new Vector3((x + .5f) * GameConstants.DOT_DELTA_X, -(y + 1) * GameConstants.DOT_DELTA_Y);

                resource = Physics2D.OverlapPoint(posA, LayerMask.GetMask("Resources"));
                if (resource != null)
                {
                    SpriteRenderer dotA = Instantiate(dotPrefab, posA, Quaternion.identity).GetComponent<SpriteRenderer>();
                    dotA.sprite = resource.GetComponent<Resource>().TileSprite;
                    dotA.sortingOrder = 1;
                }
                resource = Physics2D.OverlapPoint(posB, LayerMask.GetMask("Resources"));
                if (resource != null)
                {
                    SpriteRenderer dotB = Instantiate(dotPrefab, posB, Quaternion.identity).GetComponent<SpriteRenderer>();
                    dotB.sprite = resource.GetComponent<Resource>().TileSprite;
                    dotB.sortingOrder = 1;
                }
            }
        }

    }
}
