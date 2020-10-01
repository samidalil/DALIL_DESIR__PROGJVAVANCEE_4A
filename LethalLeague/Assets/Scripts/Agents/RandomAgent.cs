using System.Collections.Generic;
using UnityEngine;

public class RandomAgent: AAgent
{
    override public Action GetAction()
    {
        List<Action> possibleActions = game.GetPossibleActions(tag);

        return possibleActions[Random.Range(0, possibleActions.Count)];
    }
}