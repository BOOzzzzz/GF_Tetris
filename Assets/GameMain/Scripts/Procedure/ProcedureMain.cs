using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BOO.Procedure
{
    public class ProcedureMain : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(2);
            GameEntry.Entity.ShowEntity<EntityBlock>(1, AssetUtility.GetEntityAsset(drEntity.AssetName), drEntity.AssetGroup ,userData: drEntity.OriginPosition);
        }
    }
}