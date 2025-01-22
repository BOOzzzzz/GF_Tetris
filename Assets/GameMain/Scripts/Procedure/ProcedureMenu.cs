using GameFramework.Fsm;
using GameFramework.Procedure;

namespace BOO.Procedure
{
    public class ProcedureMenu:ProcedureBase
    {
        public bool startGame = false;
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormMenu"), "Menu",userData:this);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (startGame)
            {
                startGame = false;
                string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
                for (int i = 0; i < loadedSceneAssetNames.Length; i++)
                {
                    GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
                }
                GameEntry.UI.CloseAllLoadedUIForms();
                GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormMain"), "Main");
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("GameMain"));
                ChangeState<ProcedureMain>(procedureOwner);
            }
        }
    }
}