using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectorVisual : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;


    public void UpdatePoints (List<Vector3> positions)
    {
        lineRenderer.positionCount = positions.Count;

        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
    }
}
