using UnityEngine;

namespace DwarfEngine
{
    public class BowWeapon : ProjectileWeapon
    {
        [Header("Bow")]
        public float arrowDrawDistance;

        protected override void PreWindUp()
        {
            base.PreWindUp();
            SetupProjectile();
            currentProjectile.LeanMoveLocalX(arrowDrawDistance + fireOffset.x, windUpTime);
        }

        protected override void WindUpLogic(float windUpNormalized)
        {
            //currentProjectile.LeanMoveLocalX(arrowDrawDistance, windUpTime);
        }

        protected override void OnStopEquipment()
        {
            if (currentProjectile != null)
            {
                LeanTween.cancel(currentProjectile);
            }
            base.OnStopEquipment();
        }
    }
}
