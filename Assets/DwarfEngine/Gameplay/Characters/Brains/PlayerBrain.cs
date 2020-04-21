using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DwarfEngine
{
    public class PlayerBrain : CharacterBrain
    {
        public Transform handContainer;

        protected override void SetInputSources()
        {
            inputs.movement = this.UpdateAsObservable()
                .Select(_ => InputManager.Instance.GetMovement());

            inputs.look = this.UpdateAsObservable()
                .Select(_ =>
                {
                    Vector2 lookVector = InputManager.Instance.GetLook();
                    Vector2 lookInput = InputManager.Instance.inputMode == InputMode.KeyboardMouse
                    ? Vector2.ClampMagnitude(lookVector - handContainer.position.ToVector2(), 1f)
                    : lookVector;

                    if (lookInput != Vector2.zero)
                    {
                        return lookInput;
                    }
                    else
                    {
                        return InputManager.Instance.GetMovement();
                    }
                });
            
            inputs.attack = this.UpdateAsObservable()
                .Select(_ => InputManager.Instance.GetInteract() > 0)
                .ToReadOnlyReactiveProperty();
        }
    }
}