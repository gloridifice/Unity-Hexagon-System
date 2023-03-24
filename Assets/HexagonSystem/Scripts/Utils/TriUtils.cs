using HexSystem.Behaviours;
using UnityEngine;

namespace HexSystem.Utils
{
    public class TriUtils
    {
        /// <summary>
        /// 返回一个六边形从右上角开始，顺时针排序的三角形邻居
        /// </summary>
        /// <param name="hexPos">六边形的位置</param>
        /// <returns></returns>
        public static TriPos[] GetTriNeighbourFromHexPos(HexPos hexPos)
        {
            TriPos triPos0 = new TriPos(hexPos, true);
            TriPos triPos1 = triPos0.Back;
            TriPos triPos2 = triPos1.Back;
            TriPos triPos3 = triPos2.Left;
            TriPos triPos4 = triPos3.Forward;
            TriPos triPos5 = triPos4.Forward;
            return new[] { triPos0, triPos1, triPos2, triPos3, triPos4, triPos5 };
        }

        public static Vector3 TriPos2LocalWorldPos(HexGrid grid, TriPos triPos)
        {
            float size = grid.hexSize;
            Vector3 pos = HexUtils.HexPos2LocalWorldPos(grid, new HexPos(triPos));
            return triPos.up ? pos + new Vector3(size * 0.5f, 0,HexUtils.sqrt3 * 0.5f * size) : pos + new Vector3(size, 0,0);
        }
    }
}