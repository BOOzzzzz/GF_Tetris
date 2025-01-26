using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace BOO.Procedure
{
    public class ProcedureGameOver:ProcedureBase
    {

        private bool restartGame = false; 
        private bool unloadedScene = false; 
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormGameOver"), "GameOver",userData:this);
            GameEntry.Event.Subscribe(UnloadSceneSuccessEventArgs.EventId,UnloadSceneSuccess);
            GameEntry.Event.Subscribe(UnloadSceneFailureEventArgs.EventId,UnloadSceneFailure);
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
            }

            if (unloadedScene)
            {
                unloadedScene = false;
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("GameMain"));
                ChangeState<ProcedureMain>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            
            
            GameEntry.Event.Unsubscribe(UnloadSceneSuccessEventArgs.EventId,UnloadSceneSuccess);
            GameEntry.Event.Unsubscribe(UnloadSceneFailureEventArgs.EventId,UnloadSceneFailure);
        }

        private void UnloadSceneFailure(object sender, GameEventArgs e)
        {
            
        }

        private void UnloadSceneSuccess(object sender, GameEventArgs e)
        {
            unloadedScene = true;
        }

        public void RestartGame()
        {
            restartGame = true;
        }
    }
}