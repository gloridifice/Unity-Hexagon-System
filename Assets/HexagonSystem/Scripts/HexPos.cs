using HexSystem.Utils;
using UnityEngine;

namespace HexSystem
{
    public struct HexPos
    {
        private Vector3Int vector;

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

        public int s
        {
            get { return (-q - r); }
        }

        public int h
        {
            get { return vector.z; }
            set { vector.z = value; }
        }

        #region Construction

        public HexPos(int q, int r, int height)
        {
            vector = new Vector3Int(q, r, height);
        }

        /// <param name="vector">In Vector: x = q, y = r, z = height</param>
        public HexPos(Vector3Int vector)
        {
            this.vector = vector;
        }
        public HexPos(TriPos triPos)
        {
            this.vector = triPos.vector;
        }

        #endregion

        public static DirectionType GetDirectionType(HexPos pos)
        {
            if (pos == qPlus) return DirectionType.Q_PLUS;
            if (pos == qMinus) return DirectionType.Q_MINUS;
            if (pos == rPlus) return DirectionType.R_PLUS;
            if (pos == rMinus) return DirectionType.R_MINUS;
            if (pos == sPlus) return DirectionType.S_PLUS;
            if (pos == sMinus) return DirectionType.S_MINUS;
            if (pos == up) return DirectionType.UP;
            if (pos == down) return DirectionType.DOWN;
            return DirectionType.ZERO;
        }

        public static HexPos GetDirectionHexPos(DirectionType type)
        {
            switch (type)
            {
                case DirectionType.Q_PLUS: return qPlus;
                case DirectionType.Q_MINUS: return qMinus;
                case DirectionType.R_PLUS: return rPlus;
                case DirectionType.R_MINUS: return rMinus;
                case DirectionType.S_PLUS: return sPlus;
                case DirectionType.S_MINUS: return sMinus;
                case DirectionType.UP: return up;
                case DirectionType.DOWN: return down;
                default: return zero;
            }
        }

        //Axis counterpoint
        public static readonly HexPos
            qPlus = new(0, +1, 0),
            qMinus = new(0, -1, 0),
            sMinus = new(1, 1, 0),
            sPlus = new(-1, -1, 0),
            rPlus = new(+1, 0, 0),
            rMinus = new(-1, 0, 0),
            up = new(0, 0, 1),
            down = new(0, 0, -1),
            zero = new(0, 0, 0);

        public static readonly HexPos[] Directions2D = { rPlus, qPlus, sPlus, rMinus, qMinus, sMinus };
        public static readonly HexPos[] Directions3D = { rPlus, qPlus, sPlus, rMinus, qMinus, sMinus, up, down };

        public static HexPos operator +(HexPos a, HexPos b) => new HexPos(a.vector + b.vector);
        public static HexPos operator -(HexPos a, HexPos b) => new HexPos(a.vector - b.vector);
        public static HexPos operator *(HexPos a, int b) => new HexPos(a.vector * b);
        public static HexPos operator *(int b, HexPos a) => new HexPos(a.vector * b);
        public static HexPos operator /(HexPos a, int b) => new HexPos(a.vector / b);
        public static bool operator ==(HexPos a, HexPos b) => a.vector == b.vector;
        public static bool operator !=(HexPos a, HexPos b) => a.vector != b.vector;

        public override int GetHashCode()
        {
            return vector.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is HexPos && vector.Equals(((HexPos)obj).vector);
        }

        public override string ToString()
        {
            return "(q-r-h)(" + q + "," + r + ","  + h + ")";
        }

        public enum DirectionType
        {
            R_PLUS,
            Q_MINUS,
            S_PLUS,
            R_MINUS,
            Q_PLUS,
            S_MINUS,
            UP,
            DOWN,
            ZERO
        }
    }
}