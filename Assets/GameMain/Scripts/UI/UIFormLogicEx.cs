using UnityEngine;
using UnityGameFramework.Runtime;


    public class UIFormLogicEx:UIFormLogic
    {
        private Canvas canvas;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            canvas = GetComponent<Canvas>();
            canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
