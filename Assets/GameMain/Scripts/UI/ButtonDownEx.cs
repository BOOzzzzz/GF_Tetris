using System;
using System.Collections;
using System.Collections.Generic;
using BOO;
using BOO.Procedure;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDownEx : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    private ProcedureMain procedureMain;
    private void Awake()
    {
        procedureMain = GameEntry.Procedure.GetProcedure<ProcedureMain>() as ProcedureMain;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerInputManager.Instance.isDown = true;
        procedureMain.OnDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerInputManager.Instance.isDown = false;
        procedureMain.OnStopDown();
    }
}
