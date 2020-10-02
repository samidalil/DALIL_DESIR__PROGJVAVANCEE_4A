using UnityEngine;

public static class Constants
{
    public static Vector2 BALL_DEFAULT_POS = Vector3.down * 3.5f;
    public static float BALL_DEFAULT_RADIUS = .5f;
    public static int BALL_MIN_SPEED = 3;
    public static int BALL_MAX_SPEED = 14;
    public static float BALL_STEP = 1.3f;

    public static int PLAYER_BASE_HP = 3;
    public static int PLAYER_SPEED = 7;
    public static int MOVE_FRICTION = 1;
    public static int JUMP_FORCE = 14;
    public static int GRAVITY_FORCE = 30;

    public static Vector2 STRIKE_HITBOX_OFFSET = new Vector2Int(1, 0);
    public static Vector2 STRIKE_HITBOX_SCALE = new Vector2(2, 1.75f);
}