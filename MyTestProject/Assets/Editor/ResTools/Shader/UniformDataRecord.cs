/*******************************************************************
** �ļ���: UniformDataRecord.cs
** ��  Ȩ:    (C) ���ڱ������缼�����޹�˾ 
** ������:     ��С�
** ��  ��:    2021/11/18
** ��  ��:    1.0
** ��  ��:    Uniform���ݼ�¼�࣬���ڴ洢�����Uniform����(��Matrix������ȣ�
** Ӧ  ��:    

**************************** �޸ļ�¼ ******************************
** �޸���:  
** ��  ��: 
** ��  ��: 
********************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UniformDataRecord : ScriptableObject
{
    [System.Serializable]
    public class UniformData
    {
        public string uniformName;
        public string uniformType;
        public int colNum;
        public List<Vector4> uniformValues;

        public UniformData(string uniformName, string uniformType)
        {
            this.uniformName = uniformName;
            this.uniformType = uniformType;
            uniformValues = new List<Vector4>();
        }
    }

    public List<UniformData> uniformDataList;
}
