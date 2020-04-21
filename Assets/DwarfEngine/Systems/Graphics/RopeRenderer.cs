using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(LineRenderer))]
    public class RopeRenderer : MonoBehaviour
    {
        public Transform refPoint;
        public Transform startPoint;
        public Transform endPoint;
        
        [Header("Rope Settings")]
        public float ropeSegLen = 0.25f;
        public int segmentCount = 35;
        public float lineWidth = 1f;

        [Header("Gravity")]
        public Transform gravityPoint;
        public float gravityStrength = 20f;

        private LineRenderer lineRenderer;
        private List<RopeSegment> ropeSegments = new List<RopeSegment>();

        private void Awake()
        {
            if (refPoint == null)
            {
                refPoint = transform;
            }
        }
        private void Start()
        {
            this.lineRenderer = this.GetComponent<LineRenderer>();
            Vector3 ropeStartPoint = startPoint.position;

            for (int i = 0; i < segmentCount; i++)
            {
                this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
                ropeStartPoint.y -= ropeSegLen;
            }
        }

        private void Update()
        {
            this.DrawRope();
        }

        private void FixedUpdate()
        {
            this.Simulate();
        }

        private void Simulate()
        {
            // SIMULATION
            Vector2 forceGravity = gravityPoint.position - refPoint.position;
            
            for (int i = 1; i < this.segmentCount; i++)
            {
                RopeSegment firstSegment = this.ropeSegments[i];
                Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
                firstSegment.posOld = firstSegment.posNow;
                firstSegment.posNow += velocity;
                firstSegment.posNow += (forceGravity * gravityStrength) * Time.fixedDeltaTime;
                this.ropeSegments[i] = firstSegment;
            }

            // CONSTRAINTS
            for (int i = 0; i < 50; i++)
            {
                this.ApplyConstraint();
            }
        }

        private void ApplyConstraint()
        {
            //Constrant to Start Point 
            RopeSegment firstSegment = this.ropeSegments[0];
            firstSegment.posNow = this.startPoint.position;
            this.ropeSegments[0] = firstSegment;

            //Constrant to End Point 
            if (endPoint != null)
            {
                RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
                endSegment.posNow = this.endPoint.position;
                this.ropeSegments[this.ropeSegments.Count - 1] = endSegment; 
            }

            for (int i = 0; i < this.segmentCount - 1; i++)
            {
                RopeSegment firstSeg = this.ropeSegments[i];
                RopeSegment secondSeg = this.ropeSegments[i + 1];

                float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
                float error = Mathf.Abs(dist - this.ropeSegLen);
                Vector2 changeDir = Vector2.zero;

                if (dist > ropeSegLen)
                {
                    changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
                }
                else if (dist < ropeSegLen)
                {
                    changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
                }

                Vector2 changeAmount = changeDir * error;
                if (i != 0)
                {
                    firstSeg.posNow -= changeAmount * 0.5f;
                    this.ropeSegments[i] = firstSeg;
                    secondSeg.posNow += changeAmount * 0.5f;
                    this.ropeSegments[i + 1] = secondSeg;
                }
                else
                {
                    secondSeg.posNow += changeAmount;
                    this.ropeSegments[i + 1] = secondSeg;
                }
            }
        }

        private void DrawRope()
        {
            float lineWidth = this.lineWidth;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            
            Vector3[] ropePositions = new Vector3[this.segmentCount];
            for (int i = 0; i < this.segmentCount; i++)
            {
                ropePositions[i] = this.ropeSegments[i].posNow;
            }

            lineRenderer.positionCount = ropePositions.Length;
            lineRenderer.SetPositions(ropePositions);
        }

        public struct RopeSegment
        {
            public Vector2 posNow;
            public Vector2 posOld;

            public RopeSegment(Vector2 pos)
            {
                this.posNow = pos;
                this.posOld = pos;
            }
        }
    }
}
