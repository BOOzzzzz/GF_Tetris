using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace BOO.Procedure
{
    public class ProcedureGameOver:ProcedureBase
    {

        private bool restartGame = false; 
        private bool backMenu = false; 
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
                
                procedureOwner.SetData<VarInt32>("NextSceneId",2);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
            
            if (backMenu)
            {
                backMenu = false;
                
                procedureOwner.SetData<VarInt32>("NextSceneId",1);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }


        public void RestartGame()
        {
            restartGame = true;
        }
        
        public void BackMenu()
        {
            backMenu = true;
        }
    }
}