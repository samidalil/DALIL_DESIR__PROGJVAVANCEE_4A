public enum AgentType
{
    Player,
    Random,
    MCTS
};

public abstract class AAgent
{
    public static AAgent CreateAgent(AgentType a, Game g, PlayerTag t)
    {
        AAgent agent;

        if (a == AgentType.Player)
            agent = new PlayerAgent();
        else if (a == AgentType.Random)
            agent = new RandomAgent();
        else agent = new MCTSAgent();

        agent.SetGame(g);
        agent.SetPlayerTag(t);

        return agent;
    }

    protected Game game;
    protected PlayerTag tag;

    public void SetGame(Game g)
    {
        game = new Game(g);
    }

    public void SetPlayerTag(PlayerTag playerTag)
    {
        tag = playerTag;
    }

    public PlayerTag GetPlayerTag()
    {
        return tag;
    }

    public abstract Action GetAction();
}