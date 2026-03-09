using UnityEditor;
using UnityEngine;
using TriNodo.Core;

namespace TriNodo.Editor
{
    public class GameDebuggerWindow : EditorWindow
    {
        [MenuItem("TriNodo/Ultra Debugger")]
        public static void ShowWindow()
        {
            GetWindow<GameDebuggerWindow>("TriNodo Debugger");
        }

        private void OnGUI()
        {
            GUILayout.Label("Controles del MVP", EditorStyles.boldLabel);

            if (Application.isPlaying && GameManager.Instance != null)
            {
                if (GUILayout.Button("Reiniciar Partida"))
                {
                    // Forzar reinicio llamando a Start
                    GameManager.Instance.Invoke("Start", 0);
                }

                EditorGUILayout.Space();
                GUILayout.Label("Estado Actual", EditorStyles.miniLabel);
                // Aquí podrías añadir más info si fuera necesario
            }
            else
            {
                GUILayout.Label("Entra en modo Play para ver controles.");
            }
        }
    }
}
