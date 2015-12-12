using UnityEngine;
using System.Collections;
using UnityEditor;

public class ShowSize : EditorWindow
{

    [MenuItem("Window/ShowSize")]

    static void Init () {
        // Get existing open window or if none, make a new one:
        ShowSize sizeWindow = new ShowSize();
        sizeWindow.autoRepaintOnSceneChange = true;
        sizeWindow.Show();
    }

    void OnGUI()
    {
        GameObject thisObject = Selection.activeObject as GameObject;
        if (thisObject == null)
        {
            return;
        }

        Renderer rend = thisObject.GetComponent<Renderer>();
        if (rend == null)
        { return; }

        Vector3 size = rend.bounds.size;
        Vector3 scale = thisObject.transform.localScale;
        GUILayout.Label("Size\nX: " + size.x * scale.x + "   Y: " + size.y * scale.y + "   Z: " + size.z * scale.z);
    }

    void OnSelectionChange()
    {
        if (Selection.activeGameObject != null)
        {
            Repaint();
        }
    }
}
