
using System.Collections;
using System.Collections.Generic;
using BOO;
using BOO.Procedure;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = BOO.GameEntry;

public class UIFormGameOverLogic : UIFormLogicEx
{
    public Button btnTryAgain;
    public Button btnMenu;
    public Button btnQuit;

    private ProcedureGameOver procedureGameOver;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        
        procedureGameOver = userData as ProcedureGameOver;
        btnTryAgain.onClick.AddListener((() =>
        {
            procedureGameOver.RestartGame();
        }));
        
        btnMenu.onClick.AddListener((() =>
        {
            procedureGameOver.BackMenu();
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
