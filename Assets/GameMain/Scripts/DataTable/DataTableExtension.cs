using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

public static class DataTableExtension
{
    internal static readonly char[] DataSplitSeparators = new char[] { '\t' };
    internal static readonly char[] DataTrimSeparators = new char[] { '\"' };
    private const string classTypePrefixName = "BOO.DR";

    public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName,
        string dataTableAssetName,object userData)
    {
        string classTypeName = classTypePrefixName + dataTableName;
        Type classType = Type.GetType(classTypeName);
        DataTableBase dataTableBase =  dataTableComponent.CreateDataTable(classType);
        dataTableBase.ReadData(dataTableAssetName,userData);
    }
        
    public static Vector2 ParseVector2(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Vector2(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]));
    }
    
    public static Vector3 ParseVector3(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]),float.Parse(splitedValue[2]));
    }
    
    public static Color ParseColor(string value)
    {
        string[] splitedValue = value.Split(',');
        return new Color(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
    }

    public static List<Vector2> ParseListVector2(string value)
    {
        List<Vector2> listV2 = new List<Vector2>();
        string[] splitedValue = value.Split(';');
        foreach (var t in splitedValue)
        {
            string[] splitedVector2 = t.Trim().Split(',');
            listV2.Add(new Vector2(float.Parse(splitedVector2[0]), float.Parse(splitedVector2[1])));
        }

        return listV2;
    }
}