using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Vegetation : MonoBehaviour
    {
        public bool idleSwing;
        [Range(0f, 90f)]
        public float clampAngle;
        public float idleSwingTime;
        public LeanTweenType idleEaseType;
        [Range(0f, 90f)]
        public float swingStrength;
        public float swingTime;
        
        private Transform idleSwingAnchor;
        private Transform hitSwingAnchor;
        private BoxCollider2D _collider;
        private LTSeq currentSeq;
        private LTSeq idleSeq;

        private IEnumerator Start()
        {
            idleSwingAnchor = transform.parent.parent;
            hitSwingAnchor = transform.parent;
            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = true;

            if (idleSwing)
            {
                yield return new WaitForSeconds(Random.Range(0, 4f));
                IdleSwing(Random.value > 0.5);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (currentSeq != null)
            {
                LeanTween.cancel(currentSeq.id);
                hitSwingAnchor.localRotation = Quaternion.Euler(0, 0, 0);
                //return;
            }

            Vector3 contactPoint = collider.ClosestPoint(transform.position);
            Vector3 center = _collider.bounds.center;
            
            bool isRight = contactPoint.x > center.x;
            
            currentSeq = LeanTween.sequence();
            currentSeq.append(LeanTween.rotateLocal(hitSwingAnchor.gameObject, Mathf.Clamp(isRight ? swingStrength : -swingStrength, -clampAngle, clampAngle).ToVector3(0, 0, 1), swingTime).setEasePunch())
                .append(() =>
                {
                    currentSeq = null;
                });
        }

        private void IdleSwing(bool startWithRight)
        {
            idleSeq = LeanTween.sequence()
            .append(LeanTween.rotateLocal(idleSwingAnchor.gameObject, (startWithRight ? clampAngle : -clampAngle).ToVector3(0, 0, 1), idleSwingTime).setEase(idleEaseType))
            .append(LeanTween.rotateLocal(idleSwingAnchor.gameObject, (startWithRight ? -clampAngle : clampAngle).ToVector3(0, 0, 1), idleSwingTime).setEase(idleEaseType))
            .append(() => {
                IdleSwing(startWithRight);
            });
        }
    }
}