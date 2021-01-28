using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entity.pathfinding
{
    public class Line
    {
        const float verticalLineGradient = 1e5f;

        float gradient;
        float y_intercept;
        Vector2 pointOnLine_1;
        Vector2 pointOnLine_2;

        float gradientPerpendicular;

        bool approachSide;

        public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
        {
            float dx = pointOnLine.X - pointPerpendicularToLine.X;
            float dy = pointOnLine.Y - pointPerpendicularToLine.Y;

            if (dx == 0)
            {
                gradientPerpendicular = verticalLineGradient;
            }
            else
            {
                gradientPerpendicular = dy / dx;
            }

            if (gradientPerpendicular == 0)
            {
                gradient = verticalLineGradient;
            }
            else
            {
                gradient = -1 / gradientPerpendicular;
            }

            y_intercept = pointOnLine.Y - gradient * pointOnLine.X;
            pointOnLine_1 = pointOnLine;
            pointOnLine_2 = pointOnLine + new Vector2(1, gradient);

            approachSide = false;
            approachSide = GetSide(pointPerpendicularToLine);
        }

        bool GetSide(Vector2 p)
        {
            return (p.X - pointOnLine_1.X) * (pointOnLine_2.Y - pointOnLine_1.Y) > (p.Y - pointOnLine_1.Y) * (pointOnLine_2.X - pointOnLine_1.X);
        }

        public bool HasCrossedLine(Vector2 p)
        {
            return GetSide(p) != approachSide;
        }

        public float DistanceFromPoint(Vector2 p)
        {
            float yInterceptPerpendicular = p.Y - gradientPerpendicular * p.X;
            float intersectX = (yInterceptPerpendicular - y_intercept) / (gradient - gradientPerpendicular);
            float intersectY = gradient * intersectX + y_intercept;
            return Vector2.Distance(p, new Vector2(intersectX, intersectY));
        }

        public void DrawWithGizmos(float length)
        {
            Vector3 lineDir = Vector3.Normalize(new Vector3(1, 0, gradient));
            Vector3 lineCentre = new Vector3(pointOnLine_1.X, 0, pointOnLine_1.Y) + new Vector3(0, 1, 0);
            //Gizmos.DrawLine(lineCentre - lineDir * length / 2f, lineCentre + lineDir * length / 2f);
        }
    }
}
