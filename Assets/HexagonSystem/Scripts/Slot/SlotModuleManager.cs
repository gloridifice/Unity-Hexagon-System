using System;
using System.Collections.Generic;
using HexSystem.Behaviours;
using HexSystem.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace HexSystem.Slot
{
    public class SlotModuleManager
    {
        public static List<GameObject> slotPrefabs;
        private readonly AssetReference slotModulePrefabsReference0 = new AssetReferenceGameObject("Assets/HexagonSystem/Prefabs/p_BaseSlotModule_0.prefab");
        private readonly AssetReference slotModulePrefabsReference1 = new AssetReferenceGameObject("Assets/HexagonSystem/Prefabs/p_BaseSlotModule_1.prefab");
        private readonly AssetReference slotModulePrefabsReference2 = new AssetReferenceGameObject("Assets/HexagonSystem/Prefabs/p_BaseSlotModule_2.prefab");

        public async void Init()
        {
            slotPrefabs = new List<GameObject>();
            slotPrefabs.Add(await slotModulePrefabsReference0.LoadAssetAsync<GameObject>().Task);
            slotPrefabs.Add(await slotModulePrefabsReference1.LoadAssetAsync<GameObject>().Task);
            slotPrefabs.Add(await slotModulePrefabsReference2.LoadAssetAsync<GameObject>().Task);
        }

        public void Release()
        {
            slotModulePrefabsReference0.ReleaseAsset();
            slotModulePrefabsReference1.ReleaseAsset();
            slotModulePrefabsReference2.ReleaseAsset();
        }
        public static float GetRotateAngle(int index, bool[] hasVertexes, TriPos triPos)
        {
            float angle = 0;
            //+60
            if (!triPos.up)
            {
                angle += 60f;
            }

            if (index == 2)
            {
                //Do Nothing
            }
            else if (index == 0)
            {
                //+120
                int a = 0;
                for (int i = 0; i < hasVertexes.Length; i++)
                {
                    if (hasVertexes[i])
                    {
                        a = i;
                        break;
                    }
                }

                angle += a * 120f;
            }
            else if (index == 1)
            {
                //+120
                int a = 0;
                for (int i = 0; i < hasVertexes.Length; i++)
                {
                    if (!hasVertexes[i])
                    {
                        a = i;
                        break;
                    }
                }

                angle += a * 120f;
            }

            return angle;
        }

        public static bool Create(HexGrid grid, TriPos triPos, bool[] hasHexes, out GameObject gameObject)
        {
            gameObject = null;
            if (slotPrefabs == null)
            {
                Debug.LogError("slotPrefabs is null");
                return false;
            }
            int i = 0;
            foreach (var flag in hasHexes)
            {
                if (flag) i++;
            }
            if (i == 0)
            {
                return false;
            }
            gameObject = Object.Instantiate(slotPrefabs[i - 1]);
            Transform transform = gameObject.transform;
            transform.Rotate(Vector3.up, GetRotateAngle(i - 1, hasHexes, triPos));
            transform.position = TriUtils.TriPos2LocalWorldPos(grid, triPos);
            transform.parent = grid.SlotMap.TriTilesParent;
            TriTile tile = gameObject.AddComponent<TriTile>();
            tile.Init(triPos, grid, hasHexes, i);
            return true;
        }
    }
}