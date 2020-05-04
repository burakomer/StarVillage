using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    [RequireComponent(typeof(Collider2D))]
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

        private Collider2D _collider;
        [SerializeField] private Transform idleSwingAnchor;
        [SerializeField] private Transform hitSwingAnchor;

        private IEnumerator Start()
        {
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;

            LeanTween.init(1000, 1000);

            if (idleSwing)
            {
                yield return new WaitForSeconds(Random.Range(0, 4f));
                IdleSwing(Random.value > 0.5);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            LeanTween.cancel(hitSwingAnchor.gameObject);
            hitSwingAnchor.localRotation = Quaternion.Euler(0, 0, 0);

            Vector3 contactPoint = collider.ClosestPoint(transform.position);
            Vector3 center = _collider.bounds.center;
            
            bool isRight = contactPoint.x > center.x;

            LeanTween.rotateLocal(hitSwingAnchor.gameObject, Mathf.Clamp(isRight ? swingStrength : -swingStrength, -clampAngle, clampAngle).ToVector3(0, 0, 1), swingTime).setEasePunch();
        }

        private void IdleSwing(bool startWithRight)
        {
            LeanTween.sequence()
            .append(LeanTween.rotateLocal(idleSwingAnchor.gameObject, (startWithRight ? clampAngle : -clampAngle).ToVector3(0, 0, 1), idleSwingTime).setEase(idleEaseType))
            .append(LeanTween.rotateLocal(idleSwingAnchor.gameObject, (startWithRight ? -clampAngle : clampAngle).ToVector3(0, 0, 1), idleSwingTime).setEase(idleEaseType))
            .append(() => {
                IdleSwing(startWithRight);
            });
        }
    }
}