using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MCTSNode
{
    public int ni = 0;
    public int r = 0;

    public MCTSNode parent = null;
    public List<MCTSNode> childrens = new List<MCTSNode>();

    public Game game;
    public Action action;
}

public class MCTSAgent: AAgent
{
    MCTSNode root;

    override public Action GetAction()
    {
        if (root == null)
            root = new MCTSNode()
            {
                game = game,
                action = Action.Idle
            };

        for (int i = 0; i < 20; i++)
        {
            MCTSNode current = Selection(root);

            if (current.ni != 0)
            {
                Expand(current);
                current = current.childrens[0];
            }

            int result = Rollout(current);

            Backpropagate(current, result);
        }

        root = GetBestChildren(root);

        return root.action;
    }

    float ComputeUcb(MCTSNode n)
    {
        return (n.r / root.ni) + Mathf.Sqrt(2 * Mathf.Log(n.ni) / root.ni);
    }

    MCTSNode GetBestChildren(MCTSNode current)
    {
        MCTSNode result = null;

        float maxUcb = 0;

        foreach (MCTSNode node in current.childrens)
        {
            float ucb = ComputeUcb(node);

            if (ucb > maxUcb)
            {
                maxUcb = ucb;
                result = node;
            }
        }

        if (result == null)
        {
            result = current.childrens[Random.Range(0, current.childrens.Count - 1)];
        }

        return result;
    }

    MCTSNode Selection(MCTSNode current)
    {
        if (current.childrens.Count == 0) return current;

        return Selection(GetBestChildren(current));
    }

    void Expand(MCTSNode n)
    {
        foreach (Action action in n.game.GetPossibleActions(tag))
        {
            n.childrens.Add(
                new MCTSNode()
                {
                    game = game,
                    action = action,
                    parent = n
                }
            );
        }
    }

    int Rollout(MCTSNode n)
    {
        List<Action> possibleActions;
        Action a;

        while (!n.game.IsFinished())
        {
            possibleActions = n.game.GetPossibleActions(PlayerTag.One);
            a = possibleActions[Random.Range(0, possibleActions.Count - 1)];
            n.game.PlayAction(a, PlayerTag.One);

            possibleActions = n.game.GetPossibleActions(PlayerTag.Two);
            a = possibleActions[Random.Range(0, possibleActions.Count - 1)];
            n.game.PlayAction(a, PlayerTag.Two);

            // La simulation utilise trop de mémoire
            n.game.Tick();
        }

        return n.game.GetResult() == tag ? 1 : 0;
    }

    void Backpropagate(MCTSNode n, int score)
    {
        n.r += score;
        n.ni++;

        if (n.parent != null) Backpropagate(n.parent, score);
    }
}
