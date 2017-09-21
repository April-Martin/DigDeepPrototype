using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class RoundTracker : MonoBehaviour {

    public UnityEvent EndRound;
    public Image[] actionIcons;
    public GameObject TreeStandin;
    public int currMovesPerRound;
    public float speed;
    private int movesPlayed;
    private int currRound;
    private Vector3 originalTreePos;
    private float originalTreeScale;
    private float endTreeScale;
    private float scalingFactor = 0.1f;

	// Use this for initialization
	void Start () {
        currMovesPerRound = GameConstants.BASE_MOVES_PER_ROUND;
        movesPlayed = 0;
        currRound = 1;
        originalTreePos = TreeStandin.transform.position;
        originalTreeScale = TreeStandin.transform.localScale.y;
        endTreeScale = originalTreeScale;
	}

    private void Update()
    {
        TreeStandin.transform.localScale = new Vector3(TreeStandin.transform.localScale.x, Mathf.MoveTowards(TreeStandin.transform.localScale.y, endTreeScale, Time.deltaTime * speed));
    }

    public void RegisterMove(int step)
    {
        for (int i = 0; i < step; i++)
        {
            actionIcons[movesPlayed].color = new Color(actionIcons[movesPlayed].color.r, actionIcons[movesPlayed].color.g, actionIcons[movesPlayed].color.b, actionIcons[movesPlayed].color.a / 2);
            movesPlayed++;
        }
        
        if (movesPlayed >= currMovesPerRound)
        {
            StartNextRound();
            endTreeScale = endTreeScale + (scalingFactor);
        }
    }

    private void StartNextRound()
    {
        for (; movesPlayed >= 0; movesPlayed--)
        {
            if (movesPlayed < currMovesPerRound)
            {
                actionIcons[movesPlayed].color = new Color(actionIcons[movesPlayed].color.r, actionIcons[movesPlayed].color.g, actionIcons[movesPlayed].color.b, actionIcons[movesPlayed].color.a * 2);
            }
        }

        movesPlayed = 0;
        currRound++;
        EndRound.Invoke();
    }
}
