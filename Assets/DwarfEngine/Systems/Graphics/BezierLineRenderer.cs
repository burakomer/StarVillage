using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(LineRenderer))]
    public class BezierLineRenderer : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        public float k = 1;

        public int positionCount = 200;

        public Transform startPoint;
        public Transform endPoint;
        public Transform curvePoint;

        private Vector3 realEndPoint;

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = positionCount;

            k = 1 + (1 / (float)(positionCount - 1));
        }

        private void Update()
        {
            //realEndPoint = endPoint.position;
            DrawQuadraticBezierCurve(startPoint.position, curvePoint.position, endPoint.position);
        }

        private void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            float t = 0f;
            Vector3 B;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                //if (i == lineRenderer.positionCount - 1)
                //{
                //    B = realEndPoint;
                //}
                //else
                //{
                    B = ((1 - (k * t)) * (1 - (k * t)) * point0) + (2 * (1 - (k * t)) * (k * t) * point1) + ((k * t) * (k * t) * point2);
                //}
                lineRenderer.SetPosition(i, B);
                t += (1 / (float)lineRenderer.positionCount);
            }
        }
    } 
}