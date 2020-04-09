using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class SpriteIK : MonoBehaviour
    {
        public Vector3 anchorOffset;
        public Vector3 leanDirection;
        public float wiggleMultiplier;
        public float wiggleTimeOffset;
        public float tweenTime;
        public float startTimeOffset;

        private float radius;

        private SpriteRenderer[] sprites;
        private SpriteRenderer mainSprite;

        private void Start()
        {
            sprites = GetComponentsInChildren<SpriteRenderer>();
            mainSprite = sprites[0];

            foreach (var renderer in sprites)
            {
                renderer.gameObject.transform.SetParent(transform.parent);
            }
            radius = sprites[0].sprite.bounds.size.y; //radius of *black circle*

            StartCoroutine(UpDownTween(tweenTime * wiggleTimeOffset));
        }

        public void UpdatePosition(int newPosX)
        {
            transform.position += new Vector3Int(newPosX, 0, 0);
        }

        private void LateUpdate()
        {
            //anchorOffset = Vector3.ClampMagnitude(sprites[sprites.Length - 1].transform.localPosition - sprites[0].transform.localPosition, radius);
            for (int i = 1; i < sprites.Length; i++)
            {
                LeanTween.cancel(sprites[i].gameObject);
                Vector3 newLocation = sprites[i].transform.localPosition;
                float radius = sprites[i - 1].sprite.bounds.size.y; //radius of *black circle*
                Vector3 centerPosition = sprites[i - 1].transform.localPosition; //center of *black circle*
                float distance = Vector3.Distance(newLocation, centerPosition); //distance from ~green object~ to *black circle*
                                                                                //Debug.Log(distance);
                if (distance > radius) //If the distance is less than the radius, it is already within the circle.
                {

                    Vector3 fromOriginToObject = newLocation - centerPosition; //~GreenPosition~ - *BlackCenter*
                    fromOriginToObject *= radius / distance; //Multiply by radius //Divide by Distance
                    newLocation = centerPosition + fromOriginToObject / 1.01f; //*BlackCenter* + all that Math
                                                                               //LeanTween.moveLocal(sprites[i].gameObject, newLocation + anchorOffset, tweenTime).setEase(LeanTweenType.easeOutSine);
                    sprites[i].transform.localPosition = newLocation;

                }
                else
                {
                    //sprites[i].transform.localPosition = newLocation + anchorOffset + leanDirection;
                    LeanTween.moveLocal(sprites[i].gameObject, newLocation + Vector3.ClampMagnitude(anchorOffset + leanDirection, radius / 1.01f), tweenTime).setEase(LeanTweenType.easeOutQuad);
                }
            }

            for (int i = sprites.Length - 1; i >= 1; i--)
            {
                LeanTween.cancel(sprites[i].gameObject);
                Vector3 newLocation = sprites[i].transform.localPosition;
                float radius = sprites[i == sprites.Length - 1 ? i : i + 1].sprite.bounds.size.y; //radius of *black circle*
                Vector3 centerPosition = sprites[i == sprites.Length - 1 ? i : i + 1].transform.localPosition; //center of *black circle*
                float distance = Vector3.Distance(newLocation, centerPosition); //distance from ~green object~ to *black circle*
                                                                                //Debug.Log(distance);
                if (distance > radius) //If the distance is less than the radius, it is already within the circle.
                {

                    Vector3 fromOriginToObject = newLocation - centerPosition; //~GreenPosition~ - *BlackCenter*
                    fromOriginToObject *= radius / distance; //Multiply by radius //Divide by Distance
                    newLocation = centerPosition + fromOriginToObject / 1.01f; //*BlackCenter* + all that Math
                                                                               //LeanTween.moveLocal(sprites[i].gameObject, newLocation + anchorOffset, tweenTime).setEase(LeanTweenType.easeOutSine);
                    sprites[i].transform.localPosition = newLocation;

                }
                else
                {
                    //sprites[i].transform.localPosition = newLocation + anchorOffset + leanDirection;
                    LeanTween.moveLocal(sprites[i].gameObject, newLocation + Vector3.ClampMagnitude(anchorOffset + leanDirection, radius / 1.01f), tweenTime).setEase(LeanTweenType.easeOutQuad);
                }
            }
        }

        private IEnumerator UpDownTween(float delay)
        {
            yield return new WaitForSeconds(startTimeOffset);
            while (true)
            {
                LeanTween.cancel(sprites[0].gameObject);
                LeanTween.value(sprites[0].gameObject, anchorOffset, Vector3.up * wiggleMultiplier, delay)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnUpdate((Vector3 v3) =>
                    {
                        anchorOffset = v3;
                    });
                //LeanTween.move(sprites[0].gameObject, Vector2.up * 0.4f, delay).setEase(LeanTweenType.easeInOutSine);
                yield return new WaitForSeconds(delay);
                LeanTween.cancel(sprites[0].gameObject);
                LeanTween.value(sprites[0].gameObject, anchorOffset, Vector3.down * wiggleMultiplier, delay)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnUpdate((Vector3 v3) =>
                    {
                        anchorOffset = v3;
                    });
                //LeanTween.move(sprites[0].gameObject, Vector2.down * 0.4f, delay).setEase(LeanTweenType.easeInOutSine);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}