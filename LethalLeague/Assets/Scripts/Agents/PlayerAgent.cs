using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : AAgent
{
    readonly Queue<Action> bufferPlayer1 = new Queue<Action>();
    readonly Queue<Action> bufferPlayer2 = new Queue<Action>();

    Action GetActionPlayer1()
    {
        if (bufferPlayer1.Count > 0) return bufferPlayer1.Dequeue();

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
            if (a != Action.Idle) bufferPlayer1.Enqueue(Action.Jump);
            else a = Action.Jump;
        }
        else if (
            possibleActions.Contains(Action.StrikeStraight) &&
            Input.GetKeyDown(KeyCode.C)
        )
        {
            Action t;

            if (Input.GetKey(KeyCode.W)) t = Action.StrikeUp;
            else if (Input.GetKey(KeyCode.S)) t = Action.StrikeDown;
            else t = Action.StrikeStraight;

            if (a != Action.Idle) bufferPlayer1.Enqueue(t);
            else a = t;
        }

        return a;
    }

    Action GetActionPlayer2()
    {
        if (bufferPlayer2.Count > 0) return bufferPlayer2.Dequeue();

        Action a = Action.Idle;
        List<Action> possibleActions = game.GetPossibleActions(tag);

        if (Input.GetKey(KeyCode.RightArrow))
            a = Action.MoveRight;
        else if (Input.GetKey(KeyCode.LeftArrow))
            a = Action.MoveLeft;

        if (
            possibleActions.Contains(Action.Jump) &&
            Input.GetKeyDown(KeyCode.Keypad0)
        )
        {
            if (a != Action.Idle) bufferPlayer2.Enqueue(Action.Jump);
            else a = Action.Jump;
        }
        else if (
            possibleActions.Contains(Action.StrikeStraight) &&
            Input.GetKeyDown(KeyCode.KeypadEnter)
        )
        {
            Action t;

            if (Input.GetKey(KeyCode.UpArrow)) t = Action.StrikeUp;
            else if (Input.GetKey(KeyCode.DownArrow)) t = Action.StrikeDown;
            else t = Action.StrikeStraight;

            if (a != Action.Idle) bufferPlayer2.Enqueue(t);
            else a = t;
        }

        return a;
    }

    override public Action GetAction()
    {
        if (tag == PlayerTag.One) return GetActionPlayer1();
        if (tag == PlayerTag.Two) return GetActionPlayer2();
        return Action.Idle;
    }
}