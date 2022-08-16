using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader m_cs;
    private ComputeBuffer m_Buffer;
    private NativeArray<int> testArray1 = new NativeArray<int>();


    void TestCSMain()
    {
        m_Buffer = new ComputeBuffer(5 * 3 * 2 * 10 * 8 * 3, sizeof(int));
        m_cs.SetBuffer(0, "testArray1", m_Buffer);

        m_cs.Dispatch(0, 5, 3, 2);


        UnityEngine.Rendering.AsyncGPUReadback.Request(m_Buffer, (x) =>
        {
            testArray1 = x.GetData<int>();

            string tempLog = string.Empty;
            for (int i = 0; i < testArray1.Length; ++i)
            {
                tempLog += $"{testArray1[i]}/";
            }
            Debug.Log(tempLog);
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestCSMain();
        }

    }

}
