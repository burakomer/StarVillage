using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DwarfEngine
{
    public class PlayerBrain : CharacterBrain
    {
        public PlayerControls playerControls;
        public PlayerState currentState;

        //[Header("Animator Parameters")]
        //public string 

        protected override void PreInit()
        {
            playerControls = new PlayerControls();
        }

        protected override void SetInputSources()
        {
            inputs.movement = this.UpdateAsObservable()
                .Select(_ => playerControls.Controls.Move.ReadValue<Vector2>());

            inputs.look = this.UpdateAsObservable()
                .Select(_ =>
                {
                    Vector2 lookInput = playerControls.Controls.Rotate.ReadValue<Vector2>();
                    if (lookInput != Vector2.zero)
                    {
                        return lookInput;
                    }
                    else
                    {
                        return playerControls.Controls.Move.ReadValue<Vector2>();
                    }
                });
                
                //.Select(_ => playerControls.Controls.Rotate.ReadValue<Vector2>());
            
            inputs.attack = this.UpdateAsObservable()
                .Select(_ => playerControls.Controls.Attack.ReadValue<float>() > 0)
                .ToReadOnlyReactiveProperty();
        }

        protected override void Init()
        {
            
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

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }

    public enum PlayerState { Running, Shooting, Jumping, Falling }
}