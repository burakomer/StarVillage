using UnityEngine;
using UniRx;

namespace DwarfEngine
{
    public enum AimType { Inventory, Character }

    [RequireComponent(typeof(Weapon))]
    [DisallowMultipleComponent]
    public class WeaponAim : WeaponComponent
    {
        public Vector2 currentDirection { get; protected set; }
        
        //public SpriteRenderer directionIndicator;
        public bool rotateModel;
        public float gripAngleOffset;
        public float gripLength;

        protected float angle;

        //private Transform leftHand;
        //private Transform rightHand;

        protected override void Init()
        {
            base.Init();

            weapon.owner.brain.inputs.look
                        .Where(v => v != Vector2.zero)
                        .Subscribe(v =>
                        {
                            Aim(v);
                        })
                        .AddTo(this);
        }

        /// <summary>
        /// Handle aim logic here.
        /// </summary>
        /// <param name="direction"></param>
        public void Aim(Vector2 direction)
        {
            angle = Vector2.SignedAngle(Vector2.right, direction);
            currentDirection = direction.normalized;

            //if (directionIndicator != null)
            //{
            //    directionIndicator.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            //}

            if (rotateModel)
            {
                //if (angle >= 90 || angle <= -90)
                //{
                //    weapon.weaponModel.transform.rotation = Quaternion.Euler(0f, 180f, 180f - angle);
                //}
                //else
                //{
                //    weapon.weaponModel.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                //}
                //fireFeedback.transform.parent.rotation = Quaternion.Euler(0f, 0f, angle);

                Vector3 gripPos = weapon.transform.parent.position;
                Vector3 difference = direction;
                
                float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                weapon.weaponModel.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + gripAngleOffset);
                weapon.weaponModel.transform.position = gripPos + (gripLength * difference.normalized);

                if ((difference.x <= 0) != (transform.localScale.y <= 0))
                {
                    weapon.weaponModel.transform.rotation = Quaternion.Euler(0f, 180f, 180f - weapon.weaponModel.transform.eulerAngles.z);
                }
            }
        }
    }
}
