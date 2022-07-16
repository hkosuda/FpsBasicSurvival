using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyGame
{
    static public class DevUtil
    {
#if UNITY_EDITOR
        static public float CameraDisp(GameObject gameObject)
        {
            return Vector3.Distance(gameObject.transform.position, SceneView.currentDrawingSceneView.camera.transform.position);
        }

        static public Vector3 CameraDirection(GameObject gameObject)
        {
            return (SceneView.currentDrawingSceneView.camera.transform.position - gameObject.transform.position).normalized;
        }
#endif
    }
}

