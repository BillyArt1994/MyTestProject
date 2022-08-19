/*******************************************************************
** �ļ���: UniformDataBinder.cs
** ��  Ȩ:    (C) ���ڱ������缼�����޹�˾ 
** ������:     ��С�
** ��  ��:    2021/11/18
** ��  ��:    1.0
** ��  ��:    Uniform���ݰ󶨸�����(��������ʹ�ã�
** Ӧ  ��:    

**************************** �޸ļ�¼ ******************************
** �޸���:  
** ��  ��: 
** ��  ��: 
********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class UniformDataBinder : MonoBehaviour
{
    public Renderer targetRenderer;
    public UniformDataRecord uniformDataRecord;

    void Start()
    {
        if (uniformDataRecord == null)
        {
            return;
        }

        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        if (targetRenderer != null)
        {
            Material targetMat = targetRenderer.sharedMaterial;
            if (targetMat != null)
            {
                foreach (var data in uniformDataRecord.uniformDataList)
                {
                    if (data.uniformType == "Array")
                    {
                        if (data.colNum == 1)
                        {
                            float[] values = new float[data.uniformValues.Count];
                            for (int i = 0; i < data.uniformValues.Count; i++)
                            {
                                values[i] = data.uniformValues[i].x;
                            }
                            targetMat.SetFloatArray(data.uniformName, values);
                        }
                        else
                        {
                            targetMat.SetVectorArray(data.uniformName, data.uniformValues);
                        }
                    }
                    else if (data.uniformType == "Matrix")
                    {
                        Matrix4x4 matrix = new Matrix4x4();
                        matrix.SetRow(0, data.uniformValues[0]);
                        matrix.SetRow(1, data.uniformValues.Count > 1 ? data.uniformValues[1] : Vector4.zero);
                        matrix.SetRow(2, data.uniformValues.Count > 2 ? data.uniformValues[2] : Vector4.zero);
                        matrix.SetRow(3, data.uniformValues.Count > 3 ? data.uniformValues[3] : Vector4.zero);
                        targetMat.SetMatrix(data.uniformName, matrix);
                    }
                }//foreach end
            }//targetMat null check end
        }
    }       
}
