using GameFramework.Fsm;
using GameFramework.Procedure;

namespace BOO.Procedure
{
    public class ProcedureGameOver:ProcedureBase
    {

        private bool restartGame = false; 
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormGameOver"), "GameOver",userData:this);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (restartGame)
            {
                restartGame = false;
                string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
                for (int i = 0; i < loadedSceneAssetNames.Length; i++)
                {
                    GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
                }
                
                // 隐藏所有实体
                GameEntry.Entity.HideAllLoadingEntities();
                GameEntry.Entity.HideAllLoadedEntities();
                
                GameEntry.UI.CloseAllLoadedUIForms();
                
                GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormMain"), "Main");
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("GameMain"));
                ChangeState<ProcedureMain>(procedureOwner);
            }
        }

        public void RestartGame()
        {
            restartGame = true;
        }
    }
}