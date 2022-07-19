using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class CheckPointShader : MonoBehaviour
    {
        void Start()
        {
            var floorHeight = gameObject.transform.position.y - gameObject.transform.lossyScale.y * 0.5f;

            var mat = gameObject.GetComponent<MeshRenderer>().material;
            mat.SetFloat("_Y", floorHeight);
            mat.SetFloat("_DebugMode", -1.0f);
        }

        void Update()
        {

        }
    }
}

