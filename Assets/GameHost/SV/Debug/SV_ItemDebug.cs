using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_ItemDebug : HostComponent
    {
        static GameObject _debugObject;

        public override void Initialize()
        {
            _debugObject = Resources.Load<GameObject>("Debug/DebugObject");
        }

        public override void Shutdown()
        {
            _debugObject = null;
        }

        public override void Begin()
        {
            if (SV_Round.RoundNumber == 0) { return; }

            var root = new GameObject("Debug");
            root.transform.SetParent(GameHost.World.transform);

            foreach(var point in SV_Items.CornerPoints)
            {
                var pos = ShareSystem.Point2Position(point);
                var obj = GameObject.Instantiate(_debugObject, pos, Quaternion.identity);

                obj.transform.SetParent(root.transform);
            }
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}