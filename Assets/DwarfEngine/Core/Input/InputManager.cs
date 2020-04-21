using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace DwarfEngine
{
	public enum InputMode { KeyboardMouse, Gamepad }

	public class InputManager : MonoBehaviour
	{
		public static InputManager Instance;

		public InputMode inputMode;

		private PlayerControls playerControls;

		private void Awake()
		{
			this.SetSingleton(ref Instance);

			playerControls = new PlayerControls();
		}

		public Vector2 GetMovement()
		{
			return playerControls.Controls.Move.ReadValue<Vector2>();
		}

		public Vector2 GetLook(bool inWorldPoint = true)
		{
			switch (inputMode)
			{
				case InputMode.KeyboardMouse:
					Vector2 mousePos = Mouse.current.position.ReadValue();
					if (inWorldPoint)
					{
						return Camera.main.ScreenToWorldPoint(mousePos).ToVector2();
					}
					else
					{
						return mousePos;
					}
				case InputMode.Gamepad:
					return playerControls.Controls.Rotate.ReadValue<Vector2>();
				default:
					return Vector2.zero;
			}
		}

		public float GetInteract()
		{
			return playerControls.Controls.Attack.ReadValue<float>();
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
}