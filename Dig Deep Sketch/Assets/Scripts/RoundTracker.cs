using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class RoundTracker : MonoBehaviour {

    public UnityEvent EndRound;
    public Image[] actionIcons;
    public int currMovesPerRound;
    private int movesPlayed;
    private int currRound;

	// Use this for initialization
	void Start () {
        currMovesPerRound = GameConstants.BASE_MOVES_PER_ROUND;
        movesPlayed = 0;
        currRound = 1;
	}

    public void RegisterMove()
    {
        actionIcons[movesPlayed].color = new Color(actionIcons[movesPlayed].color.r, actionIcons[movesPlayed].color.g, actionIcons[movesPlayed].color.b, actionIcons[movesPlayed].color.a / 2);
        movesPlayed++;
        if (movesPlayed >= currMovesPerRound)
        {
            StartNextRound();
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
