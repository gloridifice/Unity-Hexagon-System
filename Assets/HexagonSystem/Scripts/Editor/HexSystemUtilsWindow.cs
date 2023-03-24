using UnityEditor;
using UnityEngine;

namespace HexSystem.Utils
{
    public class HexSystemUtilsWindow : EditorWindow
    {
        [MenuItem("Hex System/Utils")]
        private static void ShowWindow()
        {
            var window = GetWindow<HexSystemUtilsWindow>();
            window.titleContent = new GUIContent("Hex Utils");
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}