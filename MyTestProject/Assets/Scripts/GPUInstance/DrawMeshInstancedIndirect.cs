using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using Unity.Collections;


public class DrawMeshInstancedIndirect : MonoBehaviour
{
    public int instanceCount = 100000;
    public float range = 0.0f;
    public Mesh instanceMesh;
    public Material instanceMaterial;
    //---------------------
    private ComputeBuffer m_patchIndirectArgs;
    private ComputeBuffer m_culledPatchBuffer;
    private ComputeBuffer meshPropertiesBuffer;
    //---------------------
    Plane[] cameraFrustumPlanes;
    Bounds bounds;
    Camera m_camera;
    Shader m_shader;
    CommandBuffer m_commandBuffer;
    public ComputeShader m_computeShader;

    private ComputeBuffer m_cbuffer1;

    private NativeArray<int> testArray1 = new NativeArray<int>();

    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        bounds = new Bounds(Vector3.zero, Vector3.one * instanceCount * 512);

   //     m_culledPatchBuffer = new ComputeBuffer( instanceCount , sizeof(float)*3, ComputeBufferType.Append);


   //     m_patchIndirectArgs = new ComputeBuffer(instanceCount, instanceCount*2* 4, ComputeBufferType.IndirectArguments);
  //      m_patchIndirectArgs.SetData(new uint[] { instanceMesh.GetIndexCount(0), (uint)instanceCount, 0, 0, 0 });
    }

    // Update is called once per frame
    void Update()
    {
        DrawTerrain();
    }

    //private struct MeshProperties
    //{
    //    public Matrix4x4 mat;
    //    public Vector4 color;

    //    public static int Size()
    //    {
    //        return
    //            sizeof(float) * 4 * 4 + // matrix;
    //            sizeof(float) * 4;      // color;
    //    }

    //}

    void Dispatch()
    {
        //m_culledPatchBuffer = new ComputeBuffer( , ,ComputeBufferType.Append);
        //   m_commandBuffer.DispatchCompute(m_computeShader,0,);


        // TestCSMain1();
        // m_commandBuffer.CopyCounterValue(m_culledPatchBuffer, m_patchIndirectArgs, 4);
        // instanceMaterial.SetBuffer("PatchList", m_culledPatchBuffer);

        #region
        //MeshProperties[] properties = new MeshProperties[instanceCount];
        //for (int i = 0; i < instanceCount; i++)
        //{
        //    MeshProperties props = new MeshProperties();
        //    Vector3 position = new Vector3( ((i/24.0f)-(int)(i/24.0f))*24.0f *256.0f,0.0f,(int)(i / 24.0f) * 256.0f) ;
        //    Quaternion rotation = Quaternion.identity;//Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        //    Vector3 scale = Vector3.one;

        //    props.mat = Matrix4x4.TRS(position, rotation, scale);
        //    props.color = Color.Lerp(Color.red, Color.blue, Random.value);
        //    properties[i] = props;
        //}

        //meshPropertiesBuffer = new ComputeBuffer(instanceCount, MeshProperties.Size());
        //meshPropertiesBuffer.SetData(properties);
        //instanceMaterial.SetBuffer("_Properties", meshPropertiesBuffer);

        //// indirect args
        //uint numIndices = (instanceMesh != null) ? (uint)instanceMesh.GetIndexCount(0) : 0;
        //args[0] = numIndices;
        //args[1] = (uint)instanceCount;
        //args[2] = (uint)instanceMesh.GetIndexStart(0);
        //args[3] = (uint)instanceMesh.GetBaseVertex(0);
        //argsBuffer.SetData(args);
        #endregion
    }

    void DrawTerrain()
    {
        // Dispatch();
       //  Graphics.DrawMeshInstancedIndirect(instanceMesh, 0, instanceMaterial, bounds, m_patchIndirectArgs);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestCSMain1();
        }


    }

    void TestCSMain1()
    {
        //   m_cbuffer1 = new ComputeBuffer(47*47*1*1*1*1, sizeof(int));
        //   m_computeShader.SetBuffer(0, "testArray1", m_cbuffer1);
        //   m_computeShader.Dispatch(0, 47, 47, 1);



        m_culledPatchBuffer = new ComputeBuffer(instanceCount*2, sizeof(float), ComputeBufferType.Append);

        m_computeShader.SetBuffer(0, "CulledPatchList", m_culledPatchBuffer);
       // m_cbuffer1 = new ComputeBuffer( 1*1*1*8*8*8, sizeof(int));
       // m_computeShader.SetBuffer(0, "testArray1", m_cbuffer1);

        m_computeShader.Dispatch(0,1,1,1);


        float[] test = new float[instanceCount];

        m_culledPatchBuffer.GetData(test);


        for (int i = 0; i < test.Length; i++)
        {
            Debug.Log("Test result at " + i + " : " + test[i]);
        }

        m_culledPatchBuffer.Release();


        //UnityEngine.Rendering.AsyncGPUReadback.Request(m_cbuffer1, (x) =>
        //{
        //    testArray1 = x.GetData<int>();

        //    string tempLog = string.Empty;
        //    int count = 1;
        //    for (int i = 0; i < testArray1.Length; ++i)
        //    {
        //        tempLog += $"{testArray1[i]}/";
        //        count += 1;
        //    }
        //    Debug.Log(tempLog);
        //    Debug.Log(count);
        //});
    }
}
