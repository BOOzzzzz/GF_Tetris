
using System.Collections;
using System.Collections.Generic;
using BOO;
using BOO.Procedure;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = BOO.GameEntry;

public class UIFormMenuLogic : UIFormLogicEx
{
    public Button btnStart;
    public Button btnOption;
    public Button btnQuit;

    private ProcedureMenu procedureMenu;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        
        procedureMenu = (ProcedureMenu)userData;
        btnStart.onClick.AddListener((() =>
        {
            GameEntry.Sound.PlayUISound((int)EnumSound.NewGame);
            procedureMenu.startGame = true;
        }));
        
        btnOption.onClick.AddListener((() =>
        {
            GameEntry.Sound.PlayUISound((int)EnumSound.NewGame);
            
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
