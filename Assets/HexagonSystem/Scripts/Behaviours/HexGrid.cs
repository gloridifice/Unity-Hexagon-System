using System;
using System.Collections.Generic;
using HexSystem.Slot;
using HexSystem.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = System.Object;

namespace HexSystem.Behaviours
{
    [ExecuteInEditMode]
    public class HexGrid : MonoBehaviour
    {
        public float hexSize = 0.5f;
        public float hexHeight = 0.4f;

        private const string Tiles_Parent_Name = "HexTiles";
        private Transform tilesParent;
        public Transform TilesParent
        {
            get
            {
                if (tilesParent == null)
                {
                    if (transform.Find(Tiles_Parent_Name) == null)
                    {
                        tilesParent = new GameObject(Tiles_Parent_Name).transform;
                        tilesParent.parent = this.transform;
                    }
                    else
                    {
                        tilesParent = transform.Find(Tiles_Parent_Name);
                    }
                }

                return tilesParent;
            }
        }
        
        private Dictionary<HexPos, HexTile> hexTiles;
        public Dictionary<HexPos, HexTile> HexTiles
        {
            get
            {
                if (hexTiles == null)
                {
                    hexTiles = new Dictionary<HexPos, HexTile>();
                    foreach (Transform tileTrans in TilesParent)
                    {
                        if (tileTrans.TryGetComponent(out HexTile hexTile))
                        {
                            if(!hexTiles.ContainsKey(hexTile.hexPos)) hexTiles.Add(hexTile.hexPos, hexTile);
                        }
                    }
                }

                return hexTiles;
            }
        }

        private SlotMap slotMap;

        public SlotMap SlotMap
        {
            get
            {
                if (slotMap == null)
                {
                    return new SlotMap(this);
                }
                return slotMap;
            }
        }

        public void AddTile(GameObject tileObj, HexPos hexPos)
        {
            if (HexTiles.ContainsKey(hexPos)) return;
            GameObject tileInstance = Instantiate(tileObj, TilesParent);
            if (tileInstance.TryGetComponent(out HexTile tile))
            {
                HexTiles.Add(hexPos, tile);
                tile.Init(this, hexPos);
            }
            
            //更新周围六个三角形
            RefreshTriSlots(hexPos);
        }
        public void RemoveTile(HexPos hexPos)
        {
            if (!HexTiles.ContainsKey(hexPos)) return;
            DestroyImmediate(HexTiles[hexPos].gameObject);
            HexTiles.Remove(hexPos);

            //刷新周围六个三角形
            RefreshTriSlots(hexPos);
        }

        public void RefreshTriSlots(HexPos pos)
        {
            TriPos[] triPosArray = TriUtils.GetTriNeighbourFromHexPos(pos);
            for (int i = 0; i < triPosArray.Length; i++)
            {
                SlotMap.Refresh(triPosArray[i]);
            }
        }

        public HexTile GetTile(HexPos pos)
        {
            return HexTiles[pos];
        }

        public bool HasTile(HexPos pos)
        {
            return HexTiles.ContainsKey(pos);
        }

        public void ClearHexTiles()
        {
            HexTiles.Clear();
            for (int i = TilesParent.childCount - 1; i >= 0 ; i--)
            {
                DestroyImmediate(TilesParent.GetChild(i).gameObject);
            }
        }
    }
}