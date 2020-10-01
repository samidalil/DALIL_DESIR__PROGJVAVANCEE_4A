using UnityEngine;

public static class Geometry
{
    /* Rectangle vs Rectangle */
    public static bool IsColliding(Vector2 b1p, Vector2 b1s, Vector2 b2p, Vector2 b2s)
    {
        float dw1 = b1s.x / 2;
        float dh1 = b1s.y / 2;
        float dw2 = b2s.x / 2;
        float dh2 = b2s.y / 2;

        return b1p.x + dw1 >= b2p.x - dw2
            && b1p.x - dw1 <= b2p.x + dw2
            && b1p.y + dh1 >= b2p.y - dh2
            && b1p.y - dh1 <= b2p.y + dh2;
    }

    public static bool IsColliding(Box b1, Box b2)
    {
        return IsColliding(b1.position, b1.scale, b2.position, b2.scale);
    }

    public static bool IsColliding(Player p, Box b)
    {
        return IsColliding(p.box, b);
    }

    /* Cercle vs Rectangle, problème de bounds à corriger */
    public static bool IsColliding(Vector2 c, float r, Vector2 bp, Vector2 bs)
    {
        float xPoint = c.x;
        float yPoint = c.y;
        float dw = bs.x / 2;
        float dh = bs.y / 2;

        if (c.x < bp.x - dw) xPoint = bp.x - dw;
        else if (c.x > bp.x + dw) xPoint = bp.x + dw;

        if (c.y < bp.y - dh) yPoint = bp.y - dh;
        else if (c.y > bp.y + dh) yPoint = bp.y + dh;

        float xDist = c.x - xPoint;
        float yDist = c.y - yPoint;

        return xDist * xDist + yDist * yDist <= r * r;
    }

    public static bool IsColliding(Circle c, Box b)
    {
        return IsColliding(c.position, c.radius, b.position, b.scale);
    }

    public static bool IsColliding(Ball c, Box b)
    {
        return IsColliding(c.circle, b);
    }

    public static bool IsColliding(Ball b, Player p)
    {
        return IsColliding(b.circle, p.box);
    }



    /* Tout vs Rectangle, valide seulement s'ils sont en collision d'après leurs fonctions respectives */
    public static Vector2 GetContactPoint(Vector2 p, Vector2 bp, Vector2 bs)
    {
        Vector2 direction = p - bp;
        Vector2 ux = Vector2.right;
        Vector2 uy = Vector2.up;

        float ex = bs.x / 2;
        float ey = bs.y / 2;

        float dx = Vector2.Dot(direction, ux);

        if (dx > ex) dx = ex;
        if (dx < -ex) dx = -ex;

        float dy = Vector2.Dot(direction, uy);

        if (dy > ey) dy = ey;
        if (dy < -ey) dy = -ey;

        return bp + dx * ux + dy * uy;
    }

    public static Vector2 GetCollisionNormal(Vector2 p, Vector2 bp, Vector2 bs)
    {
        return (GetContactPoint(p, bp, bs) - p).normalized;
    }

    public static Vector2 GetCollisionNormal(Circle c, Box b)
    {
        return GetCollisionNormal(c.position, b.position, b.scale);
    }

    public static Vector2 GetCollisionNormal(Ball c, Box b)
    {
        return GetCollisionNormal(c.circle, b);
    }

    public static Vector2 GetNextPosition(Vector2 position, Vector2 velocity, float deltaTime)
    {
        return position + velocity * deltaTime;
    }

    public static Vector2 GetNextPosition(Ball b, float deltaTime)
    {
        return GetNextPosition(b.circle.position, b.velocity, deltaTime);
    }

    public static Vector2 GetNextPosition(Player p, float deltaTime)
    {
        return GetNextPosition(p.box.position, p.velocity, deltaTime);
    }
}
