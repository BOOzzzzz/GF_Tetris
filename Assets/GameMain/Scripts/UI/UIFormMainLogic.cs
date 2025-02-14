﻿using System;
using System.Collections;
using BOO;
using BOO.Procedure;
using GameFramework.Event;
using GameMain.Scripts.Event;
using GameMain.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFormMainLogic : UIFormLogicEx
{
    public TMP_Text  tmp;
    public Image fadeBackground;
    public Button btnPause;
    
    private float fadeDuration = 1f;
    private ProcedureMain procedureMain;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        procedureMain   = (ProcedureMain)userData;
        btnPause.onClick.AddListener((() =>
        {
            procedureMain.OnPause();
        }));
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        
        GameEntry.Event.Subscribe(UpdateScoreEventArgs.EventId, UpdateScoreUI);
        UIExtension.SetImageAlpha(fadeBackground,1);
        StartCoroutine(FadeBackgroundCoroutine(fadeBackground));
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown, userData);
        
        GameEntry.Event.Unsubscribe(UpdateScoreEventArgs.EventId, UpdateScoreUI);
    }

    private void UpdateScoreUI(object sender, GameEventArgs e)
    {
        var updateScoreEventArgs = e as UpdateScoreEventArgs;
        tmp.text = updateScoreEventArgs.Score.ToString();
    }

    private IEnumerator FadeBackgroundCoroutine(Image image)
    {
        Color imgColor = image.color;
        float alpha = imgColor.a;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeDuration;
            imgColor.a = Mathf.Clamp(alpha, 0f, 1f);
            image.color = imgColor;
            yield return null;
        }
    }
}