using HexSystem;
using HexSystem.Slot;
using HexSystem.Utils;
using UnityEngine;

namespace HexSystem.Behaviours
{
    public class TriTile : MonoBehaviour
    {
        public TriPos triPos;
        public HexGrid grid;
        /// <summary>
        /// 邻居六边形网络是否有瓦片
        /// </summary>
        public bool[] lastVertexStates;
        /// <summary>
        /// 邻居六边形网络的瓦片数
        /// </summary>
        public int lastVertexCount;
        public void Init(TriPos triPos, HexGrid grid, bool[] vertexStates, int vertexCount)
        {
            this.grid = grid;
            this.triPos = triPos;
            lastVertexStates = vertexStates;
            lastVertexCount = vertexCount;
        }

        public void RefreshSelf(bool[] vertexStates, int vertexCount)
        {
            lastVertexStates = vertexStates;
            lastVertexCount = vertexCount;
            float angle = SlotModuleManager.GetRotateAngle(vertexCount - 1, vertexStates, triPos);
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.up, angle);
        }
    }
}