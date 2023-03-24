using System.Collections.Generic;
using System.Linq;
using HexSystem.Behaviours;
using HexSystem.Utils;
using UnityEngine;

namespace HexSystem.Slot
{
    public class SlotMap
    {
        private Dictionary<TriPos, TriTile> tiles;
        public HexGrid grid;
        private const string Tri_Tiles_Parent = "TriTiles";
        private Transform triTilesParent;
        public Transform TriTilesParent
        {
            get
            {
                if (triTilesParent == null)
                {
                    Transform triTiles = grid.transform.Find(Tri_Tiles_Parent);
                    if (triTiles != null)
                    {
                        triTilesParent = triTiles;
                    }
                    else
                    {
                        GameObject gameObject = new GameObject(Tri_Tiles_Parent);
                        gameObject.transform.parent = grid.transform;
                        triTilesParent = gameObject.transform;
                    }
                }
                return triTilesParent;
            }
        }

        public SlotMap(HexGrid grid)
        {
            tiles = new Dictionary<TriPos, TriTile>();
            this.grid = grid;

            //初始化集合
            foreach (Transform trans in TriTilesParent)
            {
                if (trans.TryGetComponent(out TriTile tile))
                {
                    if(!tiles.ContainsKey(tile.triPos)) tiles.Add(tile.triPos, tile);
                }
            }
        }

        public void Add(TriPos triPos, TriTile tile)
        {
            if (!tiles.ContainsKey(triPos))
            {
                tiles.Add(triPos, tile);
            }
            //Debug.LogWarning("There already have a obj in slot: [TriPos]" + triPos);
        }

        public void Remove(TriPos triPos)
        {
            if (!tiles.ContainsKey(triPos)) return;
            Object.DestroyImmediate(tiles[triPos].gameObject);
            tiles.Remove(triPos);
        }

        public void ClearTriTiles()
        {
            tiles.Clear();
            for (int i = TriTilesParent.childCount - 1; i >= 0 ; i--)
            {
                Object.DestroyImmediate(TriTilesParent.GetChild(i).gameObject);
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 刷新某一块三角形瓦片
        /// </summary>
        /// <param name="triPos"></param>
        public void Refresh(TriPos triPos)
        {
            //如果没有三角形，创建三角形
            bool[] vertexStates = triPos.GetVertexStates(grid);
            int vertexCount = triPos.GetVertexCount(vertexStates);

            if (!tiles.ContainsKey(triPos))
            {
                if (SlotModuleManager.Create(grid, triPos, vertexStates, out GameObject obj))
                {
                    Add(triPos, obj.GetComponent<TriTile>());
                }
                return;
            }

            //如果有三角形，更新三角形
            TriTile tile = tiles[triPos];
            if (vertexCount == 0)
            {
                Object.DestroyImmediate(tile.gameObject);
                return;
            }

            if (vertexCount == tile.lastVertexCount)
            {
                //顶点数相同且顶点位置相同: 不变
                if (Enumerable.SequenceEqual(vertexStates, tile.lastVertexStates)) return;

                //顶点数相同但顶点位置不同: 刷新旋转方向和邻居参数
                tile.RefreshSelf(vertexStates, vertexCount);
            }
            else
            {
                //顶点数不同: 删除并生成新的块
                Remove(triPos);
                if (SlotModuleManager.Create(grid, triPos, vertexStates, out GameObject gameObject))
                {
                    Add(triPos, gameObject.GetComponent<TriTile>());
                }
            }
        }
    }
}