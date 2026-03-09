using UnityEngine;
using System.Collections.Generic;

namespace TriNodo.Core
{
    public static class ValidationService
    {
        /// <summary>
        /// Comprueba si dos segmentos de línea (A1-A2) y (B1-B2) se intersectan.
        /// </summary>
        public static bool Intersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            // Si comparten algún extremo, no se consideran "cruzados" en este juego
            if (Vector2.Distance(a1, b1) < 0.01f || Vector2.Distance(a1, b2) < 0.01f || 
                Vector2.Distance(a2, b1) < 0.01f || Vector2.Distance(a2, b2) < 0.01f) return false;

            float d = (a2.x - a1.x) * (b2.y - b1.y) - (a2.y - a1.y) * (b2.x - b1.x);
            if (Mathf.Abs(d) < 0.0001f) return false; // Paralelas con margen de error

            float u = ((b1.x - a1.x) * (b2.y - b1.y) - (b1.y - a1.y) * (b2.x - b1.x)) / d;
            float v = ((b1.x - a1.x) * (a2.y - a1.y) - (b1.y - a1.y) * (a2.x - a1.x)) / d;

            // Usamos un pequeño margen de error (0.01) para evitar líneas que se toquen tangencialmente
            return u > 0.01f && u < 0.99f && v > 0.01f && v < 0.99f;
        }

        /// <summary>
        /// Comprueba si un punto P está dentro de un triángulo formado por A, B y C.
        /// </summary>
        public static bool IsPointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = Sign(p, a, b);
            d2 = Sign(p, b, c);
            d3 = Sign(p, c, a);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }

        private static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }
    }
}
