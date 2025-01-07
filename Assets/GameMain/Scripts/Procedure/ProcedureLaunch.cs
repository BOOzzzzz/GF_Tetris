using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;

namespace BOO.Procedure
{
    public class ProcedureLaunch : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            ChangeState<ProcedureMenu>(procedureOwner);
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("GameMenu"));
        }
    }
}
