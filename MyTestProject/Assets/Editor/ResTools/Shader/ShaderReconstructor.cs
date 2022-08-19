/*******************************************************************
** 文件名: ShaderReconstructor.cs
** 版  权:    (C) 深圳冰川网络技术有限公司 
** 创建人:     钟小虬
** 日  期:    2021/11/18
** 版  本:    1.0
** 描  述:    逆向Shader自动生成器，自动从逆向数据生成GLSL版本的Shader
** 应  用:    

**************************** 修改记录 ******************************
** 修改人:  
** 日  期: 
** 描  述: 
********************************************************************/
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

public class ShaderReconstructor : EditorWindow
{
    [System.Serializable]
    public class PassData
    {
        private string vertUBODataFilePath;
        private string fragUBODataFilePath;

        private string vertProgramFilePath;
        private string fragProgramFilePath;

        public LightMode lightMode;

        public BlendMode srcBlendMode = BlendMode.SrcAlpha;
        public BlendMode dstBlendMode = BlendMode.OneMinusSrcAlpha;

        public ZTestMode zTestMode = ZTestMode.LEqual;
        public CullMode cullMode = CullMode.Back;

        private static readonly string AutoCompleteFileMatchSuffixVert = "_vert";
        private static readonly string AutoCompleteFileMatchSuffixFrag = "_frag";

        public string VertUBODataFilePath
        {
            get => vertUBODataFilePath;
            set => SetDataFilePath(ref vertUBODataFilePath, value);
        }
        public string FragUBODataFilePath
        {
            get => fragUBODataFilePath;
            set => SetDataFilePath(ref fragUBODataFilePath, value);
        }
        public string VertProgramFilePath
        {
            get => vertProgramFilePath;
            set => SetDataFilePath(ref vertProgramFilePath, value);
        }
        public string FragProgramFilePath
        {
            get => fragProgramFilePath;
            set => SetDataFilePath(ref fragProgramFilePath, value);
        }

        public PassData(LightMode lightMode)
        {
            this.lightMode = lightMode;
        }
                public bool ValidCheck()
        {
            bool valid = FilePathValidCheck(ref vertUBODataFilePath);
            valid &= FilePathValidCheck(ref fragUBODataFilePath);
            valid &= FilePathValidCheck(ref vertProgramFilePath);
            valid &= FilePathValidCheck(ref fragProgramFilePath);
            return valid;
        }

        private void AutoCompleteDataFilePaths(string changedFilePath)
        {
            if (!string.IsNullOrEmpty(changedFilePath) && File.Exists(changedFilePath))
            {
                string autoMatchPathPrefix = null;
                if (changedFilePath.Contains(AutoCompleteFileMatchSuffixVert))
                {
                    autoMatchPathPrefix = changedFilePath.Substring(0, changedFilePath.LastIndexOf(AutoCompleteFileMatchSuffixVert));
                }
                else if (changedFilePath.Contains(AutoCompleteFileMatchSuffixFrag))
                {
                    autoMatchPathPrefix = changedFilePath.Substring(0, changedFilePath.LastIndexOf(AutoCompleteFileMatchSuffixFrag));
                }

                if (!string.IsNullOrEmpty(autoMatchPathPrefix))
                {
                    AutoSetDataFilePath(ref vertUBODataFilePath, autoMatchPathPrefix + AutoCompleteFileMatchSuffixVert + ".csv");
                    AutoSetDataFilePath(ref fragUBODataFilePath, autoMatchPathPrefix + AutoCompleteFileMatchSuffixFrag + ".csv");
                    AutoSetDataFilePath(ref vertProgramFilePath, autoMatchPathPrefix + AutoCompleteFileMatchSuffixVert + ".glsl");
                    AutoSetDataFilePath(ref fragProgramFilePath, autoMatchPathPrefix + AutoCompleteFileMatchSuffixFrag + ".glsl");
                }
            }//outer if end           
        }

        private void SetDataFilePath(ref string targetPath, string pathValue)
        {
            if (targetPath != pathValue)
            {
                targetPath = pathValue;
                AutoCompleteDataFilePaths(targetPath);
            }
        }

        private void AutoSetDataFilePath(ref string targetPath, string pathValue)
        {
            if (string.IsNullOrEmpty(targetPath) && File.Exists(pathValue))
            {
                targetPath = pathValue;               
            }          
        }

        private bool FilePathValidCheck(ref string targetPath)
        {
            if (string.IsNullOrEmpty(targetPath) || !File.Exists(targetPath))
            {
                Debug.LogError("Can't find data file: Path" + targetPath);
                return false;
            }
            return true;
        }
    }   

    public enum RenderPipeline
    {
        URPFoward,
        BuiltinFoward,
    }

    public enum RenderType
    {
        Opaque,       
        Transparent,
        //Cutout,
    }

    public enum BlendMode
    {       
        Zero = 0,       
        One = 1,       
        SrcAlpha = 5,        
        DstAlpha = 7,       
        OneMinusDstAlpha = 8,
        SrcAlphaSaturate = 9,
        OneMinusSrcAlpha = 10
    }

    public enum ZTestMode
    {
        Less,
        LEqual,
        Equal,
        GEqual,
        Greater,
        NotEqual,
        Always,
    }

    public enum LightMode
    {
        Base,
        Always,
    }
 
    private PassData m_BasePassData = new PassData(LightMode.Base);
    private PassData m_ExtraPassData = new PassData(LightMode.Always);
    private bool m_ExtraPassEnabled = false;
    private string m_ShaderFileSavePath;
    private RenderPipeline m_RenderPipleLine = RenderPipeline.URPFoward;
    private RenderType m_RenderType = RenderType.Opaque;
    private bool m_ZWrite = true;   
    private string m_ShaderName = "Test001";
    private bool m_FromUnity = true;

    private string m_ConfigDir;
    private Dictionary<string, string> m_PropertyDic; //PropertyName - PropertyStr
    private string[] m_UnityBuiltinUniformMap;
    //private List<string> m_SpecialUniformDataList;
    private List<UniformDataRecord.UniformData> m_UniformDataList;

    private Vector2 m_ScrollPos;

    private static readonly string ShaderTemplateFileName = "ShaderTemplate";
    private static readonly string PassTemplateFileName = "ShaderTemplate-Pass";
    private static readonly string UnityBulitinUniformMapFileName = "Builtin-Unity.csv";
    private static readonly string ShaderNamePrefix = "GLA/Test Only/";

    private static readonly string ShaderNameHolder = "SHADER_NAME_HOLDER";
    private static readonly string PropertyHolder = "PROPERTY_HOLDER";
    private static readonly string PassHolder = "PASS_HOLDER";
    private static readonly string TagHolder = "TAG_HOLDER";
    private static readonly string CommandHolder = "COMMAND_HOLDER";
    private static readonly string VertProgramHolder = "VERT_PROGRAM_HOLDER";
    private static readonly string FragProgramHolder = "FRAG_PROGRAM_HOLDER";     

    [MenuItem("XCore/Res Tools/Shader/Open Shader Reconstructor")]
    public static void ShowWindow()
    {
        ShaderReconstructor reconstructor = EditorWindow.GetWindow(typeof(ShaderReconstructor)) as ShaderReconstructor;
        reconstructor.ShowUtility();
    }

    void OnGUI()
    {
        m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);        

        DrawSubShaderDataView();
       
        DrawPassDataView(m_BasePassData, true);

        m_ExtraPassEnabled = EditorGUILayout.Toggle("Enable Extra Pass", m_ExtraPassEnabled);
        if (m_ExtraPassEnabled)
        {
            DrawPassDataView(m_ExtraPassData);
        }

        if (GUILayout.Button("Rebuild"))
        {
            RebuildShader();            
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawSubShaderDataView()
    {
        m_ShaderName = EditorGUILayout.TextField("Shader Name", m_ShaderName);
        m_RenderPipleLine = (RenderPipeline)EditorGUILayout.EnumPopup("Render Pipeline", m_RenderPipleLine);
        m_RenderType = (RenderType)EditorGUILayout.EnumPopup("Rendering Mode", m_RenderType);       
    }

    private void DrawPassDataView(PassData passData, bool basePass = false)
    {
        EditorGUILayout.BeginVertical("Box");
        float labelWidth = EditorStyles.label.CalcSize(new GUIContent("Vert UBO Data ")).x;

        DrawEnumSettingView<ZTestMode>(ref passData.zTestMode, "ZTest", labelWidth);
        DrawEnumSettingView<CullMode>(ref passData.cullMode, "Cull Mode", labelWidth);
       
        if (m_RenderType == RenderType.Transparent)
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Blend Mode", GUILayout.MaxWidth(labelWidth));
            passData.srcBlendMode = (BlendMode)EditorGUILayout.EnumPopup( passData.srcBlendMode);
            passData.dstBlendMode = (BlendMode)EditorGUILayout.EnumPopup(passData.dstBlendMode);            
            EditorGUILayout.EndVertical();            
        }

        EditorGUIUtility.labelWidth = labelWidth;
        passData.VertUBODataFilePath = CustomEditorGUILayout.FilePathField("Vert UBO Data", passData.VertUBODataFilePath, "csv");
        passData.FragUBODataFilePath = CustomEditorGUILayout.FilePathField("Frag UBO Data", passData.FragUBODataFilePath, "csv");
        passData.VertProgramFilePath = CustomEditorGUILayout.FilePathField("Vert Program", passData.VertProgramFilePath, "glsl");
        passData.FragProgramFilePath = CustomEditorGUILayout.FilePathField("Frag Program", passData.FragProgramFilePath, "glsl");
        
        EditorGUIUtility.labelWidth = 0;
        EditorGUILayout.EndVertical();
    }

    private void DrawEnumSettingView<T>(ref T setting, string label, float labelWidth) where T : Enum
    {
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField(label, GUILayout.MaxWidth(labelWidth));
        setting = (T)EditorGUILayout.EnumPopup(setting);
        EditorGUILayout.EndVertical();
    }

    private void RebuildShader()
    {
        if (!ValidCheck())
        {
            Debug.LogError("Shader reconstruct was canceled because of error!");
            return;
        }

        InitData();

        string shaderTemplateFilePath = Path.Combine(GetCurrentDirectory(), ShaderTemplateFileName);      
        m_ShaderFileSavePath = EditorUtility.SaveFilePanel("Save Shader", m_ShaderFileSavePath, "NewShader","shader");

        List<string> content = new List<string>();
        List<string> basePassContent = GenerateShaderPass(m_BasePassData);
        List<string> extPassContent = m_ExtraPassEnabled? GenerateShaderPass(m_ExtraPassData) : null;

        //因为属性的生成依赖于Uniform数据及Program里面的定义，所以放到最后
        List<string> propertyContent = GenerateProperty();
        try
        {            
            using (StreamReader sr = new StreamReader(shaderTemplateFilePath))
            {
                string line;
                string lineTrimed;
                while ((line = sr.ReadLine()) != null)
                {
                    lineTrimed = line.TrimStart();
                    if (lineTrimed.StartsWith(ShaderNameHolder))
                    {
                        content.Add(GenerateShaderName());
                    }
                    else if (lineTrimed.StartsWith(PropertyHolder))
                    {
                        content.AddRange(propertyContent);
                    }
                    else if (lineTrimed.StartsWith(TagHolder))
                    {
                        content.Add(GenerateSubShaderTag());
                    }
                    else if (lineTrimed.StartsWith(PassHolder))
                    {
                        content.AddRange(basePassContent);
                        if (m_ExtraPassEnabled)
                        {
                            content.Add("\r\n");
                            content.AddRange(extPassContent);
                        }
                    }
                    else
                    {
                        content.Add(line);
                    }                    
                }//while end
            }//using end
        } 
        catch (Exception e)
        {
            Debug.LogError("The file could not be read:" + shaderTemplateFilePath);
            Debug.LogError(e.Message);
        }       

        if (!string.IsNullOrEmpty(m_ShaderFileSavePath))
        {
            File.WriteAllLines(m_ShaderFileSavePath, content);

            if (m_UniformDataList.Count > 0)
            {
                UniformDataRecord record = ScriptableObject.CreateInstance<UniformDataRecord>();
                record.uniformDataList = m_UniformDataList;
                string savePath = m_ShaderFileSavePath.Replace(".shader", "_uniform.asset");
                savePath = savePath.Substring(Application.dataPath.Length - 6); // 6 = length("Assets")
                AssetDatabase.CreateAsset(record, savePath);               
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();    
            Debug.Log("Shader reconstruct successful! Path:" + m_ShaderFileSavePath);
        }          
    }

    private bool ValidCheck()
    {
        bool valid = m_BasePassData.ValidCheck();
        if (m_ExtraPassEnabled)
        {
            valid &= m_ExtraPassData.ValidCheck();
        }
        return valid;
    }

    private void InitData()
    {
        m_ConfigDir =  GetCurrentDirectory();
        m_PropertyDic = new Dictionary<string, string>();
        m_UniformDataList = new List<UniformDataRecord.UniformData>();
        if (m_FromUnity)
        {
            m_UnityBuiltinUniformMap = File.ReadAllLines(Path.Combine(m_ConfigDir, UnityBulitinUniformMapFileName));
        }
        m_ShaderFileSavePath = !string.IsNullOrEmpty(m_ShaderFileSavePath) ? m_ShaderFileSavePath : Application.dataPath;
    }

    private List<string> GenerateShaderPass(PassData passData)
    {
        string passTemplateFilePath = Path.Combine(m_ConfigDir, PassTemplateFileName);      

        List<string> content = new List<string>();
        List<string> vertProgramContent  = GenerateProgramFromFile(passData.VertProgramFilePath);
        List<string> fragProgramContent = GenerateProgramFromFile(passData.FragProgramFilePath);
        ProcessingUniformData(passData.VertUBODataFilePath);
        ProcessingUniformData(passData.FragUBODataFilePath);
        try
        {
            using (StreamReader sr = new StreamReader(passTemplateFilePath))
            {
                string line;
                string lineTrimed;
                while ((line = sr.ReadLine()) != null)
                {
                    lineTrimed = line.TrimStart();
                    if (lineTrimed.StartsWith(TagHolder))
                    {
                        content.Add(GeneratePassTag(passData.lightMode));
                    }
                    else if (lineTrimed.StartsWith(CommandHolder))
                    {
                        content.AddRange(GeneratePassCommand(passData));
                    }
                    else if (lineTrimed.StartsWith(VertProgramHolder))
                    {
                        content.AddRange(vertProgramContent);
                    }
                    else if (lineTrimed.StartsWith(FragProgramHolder))
                    {
                        content.AddRange(fragProgramContent);
                    }
                    else
                    {
                        content.Add(line);
                    }
                }
            }
        } catch (Exception e)
        {
            Debug.LogError("Read failed:" + passTemplateFilePath);
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
        return content;
    }

    private List<string> GenerateProperty()
    {
        List<string> content = new List<string>();
        foreach (var item in m_PropertyDic)
        {
            content.Add(item.Value);
        }
        return content;
    }

    private string GenerateShaderName()
    {
        string shaderName = ShaderNamePrefix + m_ShaderName;
        return string.Format(@"Shader ""{0}""", shaderName);
    }

    private string GenerateSubShaderTag()
    {
        string renderQueueStr = m_RenderType == RenderType.Opaque ? "Geometry" : "Transparent";
        string tagsContentStr = string.Format(@"""Queue"" = ""{0}""", renderQueueStr);
        string renderTypeStr = m_RenderType == RenderType.Opaque ? "Opaque" : "Transparent";
        tagsContentStr += string.Format(@" ""RenderType"" = ""{0}""", renderTypeStr);
        if (m_RenderPipleLine == RenderPipeline.URPFoward)
        {
            tagsContentStr += @" ""RenderPipeline"" = ""UniversalPipeline""";
        }
        string tags = string.Format(@"Tags {{ {0} }}", tagsContentStr);
        tags = "\t\t" + tags;
        return tags;
    }

    private string GeneratePassTag(LightMode lightMode)
    {
        string lightModeStr = GetLightModeString(lightMode);
        string tags = string.Format(@"Tags {{ ""LightMode"" = ""{0}"" }}", lightModeStr);
        tags = "\t\t\t" + tags;
        return tags;
    }

    private List<string> GeneratePassCommand(PassData passData)
    {
        List<string> commands = new List<string>();
        if (m_RenderType == RenderType.Transparent)
        {
            commands.Add(string.Format("\t\t\tBlend {0} {1}", passData.srcBlendMode.ToString(), passData.dstBlendMode.ToString()));
        }
        commands.Add(string.Format("\t\t\tZWrite {0}", m_ZWrite? "On" : "Off"));
        commands.Add(string.Format("\t\t\tZTest {0}", passData.zTestMode.ToString()));
        commands.Add(string.Format("\t\t\tCull {0}", passData.cullMode.ToString()));

        return commands;
    }

    private List<string> ProcessingUniformData(string dataFilePath)
    {
        List<string> properties = new List<string>();
        try
        {
            using (StreamReader sr = new StreamReader(dataFilePath))
            {
                string uniformType, uniformName, uniformValue, uniformDesc;
                //跳过标题
                string line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    //过滤unity内置定义
                    if (IsUnityBuiltinData(line))
                    {
                        continue;
                    }

                    //Data Format Example:_BloodColor,"0.86667, 0.43922, 0.43922, 1.00",float4                    
                    if (line.Contains("\""))
                    {
                        string[] tmpData = SplitUniformDataLine(line);
                        uniformType = tmpData[2];
                        uniformName = tmpData[0];
                        uniformValue = GetProcessedUniformValueString(tmpData[1]);
                        uniformDesc = GetDescFromUnifromName(uniformName);
                        //propDesc = Regex.Replace(propName.TrimStart('_'), "([^A-Z\\s])([A-Z])", "$1 $2");
                        UniformDataCheck(uniformName, uniformValue, uniformDesc, uniformType);
                    } else
                    {
                        //TODO Matrix等不能使用属性序列化的数据保存成额外数据，通过脚本设置
                        string[]  tmpData = line.Split(',');
                        //Matrix,如float4x4
                        if (tmpData[2].Contains("x"))
                        {
                            RecordSpecialUniformData(tmpData[0], "Matrix", tmpData[2], new string[] { "x", " " }, 0, 1, sr);
                        }
                        //数组，如float4[4]
                        else if (tmpData[2].Contains("["))
                        {
                            RecordSpecialUniformData(tmpData[0], "Array", tmpData[2], new string[] { "[", "]" }, 0, 1, sr);                            
                        }
                    }//else end                    
                                              
                }//while end
            }
        }
        catch (Exception e)
        {
            Debug.LogError("read failed:" + dataFilePath);
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
        return properties;
    }

    private void RecordSpecialUniformData(string uniformName, string uniformType, string typeString, string[] typeSeparator, int labelStringIndex, int countStrIndex, StreamReader sr)
    {
        string[] typeDatas = typeString.Split(typeSeparator, StringSplitOptions.None);
        int count = int.Parse(typeDatas[countStrIndex]);   
        string typeLabel = typeDatas[labelStringIndex];
        UniformDataRecord.UniformData uniformData = new UniformDataRecord.UniformData(uniformName, uniformType);
        for (int i = 0; i < count; i++)
        {
            string line = sr.ReadLine();
            string[] valueDatas = SplitUniformDataLine(line);
            string valueStr = valueDatas[1];
            string[] values = valueStr.Split(',');
            uniformData.colNum = Mathf.Max(uniformData.colNum, values.Length);
            Vector4 itemValue = new Vector4(float.Parse(values[0]), values.Length>1? float.Parse(values[1]):0.0f, values.Length > 2 ? float.Parse(values[2]) : 0.0f, values.Length > 3 ? float.Parse(values[3]) : 0.0f);
            uniformData.uniformValues.Add(itemValue);
        }
        
        m_UniformDataList.Add(uniformData);
    }

    private List<string> GenerateProgramFromFile(string filePath)
    {
        List<string> program = new List<string>();
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {     
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("sampler2D"))
                    {
                        string[] datas = line.Split(' ');
                        string texName = datas[datas.Length - 1].TrimEnd(';');
                        UniformDataCheck(texName, "white", GetDescFromUnifromName(texName), "2D");
                    }
                    program.Add("\t\t\t" + line);
                }
            }
        } 
        catch (Exception e)
        {
            Debug.LogError("read failed:" + filePath);
            Debug.LogError(e.Message);
        }       
        return program;
    }

    private void UniformDataCheck(string unifomName, string uniformValue, string uniformDesc, string uniformType)
    {
        if (!m_PropertyDic.ContainsKey(unifomName))
        {
            string tmpProperty = "";
            if (uniformType == "float")
            {
                tmpProperty = string.Format(@"{0}(""{1}"", Float) = {2}", unifomName, uniformDesc, uniformValue);
            }
            else if (uniformType == "float2")
            {
                tmpProperty = string.Format(@"{0}(""{1}"", Vector) = ({2})", unifomName, uniformDesc, uniformValue + ", 0.0, 0.0");
            }
            else if (uniformType == "float3")
            {
                tmpProperty = string.Format(@"{0}(""{1}"", Vector) = ({2})", unifomName, uniformDesc, uniformValue + ", 0.0");
            }
            else if (uniformType == "float4")
            {
                //检查是否为贴图的_ST定义，如_MainTex_ST
                if (m_FromUnity && unifomName.EndsWith("_ST"))
                {
                    string texName = unifomName.Substring(0, unifomName.LastIndexOf("_ST"));
                    if (m_PropertyDic.ContainsKey(texName))
                    {
                        return;
                    }
                }

                if (unifomName.Contains("Color"))
                {
                    tmpProperty = string.Format(@"{0}(""{1}"", Color) = ({2})", unifomName, uniformDesc, uniformValue);
                }
                else
                {
                    tmpProperty = string.Format(@"{0}(""{1}"", Vector) = ({2})", unifomName, uniformDesc, uniformValue);
                }

            }
            else if (uniformType == "2D")
            {
                tmpProperty = string.Format(@"{0}(""{1}"", 2D) = ""{2}"" ", unifomName, uniformDesc, uniformValue) + "{}";
            }

            if (!string.IsNullOrEmpty(tmpProperty))
            {
                tmpProperty = "\t\t" + tmpProperty;
                m_PropertyDic.Add(unifomName, tmpProperty);
            }            
        } 
    }

    private bool IsUnityBuiltinData(string uniformStr)
    {
        if (m_FromUnity)
        {
            foreach (var item in m_UnityBuiltinUniformMap)
            {
                if (uniformStr.StartsWith(item))
                {
                    return true;
                }
            }  
        }
        return false;
    }

    private string GetLightModeString(LightMode lightMode)
    {
        string lightModeStr = "";
        if (m_RenderPipleLine == RenderPipeline.URPFoward)
        {
            lightModeStr = lightMode == LightMode.Base ? "UniversalForward" : "SRPDefaultUnlit";
        }
        else if (m_RenderPipleLine == RenderPipeline.BuiltinFoward)
        {
            lightModeStr = lightMode == LightMode.Base ? "ForwardBase" : "Always";
        } 
        return lightModeStr;
    }

    private string GetProcessedUniformValueString(string srcStr)
    {
        string dstStr = srcStr.Trim('\"');
        if (dstStr.Contains("E"))
        {
            string[] values = dstStr.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Contains("E"))
                {
                    values[i] = " " + Double.Parse(values[i], NumberStyles.Float, CultureInfo.InvariantCulture).ToString("N2");
                }                
            }
            dstStr = string.Join(",", values);
        }
        return dstStr;
    }

    private string GetDescFromUnifromName(string unifromName)
    {
        string desc = Regex.Replace(unifromName.TrimStart('_'), @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
        return desc;
    }

    private string[] SplitUniformDataLine(string line)
    {
        return line.Split(new string[] { ",\"", "\"," }, StringSplitOptions.None);
    }

    private string GetCurrentDirectory()
    {
        MonoScript ms = MonoScript.FromScriptableObject(this);
        string path = AssetDatabase.GetAssetPath(ms);
        return path.Substring(0, path.LastIndexOf('/') + 1);
    }
}