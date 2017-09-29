using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoundTracker : MonoBehaviour {

    public UnityEvent EndRound;
    public GameObject ActionBar;
    public GameObject ActionIcon;
    public GameObject TreeStandin;
    public int currMovesPerRound;
    public float speed;
    public float waitTime;

    public int MovesLeft { get { return movesLeft + 1; } }

    private List<Image> actionIcons;
    private int movesLeft;
    private int currRound;
    private Vector3 originalTreePos;
    private float originalTreeScale;
    private float endTreeScale;
    private float scalingFactor = 0.01f;
    private bool canGrow = false;

	// Use this for initialization
	void Start () {
        currMovesPerRound = GameConstants.BASE_MOVES_PER_ROUND;
        movesLeft = currMovesPerRound - 1;
        currRound = 1;
        originalTreePos = TreeStandin.transform.position;
        originalTreeScale = TreeStandin.transform.localScale.y;
        endTreeScale = originalTreeScale;
        actionIcons = new List<Image>(3);

        for (int i = 0; i < currMovesPerRound; i++)
        {
            CreateNewActionIcon();
        }
	}

    private void Update()
    {
        if (canGrow)
            TreeStandin.transform.localScale = new Vector3(TreeStandin.transform.localScale.x, Mathf.MoveTowards(TreeStandin.transform.localScale.y, endTreeScale, Time.deltaTime * speed));

        if (TreeStandin.transform.localScale.y == endTreeScale)
            canGrow = false;
    }

    /// <summary>
    /// Will take the step given and reduce the movement actions available to player by that step amount.
    /// If the step amount is too much, then the method will just go through remaining actions.
    /// </summary>
    /// <param name="step"></param>
    public void RegisterMove(int step)
    {
        if (movesLeft - step >= 0)
        {
                for (int i = 0; i < step; i++)
                {
                    try
                    {
                        actionIcons[movesLeft].color = new Color(actionIcons[movesLeft].color.r, actionIcons[movesLeft].color.g, actionIcons[movesLeft].color.b, actionIcons[movesLeft].color.a / 2);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }

                    movesLeft--;
                }
        }
        else
        {
            // Go through remaining actions
            for (; movesLeft >= 0; movesLeft--)
            {
                actionIcons[movesLeft].color = new Color(actionIcons[movesLeft].color.r, actionIcons[movesLeft].color.g, actionIcons[movesLeft].color.b, actionIcons[movesLeft].color.a / 2);
            }
        }

        if (movesLeft < 0)
        {
            //actionIcons[0].color = new Color(actionIcons[0].color.r, actionIcons[0].color.g, actionIcons[0].color.b, actionIcons[0].color.a / 2);
            // Run non-active phase on separate coroutine
            NonActivePhaseBegin();
        }
    }

    private void NonActivePhaseBegin()
    {
        currRound++;
        EndRound.Invoke();
    }

    public void StartNextRound(int actionBonus, int heightBonus)
    {
        bool createNewAction = false;
        if (currMovesPerRound >= 3 && currMovesPerRound < 5)
        {
            currMovesPerRound = GameConstants.BASE_MOVES_PER_ROUND + actionBonus;

            createNewAction = true;
        }

        float finalHeightBonus = heightBonus != 0 ? heightBonus : -scalingFactor;
        StartCoroutine(GrowTree(actionBonus, createNewAction, finalHeightBonus));
    }

    /// <summary>
    /// Creates a new action Icon and adds it to the actionIcons list.E
    /// </summary>
    private void CreateNewActionIcon ()
    {
        GameObject action = Instantiate(ActionIcon, ActionBar.transform);
        actionIcons.Add(action.GetComponent<Image>());
    }

    private IEnumerator GrowTree(float ActionBonus, bool CreateNewAction, float HeightBonus)
    {
        endTreeScale += HeightBonus;
        canGrow = true;

        for (int i = 0; i < currMovesPerRound; i++)
        {
            if ( CreateNewAction && i >= actionIcons.Count)
                CreateNewActionIcon();
            else
                actionIcons[i].color = new Color(actionIcons[i].color.r, actionIcons[i].color.g, actionIcons[i].color.b, actionIcons[i].color.a * 2);

            yield return new WaitForSeconds(waitTime);
        }

        int iconsToDelete = actionIcons.Count - currMovesPerRound;
        for (int i = 0; i < iconsToDelete; i++)
        {
            actionIcons[actionIcons.Count - 1].transform.SetParent(null);
            GameObject.Destroy(actionIcons[actionIcons.Count - 1]);
            actionIcons.RemoveAt(actionIcons.Count - 1);
        }

            movesLeft = currMovesPerRound - 1;
    }
}
