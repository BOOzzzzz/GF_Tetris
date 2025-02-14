
using System.Collections;
using System.Collections.Generic;
using BOO;
using BOO.Procedure;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = BOO.GameEntry;

public class UIFormPauseLogic : UIFormLogicEx
{
    public Button btnResume;
    public Button btnMenu;
    public Button btnQuit;

    private ProcedureMain procedureMain;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        
        procedureMain = userData as ProcedureMain;
        btnResume.onClick.AddListener((() =>
        {
            GameEntry.Base.GameSpeed = 1;
            GameEntry.UI.CloseUIForm(UIForm);
        }));
        
        btnMenu.onClick.AddListener((() =>
        {
            procedureMain.BackMenu();
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
