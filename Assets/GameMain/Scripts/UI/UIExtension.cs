using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
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
    }
}