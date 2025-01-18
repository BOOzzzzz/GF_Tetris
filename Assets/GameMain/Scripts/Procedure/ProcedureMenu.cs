using GameFramework.Fsm;
using GameFramework.Procedure;

namespace BOO.Procedure
{
    public class ProcedureMenu:ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.UI.OpenUIForm(AssetUtility.GetUIFormAsset("UIFormMenu"), "Menu");
        }
    }
}