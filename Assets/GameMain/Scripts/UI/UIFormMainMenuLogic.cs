
using System.Collections;
using System.Collections.Generic;
using BOO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = BOO.GameEntry;

public class UIFormMainMenuLogic : UIFormLogic
{
    public Button btnStart;
    public Button btnOption;
    public Button btnQuit;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        
        
        btnStart.onClick.AddListener((() =>
        {
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormMain"), "Main");
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("GameMain"));
        }));
        
        btnOption.onClick.AddListener((() =>
        {
            
        }));
        
        btnQuit.onClick.AddListener((() =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }));
    }
}
