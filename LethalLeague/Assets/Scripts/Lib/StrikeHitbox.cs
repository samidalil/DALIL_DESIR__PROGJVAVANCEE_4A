using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Straight,
}

public struct StrikeHitbox
{
    public StrikeHitbox(int a = 0, bool b = false, Direction c = Direction.Straight)
    {
        lastActivationTick = a;
        isEnabled = b;
        canBeEnabled = true;
        direction = c;
        box = new Box(Vector2.zero, new Vector2(1, 1));
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
