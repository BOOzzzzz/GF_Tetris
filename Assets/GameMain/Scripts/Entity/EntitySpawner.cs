using BOO.Procedure;
using GameFramework.DataTable;
using UnityEngine;

namespace BOO
{
    public class EntitySpawner
    {
         public void SpawnEntity(object data = null)
         {
             // Spawn an entity
             IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
             DREntity drEntity = dtEntity.GetDataRow(Random.Range(1,8));
             ProcedureMain procedureMain = data as ProcedureMain;
             procedureMain.originPosition = drEntity.OriginPosition;
             GameEntry.Entity.ShowEntity<EntityBlock>(GameEntry.Entity.GenerateSerialID(),
                 AssetUtility.GetEntityAsset(drEntity.AssetName), drEntity.AssetGroup, userData: data);
         }
    }
}