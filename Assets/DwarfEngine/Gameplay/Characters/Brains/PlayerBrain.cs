using UnityEngine;

namespace DwarfEngine
{
    public class PlayerBrain : CharacterBrain
    {
        public PlayerState currentState;

        //private CharacterMovement _movement;
        //private CharacterWeaponUser _weaponUser;

        protected override void Init()
        {
            //_movement = GetComponent<CharacterMovement>();
            //_weaponUser = GetComponent<CharacterWeaponUser>();

            currentState = PlayerState.Running;
        }

        protected override void SetInputManager()
        {
            //inputManager = GameManager.Instance.InputManager;
        }

        protected override void UpdateBrain()
        {
            // From any state
            // ...

            // From a specific state
            //switch (currentState)
            //{
            //    case PlayerState.Running:
            //        if (_movement.actualVelocity.y < -0.1f)
            //        {
            //            SetState(PlayerState.Falling);
            //        }
            //        else if (_movement.actualVelocity.y > 0.1f)
            //        {
            //            SetState(PlayerState.Jumping);
            //        }
            //        break;
            //    case PlayerState.Shooting:
            //
            //        break;
            //    case PlayerState.Jumping:
            //        if (_movement.actualVelocity.y < -0.1f)
            //        {
            //            SetState(PlayerState.Falling);
            //        }
            //        break;
            //    case PlayerState.Falling:
            //        if (_movement.actualVelocity.y > 0.1f)
            //        {
            //            SetState(PlayerState.Jumping);
            //        }
            //        if (_movement._controller2d.collisions.below)
            //        {
            //            SetState(PlayerState.Running);
            //        }
            //        break;
            //}
        }

        public void SetState(PlayerState newState)
        {
            // From any state
            if (newState == PlayerState.Shooting)
            {
                currentState = newState;
            }
            // From specific states
            else if (currentState == PlayerState.Running)
            {
                if (newState == PlayerState.Falling || newState == PlayerState.Jumping)
                {
                    currentState = newState;
                }
            }
            else if (currentState == PlayerState.Shooting)
            {
                if (newState == PlayerState.Falling || newState == PlayerState.Running)
                {
                    currentState = newState;
                }
            }
            else if (currentState == PlayerState.Jumping)
            {
                if (newState == PlayerState.Falling)
                {
                    currentState = newState;
                }
            }
            else if (currentState == PlayerState.Falling)
            {
                if (newState == PlayerState.Running || newState == PlayerState.Jumping)
                {
                    currentState = newState;
                }
            }

            ProcessState();
        }

        protected void ProcessState()
        {
            switch (currentState)
            {
                case PlayerState.Running:
                    character.model.PlayAnimation("Running");
                    break;
                case PlayerState.Shooting:
                    character.model.PlayAnimation("Shooting");
                    break;
                case PlayerState.Jumping:
                    character.model.PlayAnimation("Jumping");
                    break;
                case PlayerState.Falling:
                    character.model.PlayAnimation("Falling");
                    break;
                default:
                    break;
            }
        }
    }

    public enum PlayerState { Running, Shooting, Jumping, Falling }
}