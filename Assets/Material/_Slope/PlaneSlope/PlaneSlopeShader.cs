using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PlaneSlopeShader : MonoBehaviour
    {
        void Start()
        {
            var mat = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;

            // size 
            var size = gameObject.transform.lossyScale;
            mat.SetFloat("_SizeX", size.x);
            mat.SetFloat("_SizeZ", size.z);

            // rotation
            var angle = gameObject.transform.eulerAngles;
            var rotZ = -angle.z * Mathf.Deg2Rad;
            var rotX = -angle.x * Mathf.Deg2Rad;

            mat.SetFloat("_CosX", Mathf.Cos(rotX));
            mat.SetFloat("_SinX", Mathf.Sin(rotX));

            mat.SetFloat("_CosZ", Mathf.Cos(rotZ));
            mat.SetFloat("_SinZ", Mathf.Sin(rotZ));

            // position
            var pos = gameObject.transform.position;
            mat.SetFloat("_X", pos.x);
            mat.SetFloat("_Y", pos.y);
            mat.SetFloat("_Z", pos.z);
        }
    }
}

