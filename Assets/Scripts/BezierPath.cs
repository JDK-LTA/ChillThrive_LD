using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPath : MonoBehaviour
{
    public Transform p0, p1, p2;
    public int numOfPoints = 50;
    [HideInInspector] public Vector3[] positions;
    public LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        FullUpdateCurve();
    }

    public void ChangeColor(bool isDay)
    {
        if (isDay)
        {
            line.startColor = new Color32(255, 255, 255, 255);
            line.endColor = new Color32(255, 255, 255, 255);
        }
        else
        {
            line.startColor = new Color32(135, 104, 164, 255);
            line.endColor = new Color32(135, 104, 164, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurve();
    }

    private void UpdateCurve()
    {
        for (int i = 0; i < numOfPoints; i++)
        {
            float t = i / ((float)numOfPoints - 1);
            positions[i] = CalculateQuadBezierPoint(t, p0.position, p1.position, p2.position);
        }
        line.SetPositions(positions);
    }

    private void OnDrawGizmos()
    {
        FullUpdateCurve();
    }

    private void FullUpdateCurve()
    {
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.positionCount = numOfPoints;
        line.enabled = true;
        positions = new Vector3[numOfPoints];
        UpdateCurve();
    }

    private Vector3 CalculateQuadBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // B(t) = (1-t)^2P0 + 2(1-t)tP1 + t^2P2

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);

        return p;
    }
}
