using System;
using UnityEngine;

namespace DwarfEngine
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        //public InputManager InputManager;

        private void Awake()
        {
            this.SetSingleton(ref Instance, true);

            //float cutout = (Screen.width - Screen.safeArea.width) / Screen.height;
            //Debug.Log(cutout);
            //if (cutout > 0)
            //{
            //    Camera.main.rect = new Rect(cutout, Camera.main.rect.y, Camera.main.rect.width, Camera.main.rect.height);  
            //}
        }
    }
}