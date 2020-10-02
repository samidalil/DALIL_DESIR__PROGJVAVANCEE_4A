using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    static readonly int hitTickCooldown = 200;
    static readonly int strikeTickLifetime = 18;
    static readonly int strikeTickCooldown = 60;

    static readonly Vector2 strikeUp = new Vector2(1, .75f);
    static readonly Vector2 strikeDown = new Vector2(1, -.75f);
    static readonly Vector2 strikeStraight = Vector2.right;

    public Box[] walls;
    public Ball ball;
    public Player player1;
    public Player player2;
    public Box strikeHitboxP1;
    public Box strikeHitboxP2;

    bool canLog = false;
    int ticks = 0;

    public Game(bool log = false)
    {
        canLog = log;
    }

    public Game(Game g)
    {
        ticks = g.ticks;

        player1 = new Player(g.player1);
        player2 = new Player(g.player2);
        ball = new Ball(g.ball);
        walls = new Box[g.walls.Length];

        for (int i = 0; i < g.walls.Length; i++)
            walls[i] = new Box(g.walls[i]);
    }

    public void Tick()
    {
        if (IsFinished()) return;

        ComputePhysics();

        EvaluateCooldowns(ref player1);
        EvaluateCooldowns(ref player2);

        ticks++;
    }

    public PlayerTag GetResult()
    {
        if (!IsFinished()) return PlayerTag.None;
        return player1.hp > 0 ? PlayerTag.One : PlayerTag.Two;
    }

    public bool IsFinished()
    {
        return player1.hp <= 0 || player2.hp <= 0;
    }

    public void PlayAction(Action a, PlayerTag t)
    {
        if (t == player1.tag) player1.action = a;
        if (t == player2.tag) player2.action = a;
    }

    public List<Action> GetPossibleActions(PlayerTag tag)
    {
        Player p = tag == player1.tag ? player1 : player2;
        List<Action> actions = new List<Action>() {
            Action.Idle,
            Action.MoveLeft,
            Action.MoveRight
        };

        if (p.strikeHitbox.canBeEnabled)
        {
            actions.Add(Action.StrikeUp);
            actions.Add(Action.StrikeDown);
            actions.Add(Action.StrikeStraight);
        }

        if (!p.isAirborne) actions.Add(Action.Jump);

        return actions;
    }

    void DebugLog(object message)
    {
        if (canLog) Debug.Log(message);
    }

    void ApplyStrikeAction(ref Player p, Direction d)
    {
        if (p.strikeHitbox.canBeEnabled)
        {
            DebugLog($"Player {p.tag} is striking {d}");
            p.strikeHitbox.isEnabled = true;
            p.strikeHitbox.canBeEnabled = false;
            p.strikeHitbox.lastActivationTick = ticks;
            p.strikeHitbox.direction = d;
        }
    }

    void ApplyAction(ref Player p)
    {
        switch (p.action)
        {
            case Action.Idle: return;
            case Action.MoveLeft:
                p.velocity.x = -Constants.PLAYER_SPEED;
                p.facing = Facing.Left;
                break;
            case Action.MoveRight:
                p.velocity.x = Constants.PLAYER_SPEED;
                p.facing = Facing.Right;
                break;
            case Action.StrikeUp:
                ApplyStrikeAction(ref p, Direction.Up);
                break;
            case Action.StrikeDown:
                ApplyStrikeAction(ref p, Direction.Down);
                break;
            case Action.StrikeStraight:
                ApplyStrikeAction(ref p, Direction.Straight);
                break;
            case Action.Jump:
                if (!p.isAirborne) p.velocity.y = Constants.JUMP_FORCE;
                break;
        }

        p.action = Action.Idle;
    }

    void ApplyGravityAndFriction(ref Player p)
    {
        if (p.velocity.x != 0)
        {
            float abs = Mathf.Abs(p.velocity.x);
            p.velocity.x = Mathf.Sign(p.velocity.x) * (abs - Constants.MOVE_FRICTION);
            if (abs < Constants.MOVE_FRICTION) p.velocity.x = 0;
        }
        if (p.isAirborne) p.velocity.y -= Constants.GRAVITY_FORCE * Time.fixedDeltaTime;
    }

    void CheckBallPlayerColliding(Ball b, ref Player p)
    {
        if (
            b.lastHitBy != PlayerTag.None &&
            b.lastHitBy != p.tag &&
            p.canTakeDamage &&
            Geometry.IsColliding(b, p)
        )
        {
            DebugLog($"Player {p.tag} lost HP");
            p.hp--;
            p.lastHitTick = ticks;
            p.canTakeDamage = false;
        }
    }

    void CheckBallStrikeHitboxColliding(ref Ball b, ref Player p)
    {
        if (p.strikeHitbox.isEnabled)
        {
            float facingDirection = p.facing == Facing.Left ? -1 : 1;

            Vector2 offset = Constants.STRIKE_HITBOX_OFFSET;

            offset.x *= facingDirection;

            p.strikeHitbox.box.position = p.box.position + offset;

            if (Geometry.IsColliding(b, p.strikeHitbox.box))
            {
                DebugLog($"Player {p.tag} struck the ball");

                Vector2 direction;

                switch (p.strikeHitbox.direction)
                {
                    case Direction.Up:
                        direction = strikeUp;
                        break;
                    case Direction.Down:
                        direction = strikeDown;
                        break;
                    default:
                        direction = strikeStraight;
                        break;
                }

                direction.x *= facingDirection;

                ball.speed = Mathf.Min(
                    ball.speed + Constants.BALL_STEP,
                    Constants.BALL_MAX_SPEED
                );
                ball.velocity = direction * ball.speed;
                ball.lastHitBy = p.tag;

                p.strikeHitbox.isEnabled = false;
            }
        }
    }

    void CheckBallWallColliding(ref Ball b, Box w)
    {
        if (Geometry.IsColliding(b, w))
        {
            DebugLog($"Ball colliding: {b.circle.position}, {w.position}");

            b.velocity = Vector2.Reflect(b.velocity, Geometry.GetCollisionNormal(b, w));
        }
    }

    void CheckPlayerWallColliding(ref Player p, Box w)
    {
        if (Geometry.IsColliding(p, w))
        {
            DebugLog($"Player {p.tag} colliding: {p.box.position}, {w.position}");

            Vector2 normal = Geometry.GetCollisionNormal(p.box.position, w.position, w.scale);

            float x = p.velocity.x;
            float y = p.velocity.y;

            if (normal.y != 0)
            {
                if (normal.y < 0 && y <= 0)
                {
                    y = 0;
                    p.isAirborne = false;
                }
                else if (normal.y > 0 && y >= 0) y = 0;
            }
            else if (
                normal.x < 0 && x <= 0 ||
                normal.x > 0 && x >= 0
            ) x = 0;

            p.velocity.x = x;
            p.velocity.y = y;
        }
    }

    void ComputePhysics()
    {
        // Checker collisions balle avec joueurs

        CheckBallPlayerColliding(ball, ref player1);
        CheckBallPlayerColliding(ball, ref player2);

        // Appliquer gravité et friction aux joueurs

        ApplyGravityAndFriction(ref player1);
        ApplyGravityAndFriction(ref player2);

        // Consommer les actions des joueurs

        ApplyAction(ref player1);
        ApplyAction(ref player2);

        // Checker collisions balle avec strike hitbox

        CheckBallStrikeHitboxColliding(ref ball, ref player1);
        CheckBallStrikeHitboxColliding(ref ball, ref player2);

        // Checker collisions avec murs

        player1.isAirborne = true;
        player2.isAirborne = true;

        for (int i = 0; i < walls.Length; i++)
        {
            // Checker collisions balle avec murs et assigner nouvelle vélocité

            CheckBallWallColliding(ref ball, walls[i]);

            // Checker collisions joueurs avec murs et assigner vélocité

            CheckPlayerWallColliding(ref player1, walls[i]);
            CheckPlayerWallColliding(ref player2, walls[i]);
        }

        // Assigner nouvelle position de la balle

        ball.circle.position = Geometry.GetNextPosition(ball, Time.fixedDeltaTime);

        // Assigner nouvelles positions des joueurs

        player1.box.position = Geometry.GetNextPosition(player1, Time.fixedDeltaTime);
        player2.box.position = Geometry.GetNextPosition(player2, Time.fixedDeltaTime);
    }

    void EvaluateCooldowns(ref Player p)
    {
        if (
            !p.canTakeDamage &&
            ticks - p.lastHitTick > hitTickCooldown
        ) p.canTakeDamage = true;
        if (
            p.strikeHitbox.isEnabled &&
            ticks - p.strikeHitbox.lastActivationTick > strikeTickLifetime
        ) p.strikeHitbox.isEnabled = false;
        if (
            !p.strikeHitbox.canBeEnabled &&
            ticks - p.strikeHitbox.lastActivationTick > strikeTickCooldown
        ) p.strikeHitbox.canBeEnabled = true;
    }
}
