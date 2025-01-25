using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameMain.Scripts.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace BOO.Procedure
{
    public class ProcedureMain : ProcedureBase
    {
        public int width = 10;
        public int height = 20;
        public Vector2 originPosition;
        public Vector2 pivot;
        public Transform[,] grid;
        
        private bool gameOver = false;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            grid = new Transform[width, height];
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(SpawnBlockEventArgs.EventId, SpawnBlock);
            GameEntry.Event.Subscribe(GameOverEventArgs.EventId, GameOverEvent);
            GameEntry.Event.Fire(this, SpawnBlockEventArgs.Create());
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (gameOver)
            {
                gameOver = false;
                ChangeState<ProcedureGameOver>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(SpawnBlockEventArgs.EventId, SpawnBlock);
            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId, GameOverEvent);
        }

        private void GameOverEvent(object sender, GameEventArgs e)
        {
            gameOver = true;
        }

        private void SpawnBlock(object sender, GameEventArgs e)
        {
            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(Random.Range(1, 8));
            originPosition = drEntity.OriginPosition;
            pivot = drEntity.Pivot;
            GameEntry.Entity.ShowEntity<EntityBlock>(GameEntry.Entity.GenerateSerialID(),
                AssetUtility.GetEntityAsset(drEntity.AssetName), drEntity.AssetGroup, userData: this);
        }

        public void ClearTheRows(int min, int max)
        {
            for (int i = max; i >= min; i--)
            {
                if (HasLine(i))
                {
                    DeleteLine(i);
                    DropLine(i);
                }
            }
        }

        private bool HasLine(int line)
        {
            for (int i = 0; i < width; i++)
            {
                if (grid[i, line] == null)
                    return false;
            }

            return true;
        }

        private void DeleteLine(int line)
        {
            for (int i = 0; i < width; i++)
            {
                grid[i, line].gameObject.SetActive(false);
                grid[i, line] = null;
            }
        }

        private void DropLine(int line)
        {
            for (int j = line; j < height - 1; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (grid[i, j + 1] != null)
                    {
                        grid[i, j] = grid[i, j + 1];
                        grid[i, j + 1] = null;
                        grid[i, j].transform.position += Vector3.down;
                    }
                }
            }
        }
    }
}