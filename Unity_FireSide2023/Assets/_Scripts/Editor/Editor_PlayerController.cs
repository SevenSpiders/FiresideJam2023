using UnityEditor; 
using UnityEngine; 


[CustomEditor(typeof(PlayerController))]
public class Editor_PlayerController : Editor
{
    public override void OnInspectorGUI() {

        DrawDefaultInspector();

        PlayerController playerController = (PlayerController) target;
        if (GUILayout.Button("Debug")) {

            playerController.Test();
        }
    }
}


