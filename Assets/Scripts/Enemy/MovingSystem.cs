using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSystem : MonoBehaviour
{
    List<Vector3> path = new List<Vector3>();

    // Update is called once per frame
    public void MoveOn(float dt, float speed)
    {
        if (path == null || path.Count == 0) { return; }

        var currentPosition = gameObject.transform.position;
        var nextPosition = currentPosition;

        var movingDistance = dt * speed;
        var movingDistanceRemain = movingDistance;

        var removingPathIndexes = new List<int>();

        for (int n = 0; n < path.Count; n++)
        {
            float disp2Next = (path[n] - currentPosition).magnitude;

            if (movingDistanceRemain < disp2Next)
            {
                nextPosition = gameObject.transform.position + (path[n] - currentPosition).normalized * movingDistanceRemain;
                break;
            }

            else
            {
                movingDistanceRemain -= disp2Next;
                removingPathIndexes.Add(n);
                currentPosition = path[n];
                nextPosition = path[n];
            }
        }

        if (removingPathIndexes.Count > 0)
        {
            if (path.Count == 1)
            {
                path.Clear();
            }

            else
            {
                for (int n = removingPathIndexes.Count - 1; n > -1; n--)
                {
                    path.RemoveAt(removingPathIndexes[n]);
                }
            }

            removingPathIndexes.Clear();
        }

        Face2MovingDirection(currentPosition, nextPosition);

        gameObject.transform.position = nextPosition;
    }

    void Face2MovingDirection(Vector3 originalPosition, Vector3 nextPosition)
    {
        var direction = (nextPosition - originalPosition).normalized;

        var theta = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        gameObject.transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);
    }

    public void SetPath(List<Vector3> path)
    {
        this.path = path;
    }

    public List<Vector3> GetPath()
    {
        return new List<Vector3>(path);
    }

    public int PathLength()
    {
        return path.Count;
    }
}
