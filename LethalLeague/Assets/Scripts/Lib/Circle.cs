using UnityEngine;

public struct Circle
{
    public Circle(Vector2 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }

    public Circle(Circle c)
    {
        position = c.position;
        radius = c.radius;
    }

    public Vector2 position;
    public float radius;
}
