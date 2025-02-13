using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

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
                
                procedureOwner.SetData<VarInt32>("NextSceneId",2);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
    }
}