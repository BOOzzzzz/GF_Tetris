using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = BOO.GameEntry;


public class UIFormLogicEx:UIFormLogic
    {
        private Canvas canvas;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);
            for (int i = 0; i < texts.Length; i++)
            {
                if (!string.IsNullOrEmpty(texts[i].text))
                {
                    texts[i].text = GameEntry.Localization.GetString(texts[i].text);
                }
            }
            
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }
