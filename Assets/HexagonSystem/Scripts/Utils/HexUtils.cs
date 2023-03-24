using System.Collections.Generic;
using HexSystem.Behaviours;
using UnityEditor;
using UnityEngine;

namespace HexSystem.Utils
{
    public class HexUtils
    {
        public static Material LineMaterial
        {
            get
            {
                if (lineMaterial == null)
                {
                    lineMaterial = new(Shader.Find("Hidden/Internal-Colored"));
                }
                return lineMaterial;
            }
        } 
        public static Material lineMaterial = new(Shader.Find("Hidden/Internal-Colored"));
        public static float sqrt3 = 1.73205080757f;

        #region Grid

        public static Vector3 HexPos2LocalWorldPos(HexGrid grid, HexPos pos)
        {
            float size = grid.hexSize;
            float vert = sqrt3 * size;
            float horiz = 1.5f * size;
            float x = pos.q * horiz;
            float z = pos.r * vert + -pos.q * vert / 2;
            float y = pos.h * grid.hexHeight;


            return new Vector3(x, y, z);
        }

        public static HexPos LocalWorldPos2HexPos(HexGrid grid, Vector3 localWorldPos)
        {
            float size = grid.hexSize;
            float horiz = 1.5f * size;
            float vert = sqrt3 * size;
            int q = Mathf.RoundToInt(localWorldPos.x / horiz);
            int r = Mathf.RoundToInt((localWorldPos.z / vert + (float)q / 2));
            int h = Mathf.RoundToInt(localWorldPos.y);
            return new HexPos(q, r, h);
        }

        /// <summary>
        /// 将世界坐标限制到六边形网络的位置
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="localWorldPos"></param>
        /// <returns></returns>
        public static Vector3 ClampToGrid(HexGrid grid, Vector3 localWorldPos)
        {
            return HexPos2LocalWorldPos(grid, LocalWorldPos2HexPos(grid, localWorldPos));
        }

        /// <summary>
        /// 获得六边形网络一定半径内的所有点的集合
        /// </summary>
        /// <param name="grid">网络</param>
        /// <param name="origin">原点</param>
        /// <param name="radius">半径, 0是只有原点</param>
        /// <returns></returns>
        public static Dictionary<HexPos, Vector3> GetSpiralRingsPoints(HexGrid grid, HexPos origin, int radius)
        {
            Dictionary<HexPos, Vector3> points = new Dictionary<HexPos, Vector3>();
            points.Add(origin, HexPos2LocalWorldPos(grid, origin));
            for (int r = 1; r < radius; r++)
            {
                HexPos pos = HexPos.qPlus * r + origin;
                //points.Add(pos, HexUtils.HexPos2LocalWorldPos(grid, pos));
                for (int dirt = 0; dirt < 6; dirt++)
                {
                    for (int step = 0; step < r; step++)
                    {
                        pos += HexPos.Directions2D[dirt];
                        if (!points.ContainsKey(pos))
                        {
                            points.Add(pos, HexPos2LocalWorldPos(grid, pos));
                        }
                    }
                }
            }

            return points;
        }

        #endregion

        #region Interaction

        public static Vector3 ScreenToLocal(Camera camera, Vector2 screenPosition, Plane plane)
        {
            Ray ray;
            if (camera.orthographic)
            {
                Vector2 screen = EditorGUIUtility.PointsToPixels(screenPosition);
                screen.y = camera.pixelHeight - screen.y;
                Vector3 cameraWorldPoint = camera.ScreenToWorldPoint(screen);
                ray = new Ray(cameraWorldPoint, camera.transform.forward);
            }
            else
            {
                ray = HandleUtility.GUIPointToWorldRay(screenPosition);
            }

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HexTile hexTile = hit.transform.GetComponentInParent<HexTile>();
                if (hexTile != null)
                {
                    return hit.point;
                }
            }

            float result;
            plane.Raycast(ray, out result);
            Vector3 world = ray.GetPoint(result);
            return world;
            //return transform.InverseTransformPoint(world);
        }

        #endregion

        #region Drawing

        public static void DrawBatchedLine(Vector3 p1, Vector3 p2)
        {
            GL.Vertex3(p1.x, p1.y, p1.z);
            GL.Vertex3(p2.x, p2.y, p2.z);
        }

        public static void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            GL.PushMatrix();
            GL.MultMatrix(GUI.matrix);
            LineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);
            DrawBatchedLine(p1, p2);
            GL.End();
            GL.PopMatrix(); 
        }

        public static void DrawGrid(HexGrid grid, Vector3 center, float size, Color color)
        {
            Vector3 hexCamPos = center;

            Vector3 a = new Vector3(HexUtils.sqrt3, 0f, 1f) * 50f;
            Vector3 b = new Vector3(0, 0, 2) * 50f;
            Vector3 c = new Vector3(-HexUtils.sqrt3, 0f, 1f) * 50f;
            //Color color = new Color(.5f, .5f, .5f, .5f);
            for (int i = -50; i <= 50; i++)
            {
                Vector3 originAC = hexCamPos + i * size * HexUtils.sqrt3 * Vector3.forward;
                Vector3 originB = hexCamPos + i * size * 1.5f * Vector3.right;
                DrawLine(originAC - a, originAC + a, color);
                DrawLine(originAC - c, originAC + c, color);
                DrawLine(originB - b, originB + b, color);
            }
        }

        public static void DrawPolygon(Vector3 center, int edge, float size, Color color)
        {
            for (int i = 0; i < edge; i++)
            {
                float angle0 = i * 2 * Mathf.PI / edge;
                float angle1 = (i + 1) * 2 * Mathf.PI / edge;
                Vector3 p0 = new Vector3(Mathf.Cos(angle0), 0, Mathf.Sin(angle0)) * size;
                Vector3 p1 = new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * size;

                DrawLine(center + p0, center + p1, color);
            }
        }

        #endregion
    }
}