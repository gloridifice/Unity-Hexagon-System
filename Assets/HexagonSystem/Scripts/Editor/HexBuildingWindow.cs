using System;
using System.Collections.Generic;
using HexSystem.Behaviours;
using HexSystem.Slot;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace HexSystem.Utils
{
    public class HexBuildingWindow : EditorWindow
    {
        
        [MenuItem("Hex System/Hexagon Building Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<HexBuildingWindow>();
            window.titleContent = new GUIContent("Hexagon Building Window");
            window.Show();
        }

        private Vector3 pointerTargetPos;
        private Vector3 pointerPos;
        private HexPos pointedHexPos;
        private int controlID;
        private int heightBuffer;

        private SlotModuleManager slotModuleManager = new ();

        private HexGrid HexGrid => (Selection.activeGameObject != null
            ? Selection.activeGameObject.GetComponentInParent<HexGrid>()
            : null);

        private GameObject baseHexagonPrefab;
        
        private AssetReference baseHexagonPrefabReference = new AssetReferenceGameObject("Assets/HexagonSystem/Prefabs/p_BaseHexTile.prefab"); 
        
        private void OnEnable()
        {
            SceneView.duringSceneGui += DuringSceneGui;
            pointerPos = pointerTargetPos;
            LoadAssets();
            slotModuleManager.Init();
            controlID = GUIUtility.GetControlID(FocusType.Keyboard);
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= DuringSceneGui;
            //slotModuleManager.Release();
            //ReleaseAssets(); 
        }

        private async void LoadAssets()
        {
            baseHexagonPrefab = await baseHexagonPrefabReference.LoadAssetAsync<GameObject>().Task;
        }

        private void ReleaseAssets()
        {
            baseHexagonPrefabReference.ReleaseAsset();
        }
        void DuringSceneGui(SceneView sceneView)
        {
            if (HexGrid == null) return;
            DrawGridAndPointer();
            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
            {
                if (!Event.current.alt && Event.current.button == 0)
                {
                    AddTileInteraction();
                }

                if (!Event.current.alt && Event.current.button == 1)
                {
                    RemoveTileInteraction();
                }
            }

            if (Event.current.type == EventType.MouseUp)
            {
                heightBuffer = Int32.MinValue;
            }
        }

        void DrawGridAndPointer()
        {
            Vector2 mousePos = Event.current.mousePosition;
            Vector3 targetPos = HexUtils.ScreenToLocal(Camera.current, mousePos, new Plane(Vector3.up, Vector3.zero));
            UpdatePointedHexPos(targetPos);
            Vector3 clampedPos = HexUtils.HexPos2LocalWorldPos(HexGrid, pointedHexPos);
            pointerTargetPos = clampedPos + Vector3.up * 0.2f;

            pointerPos = Vector3.Lerp(pointerPos, pointerTargetPos, 0.1f);

            HexUtils.DrawGrid(HexGrid, HexGrid.transform.position, HexGrid.hexSize, Color.gray);
            //HexUtils.DrawGrid(HexGrid, HexGrid.transform.position + Vector3.forward * HexGrid.hexSize * HexUtils.sqrt3/2 + Vector3.left * 0.375f, HexGrid.hexSize, new Color(0.5f,0.5f,0));
            HexUtils.DrawPolygon(pointerPos, 6, 0.4f, Color.white);
        }

        /// <summary>
        /// 更新 pointedHexPos
        /// 会自适应高度
        /// </summary>
        /// <param name="mouseWorldPos"></param>
        void UpdatePointedHexPos(Vector3 mouseWorldPos)
        {
            pointedHexPos = HexUtils.LocalWorldPos2HexPos(HexGrid, mouseWorldPos);
            while (HexGrid.HasTile(pointedHexPos))
            {
                pointedHexPos.h += 1;
            }

            if (heightBuffer != Int32.MinValue)
            {
                pointedHexPos.h = Mathf.Min(heightBuffer, pointedHexPos.h);
            }
        }

        void AddTileInteraction()
        {
            GUIUtility.hotControl = controlID; //停止其它事件交互
            if (Event.current.type == EventType.MouseDown) heightBuffer = pointedHexPos.h;
            HexGrid.AddTile(baseHexagonPrefab, pointedHexPos);
            Event.current.Use();
            Repaint();
        }

        void RemoveTileInteraction()
        {
            GUIUtility.hotControl = controlID; //停止其它事件交互
            if (Event.current.type == EventType.MouseDown) heightBuffer = pointedHexPos.h;
            HexPos pos = pointedHexPos;
            pos.h -= 1;
            HexGrid.RemoveTile(pos);
            Event.current.Use();
            Repaint();
        }

        private int genRadius;
        private float genPerlinScale;
        private float genHeightScale;

        private void PerlinNoiseGen(HexPos origin, float perlinScale, float heightScale, int genRadius)
        {
            Dictionary<HexPos, Vector3> points = HexUtils.GetSpiralRingsPoints(HexGrid, origin, genRadius);
            foreach (KeyValuePair<HexPos, Vector3> pair in points)
            {
                float x = pair.Value.x / perlinScale;
                float y = pair.Value.z / perlinScale;
                float noiseValue = Mathf.PerlinNoise(x, y) * heightScale;
                float height = Mathf.Round(noiseValue);
                for (int i = 0; i < height; i++)
                {
                    HexPos hp = pair.Key;
                    hp.h = i;
                    HexGrid.AddTile(baseHexagonPrefab, hp);
                }
            }
        }

        private void OnGUI()
        {
            if (HexGrid != null)
            {
                GUILayout.Label("HexGrid.HexTiles.Count: " + HexGrid.HexTiles?.Count);
                GUILayout.Label("HexGrid.TilesParent.childCount: " + HexGrid.TilesParent.childCount);
                if (GUILayout.Button("Clear"))
                {
                    HexGrid.ClearHexTiles();
                    HexGrid.SlotMap.ClearTriTiles();
                }

                genRadius = EditorGUILayout.IntField("Generation Radius", genRadius);
                genPerlinScale = EditorGUILayout.FloatField("Generation Perlin Scale", genPerlinScale);
                genHeightScale = EditorGUILayout.FloatField("Generation Height Scale", genHeightScale);

                if (GUILayout.Button("Gen by Perlin Noise"))
                {
                    PerlinNoiseGen(HexPos.zero, genPerlinScale, genHeightScale, genRadius);
                }
                
            }
        }
    }
}