using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class SurfEdgeLineShader : MonoBehaviour
    {
        private void Start()
        {
            var mat = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;

            // position
            var p = gameObject.transform.position;
            mat.SetFloat("_X", p.x);
            mat.SetFloat("_Z", p.z);

            // rotation
            var r = gameObject.transform.eulerAngles;
            mat.SetFloat("_CosY", Mathf.Cos(-r.y * Mathf.Deg2Rad));
            mat.SetFloat("_SinY", Mathf.Sin(-r.y * Mathf.Deg2Rad));

            // size 
            var s = gameObject.transform.lossyScale * 0.5f;
            mat.SetFloat("_HalfX", s.x);
            mat.SetFloat("_HalfZ", s.z);
        }
    }
}

