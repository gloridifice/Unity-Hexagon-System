using System;
using HexSystem.Behaviours;
using Unity.VisualScripting;
using UnityEngine;

namespace HexSystem.Utils
{
    public struct TriPos
    {
        public Vector3Int vector;
        public bool up;

        //三角形坐标系以六边形坐标系的 q,r 为轴，划分成四边形坐标系，再用 up 来划分成三角形
        //每个六边形的坐标都对应了其右上角的四边形的坐标
        //世界坐标中，只要将 (x, z) + ((sqrt3)/4 * size, 0.75f * size) 就得到了四边形坐标
        public int q
        {
            get { return vector.x; }
            set { vector.x = value; }
        }

        public int r
        {
            get { return vector.y; }
            set { vector.y = value; }
        }

        public int h
        {
            get { return vector.z; }
            set { vector.z = value; }
        }

        public TriPos Back => up ? new TriPos(vector, !up) : new TriPos(q, r - 1, h, !up);
        public TriPos Forward => up ? new TriPos(q, r + 1, h, !up) : new TriPos(vector, !up);
        public TriPos Left => up ? new TriPos(q - 1, r, h, !up) : new TriPos(q - 1, r - 1, h, !up);
        public TriPos Right => up ? new TriPos(q + 1, r + 1, h, !up) : new TriPos(q + 1, r, h, !up);
        public TriPos Up => new(q, r, h + 1, up);
        public TriPos Down => new(q, r, h - 1, up);

        public TriPos(Vector3Int vector, bool up)
        {
            this.vector = vector;
            this.up = up;
        }

        public TriPos(HexPos hexPos, bool up) : this()
        {
            q = hexPos.q;
            r = hexPos.r;
            h = hexPos.h;
            this.up = up;
        }

        public TriPos(int q, int r, int h, bool up) : this()
        {
            this.q = q;
            this.r = r;
            this.h = h;
            this.up = up;
        }

        /// <summary>
        /// 获取相邻的六边形网络是否有六边形，从 z 轴正向最大的顶点开始，顺时针排序
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public bool[] GetVertexStates(HexGrid grid)
        {
            bool[] hasVertexes = new bool[3];
            HexPos[] hexPosArray = GetHexNeighbours();
            for (int i = 0; i < 3; i++)
            {
                hasVertexes[i] = grid.HexTiles.ContainsKey(hexPosArray[i]);
            }

            return hasVertexes;
        }

        /// <summary>
        /// 获取相邻的六边形的数量
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public int GetVertexCount(HexGrid grid)
        {
            int a = 0;
            foreach (var flag in GetVertexStates(grid))
            {
                if (flag)
                {
                    a++;
                }
            }

            return a;
        }
        public int GetVertexCount(bool[] hasVertexes)
        {
            int a = 0;
            foreach (var flag in hasVertexes)
            {
                if (flag)
                {
                    a++;
                }
            }

            return a;
        }

        /// <summary>
        /// 获取相邻的三个三角形的位置，顺序为从 z 轴坐标最大的开始，顺时针
        /// </summary>
        /// <returns>三个三角形位置数组</returns>
        public TriPos[] GetTriNeighbours()
        {
            TriPos a = this, b, c;
            a.up = !a.up;
            b = a;
            c = a;
            if (up)
            {
                b.q--;
                c.r++;
                return new[] { c, a, b };
            }
            else
            {
                b.r--;
                c.q++;
                return new[] { a, c, b };
            }
        }

        /// <summary>
        /// 获取相邻的三个六边形的位置，顺序为从 z 轴坐标最大的开始，顺时针
        /// </summary>
        /// <returns>三个六边形位置的数组</returns>
        public HexPos[] GetHexNeighbours()
        {
            HexPos hexPos0 = new HexPos(this);
            HexPos hexPos1 = hexPos0;
            HexPos hexPos2 = hexPos0;
            if (up)
            {
                hexPos1 += HexPos.qPlus;
                hexPos2 += HexPos.sMinus;
                return new[] { hexPos1, hexPos2, hexPos0 };
            }
            else
            {
                hexPos1 += HexPos.sMinus;
                hexPos2 += HexPos.rPlus;
                //Debug.Log(hexPos1 + " / " + hexPos2 +" / "+ hexPos0);
                return new[] { hexPos1, hexPos2, hexPos0 };
            }
        }

        #region Override from Object
        public override bool Equals(object obj)
        {
            if (obj is TriPos triPos)
            {
                return triPos.vector == vector && triPos.up == up;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return vector.GetHashCode();
        }

        public override string ToString()
        {
            string a = up ? "up" : "down";
            return "( " + q + ", " + r + ", " + h + ", " + a + " )";
        }
        #endregion

    }
}