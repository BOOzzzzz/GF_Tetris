using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public static class UIExtension
{
    public static void SetImageAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color imgColor = image.color;
            imgColor.a = alpha; // 设置目标透明度
            image.color = imgColor;
        }
        else
        {
            Debug.LogError("Target Image not assigned!");
        }
    }

    public static int OpenUIForm(this UIComponent uiComponent, string uiFormAssetName, string uiGroupName,
        bool coverPreviousUI,UIForm uiForm, object userData = null)
    {
        if (coverPreviousUI)
        {
            uiComponent.CloseUIForm(uiForm);
        }

        return uiComponent.OpenUIForm(uiFormAssetName, uiGroupName, userData);
    }
}