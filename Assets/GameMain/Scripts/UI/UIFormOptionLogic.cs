using System.Collections;
using System.Collections.Generic;
using BOO;
using BOO.Procedure;
using GameFramework.Localization;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = BOO.GameEntry;

public class UIFormOptionLogic : UIFormLogicEx
{
    public Button btnBack;
    public Button btnConfirm;
    public Slider musicSlider;
    public Slider soundSlider;
    public Toggle chineseSimplifiedToggle;
    public Toggle chineseTraditionalToggle;
    public Toggle englishToggle;
    public GameObject languageTipsUI;

    private ProcedureMenu procedureMenu;
    private Language selectedLanguage;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        btnBack.onClick.AddListener(() =>
        {
            GameEntry.Sound.PlayUISound((int)EnumSound.NewGame);

            GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormMenu"), "Menu", true, UIForm ,null);
        });
        
        btnConfirm.onClick.AddListener(() =>
        {
            if (selectedLanguage == GameEntry.Localization.Language)
            {
                GameEntry.Sound.PlayUISound((int)EnumSound.NewGame);
                GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormMenu"), "Menu", true, UIForm ,null);
                return;
            }
            GameEntry.Setting.SetString(Constant.Setting.Language, selectedLanguage.ToString());
            GameEntry.Setting.Save();
            //关闭框架 ShutdownType为重启
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        });
        
        musicSlider.onValueChanged.AddListener((arg0 =>
        {
            musicSlider.value = arg0;
            GameEntry.Sound.SetVolume(arg0 , "Music");
            GameEntry.Setting.SetFloat(Constant.Setting.MusicVolume , arg0);
            GameEntry.Setting.Save();
        }));
        
        soundSlider.onValueChanged.AddListener((arg0 =>
        {
            soundSlider.value = arg0;
            GameEntry.Sound.SetVolume(arg0 , "Sound");
            GameEntry.Sound.SetVolume(arg0 , "UISound");
            GameEntry.Setting.SetFloat(Constant.Setting.SoundVolume , arg0);
            GameEntry.Setting.Save();
        }));
        
        chineseTraditionalToggle.onValueChanged.AddListener((isOn =>
        {
            if (!isOn) return;
            selectedLanguage = Language.ChineseTraditional;
            RefreshLanguageTips();
        }));
        
        chineseSimplifiedToggle.onValueChanged.AddListener((isOn =>
        {
            if (!isOn) return;
            selectedLanguage = Language.ChineseSimplified;
            RefreshLanguageTips();
        }));
        
        englishToggle.onValueChanged.AddListener((isOn =>
        {
            if (!isOn) return;
            selectedLanguage = Language.English;
            RefreshLanguageTips();
        }));
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        selectedLanguage = GameEntry.Localization.Language;

        switch (selectedLanguage)
        {
            case Language.ChineseSimplified:
                chineseSimplifiedToggle.isOn = true;
                break;
            case Language.ChineseTraditional:
                chineseTraditionalToggle.isOn = true;
                break;
            case Language.English:
                englishToggle.isOn = true;
                break;
        }
        musicSlider.value = GameEntry.Sound.GetVolume("Music");
        soundSlider.value = GameEntry.Sound.GetVolume("Sound");
    }

    private void RefreshLanguageTips()
    {
        languageTipsUI.SetActive(selectedLanguage != GameEntry.Localization.Language);
    }
}