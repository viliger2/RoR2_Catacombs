using UnityEngine;
using UnityEditor;
using System.Collections;

public class Ruler : EditorWindow
{
    [MenuItem("Tools/Ruler")]
    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof(Ruler));
        window.position = new Rect(0, 0, 250, 80);
        window.Show();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    Transform objA;
    Transform objB;
    float distance;

    void OnGUI()
    {
        objA = (Transform)EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "start object", objA, typeof(Transform), true);
        objB = (Transform)EditorGUI.ObjectField(new Rect(3, 23, position.width - 6, 20), "end object", objB, typeof(Transform), true);

        if (objA != null && objB != null)
        {
            distance = Vector3.Distance(objA.position, objB.position);
            EditorGUI.LabelField(new Rect(3, 43, position.width - 6, 20), string.Format("distance between objects = {0}", distance));
        }
    }
}