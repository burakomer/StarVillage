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

        private float stringDrawDistance;
        private Color startingColor;

        private RopeRenderer _rope;
        
        protected override void EquipLogic()
        {
            base.EquipLogic();

            _rope = GetComponentInChildren<RopeRenderer>();

            //stringLoosePoint = stringDrawPoint.transform.localPosition.x;
            stringDrawDistance = (arrowDrawDistance); //+ stringLoosePoint) * -10f;
            startingColor = _renderer.material.GetColor("_Color");
        }

        protected override void PrepareAttack()
        {
            base.PrepareAttack();
            SetupProjectile();

            if (charge != null)
            {
                currentProjectile.gameObject.LeanMoveLocalX(arrowDrawDistance + fireOffset.x, charge.chargeTime.FloatValue).setEase(drawEase);

                LeanTween.cancel(stringDrawPoint.gameObject);
                //stringDrawPoint.LeanMoveLocalX(stringDrawDistance, charge.chargeTime).setEase(drawEase);
                _rope.gravityPoint.LeanMoveLocalX(stringDrawDistance, charge.chargeTime.FloatValue).setEase(drawEase);

                
                gameObject.LeanValue((value) => 
                {
                    _renderer.material.SetColor("_Color", value);
                },
                startingColor, startingColor * 2f, charge.chargeTime.FloatValue);
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

            LeanTween.cancel(_rope.gravityPoint.gameObject);
            _rope.gravityPoint.localPosition = Vector3.zero;
            
            LeanTween.cancel(weaponModel);
            weaponModel.transform.localScale = Vector3.one;
            weaponModel.LeanScale(new Vector3(1f, 1.35f, 1f), STRING_RELEASE_TWEEN_TIME).setEasePunch();

            LeanTween.cancel(gameObject);
            _renderer.material.SetColor("_Color", startingColor);
        }

        protected override void OnStopEquipment()
        {
            if (currentProjectile != null && charge != null)
            {
                LeanTween.cancel(currentProjectile.gameObject);
            }
            base.OnStopEquipment();
        }

        protected override void OnStopWeapon()
        {
            if (currentProjectile != null)
            {
                pooler.SetParentToContainer(currentProjectile.transform);
                currentProjectile.gameObject.SetActive(false);
                currentProjectile = null;
            }
            base.OnStopWeapon();
        }
    }
}
