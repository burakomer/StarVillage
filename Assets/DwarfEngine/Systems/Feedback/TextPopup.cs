using TMPro;
using UnityEngine;

namespace DwarfEngine
{
    /// <summary>
    /// A configurable popup text to show damage, heal, condition etc. for a small duration of time.
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class TextPopup : MonoBehaviour
    {
        [Header("Properties")]
        public float disappearTimerMax = 0.5f;
        public FontStyles specialStyle;

        private TextMeshPro _textMesh;

        private float oFontSize;
        private FontStyles oFontStyle;
        private Color oFontColor;

        private (string _text, Color _color, float _size, bool isBold) textProperties;

        private float disappearTimer;
        private Vector3 moveVector;

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshPro>();
            oFontSize = _textMesh.fontSize;
            oFontStyle = _textMesh.fontStyle;
            oFontColor = _textMesh.color;
        }

        private void OnEnable()
        {
            _textMesh.SetText(textProperties._text);
            _textMesh.fontSize = textProperties._size;
            _textMesh.color = textProperties._color;
        }

        private void OnDisable()
        {
            transform.localScale = Vector3.one;
        }

        private void ResetValues()
        {
            textProperties._size = oFontSize;
            textProperties._color = oFontColor;
            _textMesh.fontStyle = oFontStyle;
        }

        public void Setup(string text, Vector3 position, bool isBold = false, float sizeMultiplier = 1)
        {
            ResetValues();

            textProperties._text = text;
            transform.position = position;
            if (isBold) _textMesh.fontStyle = specialStyle;
            textProperties._size = oFontSize * sizeMultiplier;

            disappearTimer = disappearTimerMax;
            moveVector = new Vector3(0, 1f) * 10f;
        }

        public void Setup(string text, Vector3 position, Color color, bool isBold = false, float sizeMultiplier = 1)
        {
            Setup(text, position, isBold, sizeMultiplier);
            textProperties._color = color;
        }

        private void Update()
        {
            transform.localPosition += moveVector * Time.deltaTime;
            moveVector -= moveVector * 8f * Time.deltaTime;

            if (disappearTimer > disappearTimerMax * .8f)
            {
                transform.localScale += Vector3.one * 2f * Time.deltaTime;
            }
            else if (disappearTimer < disappearTimerMax * .2f)
            {
                transform.localScale -= Vector3.one * 1f * Time.deltaTime;
            }

            disappearTimer -= Time.deltaTime;

            if (disappearTimer < 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}