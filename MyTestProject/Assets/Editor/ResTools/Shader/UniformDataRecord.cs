/*******************************************************************
** 文件名: UniformDataRecord.cs
** 版  权:    (C) 深圳冰川网络技术有限公司 
** 创建人:     钟小虬
** 日  期:    2021/11/18
** 版  本:    1.0
** 描  述:    Uniform数据记录类，用于存储特殊的Uniform数据(如Matrix，数组等）
** 应  用:    

**************************** 修改记录 ******************************
** 修改人:  
** 日  期: 
** 描  述: 
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
