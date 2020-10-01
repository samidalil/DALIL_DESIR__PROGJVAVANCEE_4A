using UnityEngine;

public struct Box
{
    public Box(Vector2 position, Vector2 scale)
    {
        this.position = position;
        this.scale = scale;
    }

    public Box(Box b)
    {
        position = b.position;
        scale = b.scale;
    }

    public Vector2 position;
    public Vector2 scale;
}
