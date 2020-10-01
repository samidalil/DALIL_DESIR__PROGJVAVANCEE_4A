using UnityEngine;

public struct Ball
{
    public Ball(Vector2 pos, float radius, float speed)
    {
        circle = new Circle(pos, radius);
        velocity = Vector2.right * speed;
        lastHitBy = PlayerTag.None;
        this.speed = speed;
    }

    public Ball(Ball b)
    {
        circle = new Circle(b.circle);
        velocity = b.velocity;
        lastHitBy = b.lastHitBy;
        speed = b.speed;
    }

    public Circle circle;
    public Vector2 velocity;
    public PlayerTag lastHitBy;
    public float speed;
}
