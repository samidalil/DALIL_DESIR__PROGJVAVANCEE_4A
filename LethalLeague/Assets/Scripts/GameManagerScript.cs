﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] Transform player1Transform;
    [SerializeField] Transform player2Transform;
    [SerializeField] Transform ballTransform;
    [SerializeField] Transform[] wallsTransform;
    [SerializeField] AgentType agentType1;
    [SerializeField] AgentType agentType2;

    // Debugging
    [SerializeField] Transform strikeHitbox1Transform;
    [SerializeField] Transform strikeHitbox2Transform;

    AAgent agent1;
    AAgent agent2;

    bool isRunning = true;
    Game game;

    public void ToggleRunning()
    {
        isRunning = !isRunning;
    }

    void Awake()
    {
        InitializeGame();

        agent1 = AAgent.CreateAgent(agentType1, game, PlayerTag.One);
        agent2 = AAgent.CreateAgent(agentType2, game, PlayerTag.Two);
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            game.PlayAction(agent1.GetAction(), agent1.GetPlayerTag());
            game.PlayAction(agent2.GetAction(), agent2.GetPlayerTag());
            game.Tick();
            UpdatePositions();
        }
    }

    void InitializeGame()
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
        player1Transform.localScale = game.player1.box.scale;

        player2Transform.position = game.player2.box.position;
        player2Transform.localScale = game.player2.box.scale;

        ballTransform.position = game.ball.circle.position;
        ballTransform.localScale = new Vector2(game.ball.circle.radius, game.ball.circle.radius);

        for (int i = 0; i < 4; i++)
        {
            wallsTransform[i].position = game.walls[i].position;
            wallsTransform[i].localScale = game.walls[i].scale;
        }

        this.game = game;
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