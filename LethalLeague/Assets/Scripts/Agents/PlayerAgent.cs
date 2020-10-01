using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : AAgent
{
    readonly Queue<Action> buffer = new Queue<Action>();

    override public Action GetAction()
    {
        if (buffer.Count > 0) return buffer.Dequeue();

        Action a = Action.Idle;
        List<Action> possibleActions = game.GetPossibleActions(tag);

        if (Input.GetKey(KeyCode.D))
            a = Action.MoveRight;
        else if (Input.GetKey(KeyCode.A))
            a = Action.MoveLeft;

        if (
            possibleActions.Contains(Action.Jump) &&
            Input.GetKeyDown(KeyCode.Space)
        )
        {
            if (a != Action.Idle) buffer.Enqueue(Action.Jump);
            else a = Action.Jump;
        }
        else if (
            possibleActions.Contains(Action.StrikeStraight) &&
            Input.GetKeyDown(KeyCode.LeftShift)
        )
        {
            Action t;

            if (Input.GetKey(KeyCode.W)) t = Action.StrikeUp;
            else if (Input.GetKey(KeyCode.S)) t = Action.StrikeDown;
            else t = Action.StrikeStraight;

            if (a != Action.Idle) buffer.Enqueue(t);
            else a = t;
        }

        return a;
    }
}