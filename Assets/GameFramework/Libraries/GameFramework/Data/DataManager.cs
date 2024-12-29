using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Data
{
    internal class DataManager:GameFrameworkModule,IDataManager
    {
        public List<Data> datas = new List<Data>();
        public Dictionary<Type, Data> dicDatas = new Dictionary<Type, Data>();
        
        public void AddData(Data data)
        {
            if (datas.Contains(data))
            {
                throw new GameFrameworkException(Utility.Text.Format("Data Type '{0}' is already exist.", data.ToString()));
                return;
            }
            datas.Add(data);
            dicDatas.Add(data.GetType(),data);
        }

        public Data GetData(Type type)
        {
            return dicDatas[type];
        }

        public T GetData<T>() where T : Data
        {
            Type type = typeof(T);
            return dicDatas[type] as T;
        }

        public List<Data> GetAllDatas()
        {
            return datas;
        }

        public void OnPreload()
        {
            foreach (var data in datas)
            {
                data.OnPreload();
            }
        }

        public void OnLoad()
        {
            foreach (var data in datas)
            {
                data.OnLoad();
            }
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            
        }

        internal override void Shutdown()
        {
            
        }
    }
}