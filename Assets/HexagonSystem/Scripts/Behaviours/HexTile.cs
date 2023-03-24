using System.Collections.Generic;
using UnityEngine;
using HexSystem.Utils;
using UnityEditor;

namespace HexSystem.Behaviours
{
    public class HexTile : MonoBehaviour
    {
        public HexPos hexPos;
        public HexGrid grid;
        public MeshFilter meshFilter;

        // ReSharper disable Unity.PerformanceAnalysis
        public void Init(HexGrid grid, HexPos pos)
        {
            hexPos = pos;
            this.grid = grid;
            transform.position = HexUtils.HexPos2LocalWorldPos(grid, pos);
            meshFilter = GetComponentInChildren<MeshFilter>();
            //RefreshSelf();
            //RefreshNeighbor();
        }

        void PrintTriangles()
        {
            int[] tris = meshFilter.mesh.triangles;
            for (int i = 0; i < tris.Length / 3; i++)
            {
                //Debug.Log(i + ":( " + tris[i * 3]+", "+tris[i * 3 + 1]+", "+tris[i * 3 + 2]+" )");
            }
        }

        /// <summary>
        /// 全面更新自身的面剔除
        /// </summary>
        public void RefreshSelf()
        {
            HexPos center = hexPos;
            HexPos pos = center + HexPos.qPlus;
            List<HexPos.DirectionType> dirts = new List<HexPos.DirectionType>();
            for (int i = 0; i < 6; i++)
            {
                if (grid.HexTiles.TryGetValue(pos, out HexTile tile))
                {
                    dirts.Add(HexPos.GetDirectionType(pos - hexPos));
                }

                pos += HexPos.Directions2D[i];
            }

            CellOutFace(dirts.ToArray());
        }

        /// <summary>
        /// 当某个邻居更新时，更新自身的面剔除
        /// </summary>
        public void OnNeighbourChanged(HexPos neighbour, HexPos.DirectionType direction)
        {
            if (grid.HexTiles.ContainsKey(neighbour))
            {
                DeleteFace(direction);
            }
            else
            {
                AddFace(direction);
            }
        }

        /// <summary>
        /// 更新自身邻居的面剔除
        /// </summary>
        public void RefreshNeighbor()
        {
            HexPos center = hexPos;
            HexPos pos = center + HexPos.qPlus;
            for (int i = 0; i < 6; i++)
            {
                if (grid.HexTiles.TryGetValue(pos, out HexTile tile))
                {
                    tile.OnNeighbourChanged(center, (HexPos.DirectionType) i);
                }

                pos += HexPos.Directions2D[i];
            }
        }

        public void AddFace(HexPos.DirectionType direction)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3Int[] faces = DefaultHexMeshInfo.GetFaceFromDirection(direction);
            mesh.triangles = HexMeshUtils.AddFaces(mesh.triangles, faces);
        }

        public void DeleteFace(HexPos.DirectionType direction)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3Int[] faces = DefaultHexMeshInfo.GetFaceFromDirection(direction);
            mesh.triangles = HexMeshUtils.DeleteFaces(mesh.triangles, faces);
        }

        public void CellOutFace(HexPos.DirectionType[] types)
        {
            Mesh mesh = meshFilter.mesh;
            int[] tris = meshFilter.sharedMesh.triangles;

            foreach (var type in types)
            {
                Vector3Int[] faces = DefaultHexMeshInfo.GetFaceFromDirection(type);
                tris = HexMeshUtils.DeleteFaces(tris, faces);
            }

            mesh.triangles = tris;
        }
    }
}