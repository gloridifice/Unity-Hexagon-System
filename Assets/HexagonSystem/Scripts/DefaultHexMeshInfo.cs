using HexSystem;
using UnityEngine;

namespace HexSystem.Utils
{
    public class DefaultHexMeshInfo
    {
        public static readonly Vector3Int[] DefaultMeshFaces =
        {
            new(0,1,2),//0
            new(1,0,3),//1
            new(4,5,6),//2
            new(5,4,7),//3
            new(8,9,10),//4
            new(9,8,11),//5
            new(12,13,14),//6
            new(13,12,15),//7
            new(16,17,18),//8
            new(17,16,19),//9
            new(20,21,22),//10
            new(21,20,23),//11
            new(24,25,26),//12
            new(27,24,26),//13
            new(25,28,26),//14
            new(29,27,26),//15
            new(28,30,26),//16
            new(30,29,26),//17
            new(31,32,33),//18
            new(34,31,33),//19
            new(32,35,33),//20
            new(36,34,33),//21
            new(35,37,33),//22
            new(37,36,33),//23
        };

        public static readonly Vector3Int[]
            Q_PlusFaces = { DefaultMeshFaces[2], DefaultMeshFaces[3] },
            Q_MinusFaces = { DefaultMeshFaces[8], DefaultMeshFaces[9] },
            R_PlusFaces = { DefaultMeshFaces[10], DefaultMeshFaces[11] },
            R_MinusFaces = { DefaultMeshFaces[4], DefaultMeshFaces[5] },
            S_PlusFaces = { DefaultMeshFaces[6], DefaultMeshFaces[7] },
            S_MinusFaces = { DefaultMeshFaces[0], DefaultMeshFaces[1] },
            Up_Faces =
            {
                DefaultMeshFaces[12], DefaultMeshFaces[13], DefaultMeshFaces[14], DefaultMeshFaces[15],
                DefaultMeshFaces[16], DefaultMeshFaces[17]
            },
            Down_Faces =
            {
                DefaultMeshFaces[18], DefaultMeshFaces[19], DefaultMeshFaces[20], DefaultMeshFaces[21],
                DefaultMeshFaces[22], DefaultMeshFaces[23]
            };

        public static Vector3Int[] GetFaceFromDirection(HexPos.DirectionType direction)
        {
            switch (direction)
            {
                case HexPos.DirectionType.Q_PLUS: return Q_PlusFaces;
                case HexPos.DirectionType.Q_MINUS: return Q_MinusFaces;
                case HexPos.DirectionType.R_PLUS: return R_PlusFaces;
                case HexPos.DirectionType.R_MINUS: return R_MinusFaces;
                case HexPos.DirectionType.S_PLUS: return S_PlusFaces;
                case HexPos.DirectionType.S_MINUS: return S_MinusFaces;
                case HexPos.DirectionType.UP: return Up_Faces;
                case HexPos.DirectionType.DOWN: return Down_Faces;
            }

            return null;
        }
    }
}