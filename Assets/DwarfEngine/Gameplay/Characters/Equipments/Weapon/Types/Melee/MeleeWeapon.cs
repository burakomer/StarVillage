using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class MeleeWeapon : Weapon
    {
        public const int MAX_OBJECTS_TO_HIT = 20;

        [Header("Melee Weapon")]
        //public float damageAreaRadius;
        public Vector2 damageAreaSize;
        public Vector2 damageAreaOffset;
        public float attackAnimationTime;

        private Transform _hitCircleOrigin;
        private SpriteModel _model;
        private Collider2D[] hitObjects;

        protected override void EquipLogic()
        {
            _model = weaponModel.GetComponent<SpriteModel>();

            _hitCircleOrigin = new GameObject().transform;
            _hitCircleOrigin.position += damageAreaOffset.ToVector3();
            _hitCircleOrigin.SetParent(weaponModel.transform);

            #region For Visualization
            var capsule = _hitCircleOrigin.gameObject.AddComponent<CapsuleCollider2D>();
            capsule.size = damageAreaSize;
            capsule.gameObject.layer = LayerMask.NameToLayer("NullLayer"); 
            #endregion

            hitObjects = new Collider2D[MAX_OBJECTS_TO_HIT];
        }

        protected override void UnequipLogic()
        {

        }

        protected override void PrepareAttack()
        {
            _model.PlayAnimation(weaponAttackAnim);
        }

        protected override void OnStopWeapon()
        {
            _model.PlayAnimation(weaponIdleAnim);
        }

        protected override IEnumerator Attack()
        {
            //Physics2D.OverlapCircleNonAlloc(_hitCircleOrigin.position, damageAreaRadius, hitObjects, hitMask);
            
            Physics2D.OverlapCapsuleNonAlloc(
                _hitCircleOrigin.position,
                damageAreaSize,
                CapsuleDirection2D.Vertical,
                aim.currentAngle,
                hitObjects,
                hitMask);

            foreach (Collider2D hitObject in hitObjects)
            {
                if (hitObject != null)
                {
                    hitObject.SendMessage("OnTriggerEnter2D", owner.collider);
                    Health hitObjHealth = hitObject.GetComponent<Health>();
                    if (hitObjHealth != null && hitObjHealth.isAlive.Value)
                    {
                        hitObjHealth.Damage(damage.IntValue, Health.INVINCIBILITY_DURATION, owner.gameObject);
                    } 
                }
            }

            // Clean the array
            for (int i = 0; i < hitObjects.Length; i++)
            {
                hitObjects[i] = null;
            }

            yield return new WaitForSeconds(attackAnimationTime);
        }

        private void OnDrawGizmos()
        {
            if (_hitCircleOrigin != null)
            {
                //Gizmos.DrawWireSphere(_hitCircleOrigin.position, damageAreaRadius);
            }
        }
    }
}
