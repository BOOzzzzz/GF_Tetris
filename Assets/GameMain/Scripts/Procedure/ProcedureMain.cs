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
        public Transform[,] grid;

        private bool isClear;
        private EntitySpawner entitySpawner;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            grid = new Transform[width, height];
            entitySpawner = new EntitySpawner();
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            GameEntry.Event.Subscribe(SpawnBlockEventArgs.EventId, SpawnBlock);
            entitySpawner.SpawnEntity(this);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            
            GameEntry.Event.Unsubscribe(SpawnBlockEventArgs.EventId, SpawnBlock);
        }

        private void SpawnBlock(object sender, GameEventArgs e)
        {
            entitySpawner.SpawnEntity(this);
        }

        public void ClearTheRows(int min,int max)
        {
            for (int i = min; i <= max; i++)
            {
                isClear = true;
                for (int j = 0; j < width; j++)
                {
                    if (grid[j, i]== null)
                    {
                        isClear = false;
                        break;
                    }
                }

                if (isClear)
                {
                    for (int j = 0; j < width; j++)
                    {
                        grid[j, i].gameObject.SetActive(false);
                        grid[j, i] = null;
                    }
                }
            }
        }
    }
}