using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(LineRenderer))]
    public class BezierLineRenderer : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        public int positionCount = 200;

        public Transform startPoint;
        public Transform endPoint;
        public Transform curvePoint;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = positionCount;
        }

        private void Update()
        {
            DrawQuadraticBezierCurve(startPoint.position, curvePoint.position, endPoint.position);
        }

        private void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            float t = 0f;
            Vector3 B;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
                lineRenderer.SetPosition(i, B);
                t += (1 / (float)lineRenderer.positionCount);
            }
        }
    } 
}