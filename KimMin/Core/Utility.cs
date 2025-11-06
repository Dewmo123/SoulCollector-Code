
using UnityEngine;

namespace Work.Core
{
    public static class Utility
    {
        public static float GetAngleFromPos(Vector3 origin, Vector2 target)
        {
            float x = target.x - origin.x;
            float y = target.y - origin.y;
            return Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        }
        
        public static float DegreeToRadian(float angle)
        {
            return angle * Mathf.PI / 180;
        }

        public static Vector2 GetNewPoint(Vector3 start, float angleDeg, float dist)
        {
            float rad = angleDeg * Mathf.Deg2Rad;
            return new Vector2(
                start.x + Mathf.Cos(rad) * dist,
                start.y + Mathf.Sin(rad) * dist
            );
        }

        public static Vector2 Lerp(Vector2 start, Vector2 end, float t)
        {
            return start + t * (end - start);
        }

        public static Vector2 QuaternionCurve(Vector2 a, Vector2 b, Vector2 c, float t)
        {
            Vector2 p1 = Lerp(a, b, t);
            Vector2 p2 = Lerp(b, c, t);
            
            return Lerp(p1, p2, t);
        }
    }
}