using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DwarfEngine
{
	[RequireComponent(typeof(Image))]
	public class CustomCursor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public Sprite cursorSprite;
		public float fixedCursorDistance = 32;

		private Image _image;

		private void Start()
		{
			_image = GetComponent<Image>();
			_image.sprite = cursorSprite;

			this.UpdateAsObservable()
				.Select(_ => InputManager.Instance.GetLook(false))
				.Subscribe(v =>
				{
					if (v != Vector2.zero)
					{
						_image.color = Color.white;
						if (InputManager.Instance.inputMode == InputMode.KeyboardMouse)
						{
							((RectTransform)transform).anchoredPosition = v / UIManager.Instance.canvas.scaleFactor;
						}
						else
						{
							((RectTransform)transform).anchoredPosition = 
							Camera.main.WorldToScreenPoint((v * fixedCursorDistance) 
							+ LevelManager
							.Instance
							.player
							.equipmentManager
							.GetSlot<CharacterWeaponEquipmentSlot, Weapon>(typeof(Weapon))
							.handContainer
							.transform
							.position
							.ToVector2())
								/ UIManager.Instance.canvas.scaleFactor;
						}
					}
					else
					{
						_image.color = Color.clear;
					}
				})
				.AddTo(this);

			Cursor.visible = false;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			LeanTween.cancel(gameObject);
			gameObject.LeanScale(Vector3.one * 0.8f, 0.4f).setEaseOutQuart();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			LeanTween.cancel(gameObject);
			gameObject.LeanScale(Vector3.one, 0.4f).setEaseOutElastic();
		}
	}
}