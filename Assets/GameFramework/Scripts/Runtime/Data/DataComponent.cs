using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.Data;
using UnityEngine;
using System.Reflection;

namespace UnityGameFramework.Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Data")]
    public class DataComponent:GameFrameworkComponent
    {
        private IDataManager dataManager = null;

        protected override void Awake()
        {
            base.Awake();
            dataManager = GameFrameworkEntry.GetModule<IDataManager>();
            if (dataManager == null)
            {
                Log.Fatal("Data manager is invalid.");
                return;
            }
        }

        private void Start()
        {
            // 通过反射获取所有继承自 Data 的类型
            Type baseType = typeof(Data);
            Assembly assembly = Assembly.Load("Assembly-CSharp");
            Type[] derivedTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract).ToArray();

            foreach (Type type in derivedTypes)
            {
                Data data = Activator.CreateInstance(type) as Data;

                if (data != null)
                {
                    AddData(data);
                }
            }
        }
        
        public Data GetData(Type type)
        {
            return dataManager.GetData(type);
        }

        public T GetData<T>() where T : Data
        {
            return dataManager.GetData<T>();
        }

        public List<Data> GetAllDatas()
        {
            return dataManager.GetAllDatas();
        }

        public void AddData(Data data)
        {
            dataManager.AddData(data);
        }

        public void OnPreloadAllDatas()
        {
            dataManager.OnPreload();
        }

        public void OnLoadAllDatas()
        {
            dataManager.OnLoad();
        }
    }
}