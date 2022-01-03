using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boxfriend.Utils
{

    /// <summary>
    /// Class to draw debug information such as physics2d casts
    /// </summary>
    public class PhysicsCastDebug
    {
        /// <summary>
        /// Casts a Physics2D BoxCast with debug lines drawn
        /// </summary>
        public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int mask)
        {
            var hit = Physics2D.BoxCast(origin, size, angle, direction, distance, mask);

            //Setting up the points to draw the cast
            Vector2[] point = new Vector2[8];
            var w = size.x * 0.5f;
            var h = size.y * 0.5f;
            point[0] = new Vector2(-w, h);
            point[1] = new Vector2(w, h);
            point[2] = new Vector2(w, -h);
            point[3] = new Vector2(-w, -h);

            var q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
            for (int i = 0; i < 4; i++)
            {
                point[i] = q * point[i];
                point[i] += origin;
            }
            

            var realDistance = direction.normalized * distance;
            point[4] = point[0] + realDistance;
            point[5] = point[1] + realDistance;
            point[6] = point[2] + realDistance;
            point[7] = point[3] + realDistance;

            
            //Drawing the cast
            var castColor = hit ? Color.green : Color.red;
            Debug.DrawLine(point[0], point[1], castColor);
            Debug.DrawLine(point[1], point[2], castColor);
            Debug.DrawLine(point[2], point[3], castColor);
            Debug.DrawLine(point[3], point[0], castColor);

            Debug.DrawLine(point[4], point[5], castColor);
            Debug.DrawLine(point[5], point[6], castColor);
            Debug.DrawLine(point[6], point[7], castColor);
            Debug.DrawLine(point[7], point[4], castColor);

            Debug.DrawLine(point[0], point[4], Color.grey);
            Debug.DrawLine(point[1], point[5], Color.grey);
            Debug.DrawLine(point[2], point[6], Color.grey);
            Debug.DrawLine(point[3], point[7], Color.grey);
            if (hit)
            {
                Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow);
            }

            return hit;
        }
    }
}