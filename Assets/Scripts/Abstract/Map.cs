using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public abstract class Map : MonoBehaviour
    {
        public GameObject[] respawnPositions = new GameObject[1];
        public MapName MapName { get; protected set; }

        public int Index { get; private set; }

        public virtual void Initialize()
        {
            var tr = respawnPositions[0].transform;
            Player.SetPosition(tr.position, tr.eulerAngles);
        }

        public virtual void Shutdown() { }

        public void Next()
        {
            Index = (Index + 1) % respawnPositions.Length;

            var pos = respawnPositions[Index].transform.position;
            var rot = respawnPositions[Index].transform.eulerAngles;

            Player.SetPosition(pos, rot);

        }

        public void Prev()
        {
            Index -= 1;
            if (Index < 0) { Index = respawnPositions.Length - 1; }

            var pos = respawnPositions[Index].transform.position;
            var rot = respawnPositions[Index].transform.eulerAngles;

            Player.SetPosition(pos, rot);
        }

        public void Back(int index = -1)
        {
            if (0 <= index && index < respawnPositions.Length)
            {
                Index = index;
            }

            var pos = respawnPositions[Index].transform.position;
            var rot = respawnPositions[Index].transform.eulerAngles;

            Player.SetPosition(pos, rot);
        }

        public void SetIndex(int Index)
        {
            if (Index < 0)
            {
                Index = 0;
            }

            if (Index > respawnPositions.Length - 1)
            {
                Index = respawnPositions.Length - 1;
            }

            this.Index = Index;
        }
    }
}

