using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class RoundTracker : MonoBehaviour {

    public UnityEvent EndRound;
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
        movesPlayed++;
        if (movesPlayed >= currMovesPerRound)
        {
            StartNextRound();
        }
    }

    private void StartNextRound()
    {
        movesPlayed = 0;
        currRound++;
        EndRound.Invoke();
    }
}
