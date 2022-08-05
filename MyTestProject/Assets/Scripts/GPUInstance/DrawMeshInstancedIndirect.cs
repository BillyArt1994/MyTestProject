using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMeshInstancedIndirect : MonoBehaviour
{
    public int instanceCount = 100000;
    public float range = 0.0f;
    public Mesh instanceMesh;
    public Material instanceMaterial;
    //---------------------
    private ComputeBuffer argsBuffer;
    private ComputeBuffer meshPropertiesBuffer;
    //---------------------
    Plane[] cameraFrustumPlanes;
    Bounds bounds;
    Camera m_camera;
    Shader m_shader;

    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        bounds = new Bounds(Vector3.zero, Vector3.one * instanceCount * 512);
        argsBuffer = new ComputeBuffer(1, args.Length* sizeof(uint),ComputeBufferType.IndirectArguments);
        UpdateBuffer();
    //  argsBuffer.SetData();
    }

    // Update is called once per frame
    void Update()
    {
      //  UpdateBuffer();
        DrawTerrain();
    }

    private struct MeshProperties
    {
        public Matrix4x4 mat;
        public Vector4 color;

        public static int Size()
        {
            return
                sizeof(float) * 4 * 4 + // matrix;
                sizeof(float) * 4;      // color;
        }
    }

    void UpdateBuffer()
    {
        MeshProperties[] properties = new MeshProperties[instanceCount];
        for (int i = 0; i < instanceCount; i++)
        {
            MeshProperties props = new MeshProperties();
            Vector3 position = new Vector3( ((i/24.0f)-(int)(i/24.0f))*24.0f *256.0f,0.0f,(int)(i / 24.0f) * 256.0f) ;
            Quaternion rotation = Quaternion.identity;//Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
            Vector3 scale = Vector3.one;

            props.mat = Matrix4x4.TRS(position, rotation, scale);
            props.color = Color.Lerp(Color.red, Color.blue, Random.value);
            properties[i] = props;
        }

        meshPropertiesBuffer = new ComputeBuffer(instanceCount, MeshProperties.Size());
        meshPropertiesBuffer.SetData(properties);
        instanceMaterial.SetBuffer("_Properties", meshPropertiesBuffer);

        // indirect args
        uint numIndices = (instanceMesh != null) ? (uint)instanceMesh.GetIndexCount(0) : 0;
        args[0] = numIndices;
        args[1] = (uint)instanceCount;
        args[2] = (uint)instanceMesh.GetIndexStart(0);
        args[3] = (uint)instanceMesh.GetBaseVertex(0);
        argsBuffer.SetData(args);
    }

    void DrawTerrain()
    {
        Graphics.DrawMeshInstancedIndirect(instanceMesh, 0, instanceMaterial, bounds, argsBuffer);
    }

    void SendFrustumDataToShader()
    {
        GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>(), cameraFrustumPlanes);

    }

}
