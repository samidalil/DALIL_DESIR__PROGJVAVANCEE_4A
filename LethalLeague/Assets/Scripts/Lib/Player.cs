using UnityEngine;

public enum PlayerTag
{
    None,
    One,
    Two,
};

public enum Facing
{
    Left,
    Right,
};

public struct Player
{
    public Player(Vector2 position, PlayerTag playerTag)
    {
        box = new Box(position, new Vector2(1, 2));
        velocity = Vector2.zero;
        action = Action.Idle;
        facing = position.x > 0 ? Facing.Left : Facing.Right;
        tag = playerTag;
        hp = 5;
        isAirborne = false;
        isStriking = false;
        canTakeDamage = true;
        lastHitTick = 0;
        strikeHitbox = new StrikeHitbox();
    }

    public Player(Player p)
    {
        box = new Box(p.box);
        velocity = p.velocity;
        action = p.action;
        facing = p.facing;
        tag = p.tag;
        hp = p.hp;
        isAirborne = p.isAirborne;
        isStriking = p.isStriking;
        canTakeDamage = p.canTakeDamage;
        lastHitTick = p.lastHitTick;
        strikeHitbox = p.strikeHitbox;
    }

    public Box box;
    public Vector2 velocity;
    public Action action;
    public Facing facing;
    public PlayerTag tag;
    public int hp;
    public bool isAirborne;
    public bool isStriking;
    public bool canTakeDamage;
    public float lastHitTick;
    public StrikeHitbox strikeHitbox;
}
