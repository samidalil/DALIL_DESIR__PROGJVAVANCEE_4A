using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] Transform player1Transform;
    [SerializeField] Transform player2Transform;
    [SerializeField] Transform ballTransform;
    [SerializeField] Transform[] wallsTransform;

    [SerializeField] private UIManagerScript uiManager;

    // Debugging
    [SerializeField] AgentType agentType1;
    [SerializeField] AgentType agentType2;
    [SerializeField] Transform strikeHitbox1Transform;
    [SerializeField] Transform strikeHitbox2Transform;

    Game game;
    bool isRunning = false;
    AAgent agent1;
    AAgent agent2;


    public void ToggleRunning()
    {
        isRunning = !isRunning;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    void Awake()
    {
        InitializeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isRunning) uiManager.Pause();
            else uiManager.Resume();

            ToggleRunning();
        }
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            game.PlayAction(agent1.GetAction(), agent1.GetPlayerTag());
            game.PlayAction(agent2.GetAction(), agent2.GetPlayerTag());

            game.Tick();

            agent1.SetGame(game);
            agent2.SetGame(game);

            uiManager.OnGameTick(game);

            UpdatePositions();

            Debug.Log("test");
            if (game.IsFinished()) FinishGame();
        }
    }

    void FinishGame()
    {
        isRunning = false;
        uiManager.OnGameEnd(game);
    }

    public void InitializeGame()
    {
        Game game = new Game();

        game.player1 = new Player(Vector2.left * 4, PlayerTag.One);
        game.player2 = new Player(Vector2.right * 4, PlayerTag.Two);
        game.ball = new Ball(
            Constants.BALL_DEFAULT_POS,
            Constants.BALL_DEFAULT_RADIUS,
            Constants.BALL_MIN_SPEED
        );

        Vector2 horizontalPos = new Vector2(0, 5);
        Vector2 verticalPos = new Vector2(10, 0);
        Vector2 horizontalScale = new Vector2(1, 10);
        Vector2 verticalScale = new Vector2(20, 1);

        game.walls = new Box[4]
        {
            new Box(horizontalPos, verticalScale),
            new Box(-horizontalPos, verticalScale),
            new Box(verticalPos, horizontalScale),
            new Box(-verticalPos, horizontalScale)
        };

        player1Transform.position = game.player1.box.position;
        player1Transform.localScale = new Vector3(game.player1.box.scale.x, game.player1.box.scale.y, 1);
        strikeHitbox1Transform.localScale = new Vector3(
            game.player1.strikeHitbox.box.scale.x,
            game.player1.strikeHitbox.box.scale.y, 1);

        player2Transform.position = game.player2.box.position;
        player2Transform.localScale = new Vector3(game.player2.box.scale.x, game.player2.box.scale.y, 1);
        strikeHitbox2Transform.localScale = new Vector3(
            game.player2.strikeHitbox.box.scale.x,
            game.player2.strikeHitbox.box.scale.y, 1);

        ballTransform.position = game.ball.circle.position;
        ballTransform.localScale = new Vector2(game.ball.circle.radius, game.ball.circle.radius);

        for (int i = 0; i < 4; i++)
        {
            wallsTransform[i].position = game.walls[i].position;
            wallsTransform[i].localScale = new Vector3(game.walls[i].scale.x, game.walls[i].scale.y, 1);
        }

        this.game = game;

        isRunning = true;

        uiManager.OnGameStart(game);

        agent1 = AAgent.CreateAgent(agentType1, game, PlayerTag.One);
        agent2 = AAgent.CreateAgent(agentType2, game, PlayerTag.Two);
    }

    void UpdatePositions()
    {
        ballTransform.position = game.ball.circle.position;

        player1Transform.position = game.player1.box.position;
        player2Transform.position = game.player2.box.position;

        strikeHitbox1Transform.position = game.player1.strikeHitbox.box.position;
        if (strikeHitbox1Transform.gameObject.activeSelf != game.player1.strikeHitbox.isEnabled)
            strikeHitbox1Transform.gameObject.SetActive(game.player1.strikeHitbox.isEnabled);

        strikeHitbox2Transform.position = game.player2.strikeHitbox.box.position;
        if (strikeHitbox2Transform.gameObject.activeSelf != game.player2.strikeHitbox.isEnabled)
            strikeHitbox2Transform.gameObject.SetActive(game.player2.strikeHitbox.isEnabled);
    }
}
