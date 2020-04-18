using UnityEngine;

namespace DwarfEngine
{
    public class BowWeapon : ProjectileWeapon
    {
        [Header("Bow")]
        public float arrowDrawDistance;

        protected override void PrepareAttack()
        {
            base.PrepareAttack();
            SetupProjectile();

            if (charge != null)
                currentProjectile.LeanMoveLocalX(arrowDrawDistance + fireOffset.x, charge.chargeTime);
            else
                currentProjectile.transform.localPosition = new Vector2(arrowDrawDistance + fireOffset.x, 0);
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
