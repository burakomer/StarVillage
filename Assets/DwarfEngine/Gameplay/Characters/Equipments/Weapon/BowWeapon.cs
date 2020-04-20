using UnityEngine;

namespace DwarfEngine
{
    public class BowWeapon : ProjectileWeapon
    {
        private const float STRING_RELEASE_TWEEN_TIME = 0.5f;
        
        [Header("Bow")]
        public float arrowDrawDistance;
        public GameObject stringPullPoint;

        private float stringLoosePoint;
        private float stringDrawDistance;

        protected override void Init()
        {
            base.Init();

            stringLoosePoint = stringPullPoint.transform.localPosition.x;
            stringDrawDistance = (arrowDrawDistance + stringLoosePoint) * -10f;
        }

        protected override void PrepareAttack()
        {
            base.PrepareAttack();
            SetupProjectile();

            if (charge != null)
            {
                currentProjectile.LeanMoveLocalX(arrowDrawDistance + fireOffset.x, charge.chargeTime).setEaseOutSine();

                LeanTween.cancel(stringPullPoint.gameObject);
                stringPullPoint.LeanMoveLocalX(stringDrawDistance, charge.chargeTime).setEaseOutSine();
            }
            else
            {
                currentProjectile.transform.localPosition = new Vector2(arrowDrawDistance + fireOffset.x, 0);
                stringPullPoint.transform.localPosition = stringDrawDistance.ToVector3(1, 0, 0);
            }
        }

        protected override void Attack()
        {
            base.Attack();

            LeanTween.cancel(stringPullPoint.gameObject);

            stringPullPoint.LeanMoveLocalX(stringLoosePoint, STRING_RELEASE_TWEEN_TIME).setEaseOutElastic();
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
