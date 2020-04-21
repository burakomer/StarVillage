using UnityEngine;

namespace DwarfEngine
{
    public class BowWeapon : ProjectileWeapon
    {
        private const float STRING_RELEASE_TWEEN_TIME = 0.5f;
        
        [Header("Bow")]
        public float arrowDrawDistance;
        public GameObject stringDrawPoint;
        public LeanTweenType drawEase;

        private float stringLoosePoint;
        private float stringDrawDistance;

        private RopeRenderer _rope;
        
        protected override void Init()
        {
            base.Init();

            _rope = GetComponentInChildren<RopeRenderer>();

            //stringLoosePoint = stringDrawPoint.transform.localPosition.x;
            stringDrawDistance = (arrowDrawDistance); //+ stringLoosePoint) * -10f;
        }

        protected override void PrepareAttack()
        {
            base.PrepareAttack();
            SetupProjectile();

            if (charge != null)
            {
                currentProjectile.LeanMoveLocalX(arrowDrawDistance + fireOffset.x, charge.chargeTime).setEase(drawEase);

                LeanTween.cancel(stringDrawPoint.gameObject);
                //stringDrawPoint.LeanMoveLocalX(stringDrawDistance, charge.chargeTime).setEase(drawEase);
                _rope.gravityPoint.LeanMoveLocalX(stringDrawDistance, charge.chargeTime).setEase(drawEase);

                //gameObject.LeanValue(value => 
                //{
                //    _rope.gravityX += value;
                //}, 
                //0f, stringDrawDistance, charge.chargeTime);
            }
            else
            {
                currentProjectile.transform.localPosition = new Vector2(arrowDrawDistance + fireOffset.x, 0);
                //stringDrawPoint.transform.localPosition = stringDrawDistance.ToVector3(1, 0, 0);
                _rope.gravityPoint.localPosition = stringDrawDistance.ToVector3(1, 0, 0);

                //_rope.gravityX = stringDrawDistance;
            }
        }

        protected override void Attack()
        {
            base.Attack();

            //LeanTween.cancel(stringDrawPoint.gameObject);
            //stringDrawPoint.LeanMoveLocalX(stringLoosePoint, STRING_RELEASE_TWEEN_TIME).setEaseOutElastic();

            LeanTween.cancel(_rope.gravityPoint.gameObject);
            _rope.gravityPoint.localPosition = Vector3.zero;
            //_rope.gravityX = 0f;

            LeanTween.cancel(weaponModel);
            weaponModel.transform.localScale = Vector3.one;
            weaponModel.LeanScale(new Vector3(1f, 1.35f, 1f), 0.5f).setEasePunch();
        }

        protected override void OnStopEquipment()
        {
            if (currentProjectile != null && charge != null)
            {
                LeanTween.cancel(currentProjectile);
            }
            base.OnStopEquipment();
        }

        protected override void OnStopWeapon()
        {
            if (currentProjectile != null)
            {
                pooler.SetParentToContainer(currentProjectile.transform);
                currentProjectile.SetActive(false);
                currentProjectile = null;
            }
            base.OnStopWeapon();
        }
    }
}
