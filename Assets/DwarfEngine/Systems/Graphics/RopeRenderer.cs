using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Unity.Entities;

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

        private EntityManager entityManager;
        private Entity simulationEntity;

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
            float2 ropeStartPoint = new float2(startPoint.position.x, startPoint.position.y);

            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;

            #region Simulation System Initialization
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            simulationEntity = entityManager.CreateEntity();
            entityManager.AddBuffer<RopeSegmentBufferElement>(simulationEntity);
            entityManager.AddComponentData(simulationEntity, new RopeSimulationData
            {
                segmentCount = segmentCount,
                ropeSegLen = ropeSegLen,
                startPoint = new float2(startPoint.position.x, startPoint.position.y),
                refPoint = new float2(refPoint.position.x, refPoint.position.y),
                endPoint = new float2(endPoint.position.x, endPoint.position.y),
                gravityPoint = new float2(gravityPoint.position.x, gravityPoint.position.y),
                gravityStrength = gravityStrength
            });

            DynamicBuffer<RopeSegmentBufferElement> ropeSegments = entityManager.GetBuffer<RopeSegmentBufferElement>(simulationEntity);

            for (int i = 0; i < segmentCount; i++)
            {
                ropeSegments.Add(new RopeSegmentBufferElement { value = new RopeSegment(ropeStartPoint) });
                ropeStartPoint.y -= ropeSegLen;
            } 
            #endregion
        }

        // TODO : Try putting this inside an event callback in the RopeSimulationSystem
        private void Update()
        {
            var segments = entityManager.GetBuffer<RopeSegmentBufferElement>(simulationEntity);
            
            Vector3[] ropePositions = new Vector3[segmentCount];
            for (int i = 0; i < segmentCount; i++)
            {
                ropePositions[i] = new Vector3(segments[i].value.posNow.x, segments[i].value.posNow.y);
            }

            lineRenderer.positionCount = ropePositions.Length;
            lineRenderer.SetPositions(ropePositions);

            var data = entityManager.GetComponentData<RopeSimulationData>(simulationEntity);

            data.startPoint = startPoint.position.ToFloat2();
            data.refPoint = refPoint.position.ToFloat2();
            data.endPoint = endPoint.position.ToFloat2();
            data.gravityPoint = gravityPoint.position.ToFloat2();

            entityManager.SetComponentData(simulationEntity, data);
        }
    }

    
}
