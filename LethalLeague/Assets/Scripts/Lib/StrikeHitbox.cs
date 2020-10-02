using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Straight,
}

public struct StrikeHitbox
{
    public StrikeHitbox(Vector2 pos)
    {
        lastActivationTick = 0;
        isEnabled = false;
        canBeEnabled = true;
        direction = Direction.Straight;
        box = new Box(pos, Constants.STRIKE_HITBOX_SCALE);
    }

    public StrikeHitbox(StrikeHitbox s)
    {
        lastActivationTick = s.lastActivationTick;
        isEnabled = s.isEnabled;
        canBeEnabled = s.canBeEnabled;
        direction = s.direction;
        box = s.box;
    }

    public int lastActivationTick;
    public bool isEnabled;
    public bool canBeEnabled;
    public Direction direction;
    public Box box;
};
