using UnityEngine;
using UniRx;

namespace DwarfEngine
{
    public enum AimType { TwoDirectional, FourDirectional, EightDirectional, FreeAxis }

    [RequireComponent(typeof(Weapon))]
    [DisallowMultipleComponent]
    public class WeaponAim : WeaponComponent
    {
        public AimType aimType;

        public float currentAngle;
        
        public float gripAngleOffset;
        public float gripLength;

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

        public void Aim(Vector2 direction)
        {
            Vector3 gripPos = weapon.transform.parent.position;
            Vector3 difference = Vector2.zero;

            switch (aimType)
            {
                case AimType.TwoDirectional:
                    difference = direction.x > 0 ? Vector2.right : Vector2.left;
                    break;
                case AimType.FourDirectional:
                    if (direction.x > 0)
                    {
                        if (direction.y > 0)
                        {
                            if (direction.x > direction.y)
                            {
                                difference = Vector2.right;
                            }
                            else
                            {
                                difference = Vector2.up;
                            }
                        }
                        else
                        {
                            if (-direction.x < direction.y)
                            {
                                difference = Vector2.right;
                            }
                            else
                            {
                                difference = Vector2.down;
                            }
                        }
                    }
                    else
                    {
                        if (direction.y > 0)
                        {
                            if (direction.x < -direction.y)
                            {
                                difference = Vector2.left;
                            }
                            else
                            {
                                difference = Vector2.up;
                            }
                        }
                        else
                        {
                            if (direction.x < direction.y)
                            {
                                difference = Vector2.left;
                            }
                            else
                            {
                                difference = Vector2.down;
                            }
                        }
                    }
                    break;
                case AimType.EightDirectional:
                    if (direction.x > 0.5f && direction.x <= 1f)
                    {
                        if (direction.y > 0.5f && direction.y <= 1f)
                        {
                            difference = Vector3.ClampMagnitude(Vector2.right + Vector2.up, 1f);
                        }
                        else if (direction.y > 0 && direction.x <= 0.5f)
                        {
                            difference = Vector2.right; // FIX????
                        }
                        else if (direction.y < 0 && direction.y >= -0.5f)
                        {
                            difference = Vector2.right;
                        }
                        else if (direction.y < -0.5f && direction.y >= -1f)
                        {
                            difference = Vector3.ClampMagnitude(Vector2.right + Vector2.down, 1f);
                        }
                    }
                    else if (direction.x > 0 && direction.x <= 0.5f)
                    {
                        if (direction.y > 0.5f && direction.y <= 1f)
                        {
                            difference = Vector2.up;
                        }
                        else if (direction.y > 0 && direction.x <= 0.5f)
                        {
                            difference = Vector3.ClampMagnitude(Vector2.right + Vector2.up, 1f);
                        }
                        else if (direction.y < 0 && direction.y >= -0.5f)
                        {
                            difference = Vector3.ClampMagnitude(Vector2.right + Vector2.down, 1f);
                        }
                        else if (direction.y < -0.5f && direction.y >= -1f)
                        {
                            difference = Vector2.down;
                        }
                    } 
                    else if (direction.x < 0 && direction.x >= -0.5f)
                    {
                        if (direction.y > 0.5f && direction.y <= 1f)
                        {
                            difference = Vector2.up;
                        }
                        else if (direction.y > 0 && direction.x <= 0.5f)
                        {
                            difference = Vector3.ClampMagnitude(Vector2.left + Vector2.up, 1f);
                        }
                        else if (direction.y < 0 && direction.y >= -0.5f)
                        {
                            difference = Vector3.ClampMagnitude(Vector2.left + Vector2.down, 1f);
                        }
                        else if (direction.y < -0.5f && direction.y >= -1f)
                        {
                            difference = Vector2.down;
                        }
                    }
                    else if (direction.x < -0.5f && direction.x >= -1f)
                    {
                        if (direction.y > 0.5f && direction.y <= 1f)
                        {
                            difference = Vector3.ClampMagnitude(Vector2.left + Vector2.up, 1f);
                        }
                        else if (direction.y > 0 && direction.x <= 0.5f)
                        {
                            difference = Vector2.left;
                        }
                        else if (direction.y < 0 && direction.y >= -0.5f)
                        {
                            difference = Vector2.left;
                        }
                        else if (direction.y < -0.5f && direction.y >= -1f)
                        {
                            difference = Vector3.ClampMagnitude(Vector2.left + Vector2.down, 1f);
                        }
                    }
                    break;
                case AimType.FreeAxis:
                    difference = direction;
                    break;
            }

            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            bool shouldFlip = angle > 90 || angle < -90;

            weapon.weaponModel.transform.rotation = Quaternion.Euler(0f, 0f, angle + (shouldFlip ? -gripAngleOffset : gripAngleOffset));
            weapon.weaponModel.transform.position = gripPos + (gripLength * (difference != Vector3.zero ? difference : Vector3.right));

            currentAngle = weapon.weaponModel.transform.eulerAngles.z;

            if (shouldFlip)
            {
                weapon.weaponModel.transform.rotation = Quaternion.Euler(0f, 180f, 180f - weapon.weaponModel.transform.eulerAngles.z);
            }
        }
    }
}


/*
switch (aimType)
            {
                case AimType.TwoDirectional:
                    angle = ((angle > 90) || (angle< -90)) ? 180f : 0f;
                    break;
                case AimType.FourDirectional:
                    if (angle > 45 && angle <= 135)
                    {
                        angle = 90f;
                    }
                    else if ((angle > 135 && angle <= 180) || (angle <= -135 && angle > -180))
                    {
                        angle = 180f;
                    }
                    else if (angle > -135 && angle <= -45)
                    {
                        angle = -90f;
                    }
                    else if (angle > -45 && angle <= 45)
                    {
                        angle = 0f;
                    }
                    break;
                case AimType.EightDirectional:
                    if (angle > 27.5f && angle <= 72.5f)
                    {
                        angle = 45f;
                    }
                    else if (angle > 72.5f && angle <= 117.5f)
                    {
                        angle = 90f;
                    }
                    else if (angle > 117.5f && angle <= 162.5f)
                    {
                        angle = 135f;
                    }
                    else if ((angle > 162.5f && angle <= 180f) || (angle <= -162.5f && angle > -180f))
                    {
                        angle = 180f;
                    }
                    else if (angle > -162.5f && angle <= -117.5f)
                    {
                        angle = -135f;
                    }
                    else if (angle > -117.5f && angle <= -72.5f)
                    {
                        angle = -90f;
                    }
                    else if (angle > -72.5f && angle <= -27.5f)
                    {
                        angle = -45f;
                    }
                    else if (angle > -27.5f && angle <= 27.5f)
                    {
                        angle = 0f;
                    }
                    break;
                case AimType.FreeAxis:
                    break;
            }

            Vector3 difference;

            if (angle >= 0)
            {
                difference = angle != 0 ? new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) : Vector2.right;
            }
            else
            {
                difference = new Vector2(Mathf.Sin(360f - angle), Mathf.Cos(360f - angle));
            }
            */