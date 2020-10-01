using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MCTSNode
{
    public int visits = 0;
    public int wins = 0;

    Game game;
    List<MCTSNode> childrens = new List<MCTSNode>();

    void Compute()
    {

    }
}

public class MCTSAgent: AAgent
{
    MCTSNode root;

    override public Action GetAction()
    {
        return Action.Idle;
    }
}
