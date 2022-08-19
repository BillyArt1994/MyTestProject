/*******************************************************************
** 文件名: ParticleSystemOptimizer.cs
** 版  权:    (C) 深圳冰川网络技术有限公司 
** 创建人:     钟小虬
** 日  期:    2020/11/18
** 版  本:    1.0
** 描  述:    特效优化工具
** 应  用:    1、贴图使用情况分析
**            2、智能特效合批，自动分析合批方案，并进行相关的合批处理

**************************** 修改记录 ******************************
** 修改人:  
** 日  期: 
** 描  述: 
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;


public class ParticleSystemOptimizer : EditorWindow
{   
    private List<Object> m_SearchDirs = new List<Object>();
    private Object m_MatCheckLimitDir;
    private bool m_OnlyCheckMainTex = true;
    
    private Dictionary<Texture, TextureData> m_TextureUsage = new Dictionary<Texture, TextureData>();
    private List<MaterialPreBatchingData> m_MaterialBatchingDatas = new List<MaterialPreBatchingData>();
    private Dictionary<Material, MaterialPreBatchingData> m_MaterialBatchingTable = new Dictionary<Material, MaterialPreBatchingData>();
    private Dictionary<ParticleSystem, int> m_ParticleSystemBatchingTable = new Dictionary<ParticleSystem, int>(); //value- groupID;
    private Dictionary<ParticleSystem, Texture> m_ParticleSystemMainTexTable = new Dictionary<ParticleSystem, Texture>();
    private Dictionary<int, BatchingData> m_BatchingDataTable = new Dictionary<int, BatchingData>();

    private bool m_MatCheckLimitDirEnable = false;
    private string m_MatCheckLimitDirPath;

    private static readonly int MaxBatchingTexSize = 256;
    public static readonly bool IgnoreAlpha = true;
    public static readonly bool AdjustRenderOrder = true;

    [MenuItem("XCore/Res Tools/ParticleSystem/Open ParticleSystem Optimzer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ParticleSystemOptimizer));
    }

    void OnGUI()
    {
        m_SearchDirs = CustomEditorGUILayout.ObjectListField("Prefab Search Directory", m_SearchDirs, typeof(Object));        

        EditorGUILayout.Separator();
        m_MatCheckLimitDir = EditorGUILayout.ObjectField("Material Limit Directory", m_MatCheckLimitDir, typeof(Object), false);

        EditorGUILayout.Separator();
        EditorGUILayout.BeginVertical("Box");
        m_OnlyCheckMainTex = EditorGUILayout.Toggle("Only Check Main Texure", m_OnlyCheckMainTex);            
        if (GUILayout.Button("Check Texture Usage From Prefabs"))
        {
            CheckTextureUsageFromPrefabs();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();
        if (GUILayout.Button("Analysis ParticleSystem Batching For Prefabs"))
        {
            BatchingParticleSytemFromPrefabs(true);
        }

        EditorGUILayout.Separator();
        if (GUILayout.Button("Batching ParticleSystem For Prefabs"))
        {
            BatchingParticleSytemFromPrefabs();
        }    
    }

    private void BatchingParticleSytemFromPrefabs(bool analysisOnly = false)
    {
        InitData();
        CustomEditorUtility.ProcessingFiles<GameObject>(m_SearchDirs, "*.prefab", ProcessingBatchingParticleSystemFromPrefab,false);
        AnalysisBatchingData();
        if (!analysisOnly)
        {
            BatchingParticleSystems();
        }       
        SaveBatchingDataToCsv();      
    }

    private void CheckTextureUsageFromPrefabs()
    {
        InitData();
        CustomEditorUtility.ProcessingFiles<GameObject>(m_SearchDirs, "*.prefab", ProcessingTextureUsageCheckFromPrefab);
        SaveUsageDataToCsv();
    }

    private void InitData()
    {
        MaterialPreBatchingData.groupIndex = 0;
        m_TextureUsage.Clear();
        m_MaterialBatchingDatas.Clear();
        m_MaterialBatchingTable.Clear();
        m_ParticleSystemBatchingTable.Clear();
        m_ParticleSystemMainTexTable.Clear();
        m_BatchingDataTable.Clear();
        if (m_MatCheckLimitDir != null)
        {
            m_MatCheckLimitDirPath = AssetDatabase.GetAssetPath(m_MatCheckLimitDir);
            m_MatCheckLimitDirEnable = true;
        }
        else
        {
            m_MatCheckLimitDirEnable = false;
        }
    }


    private bool ProcessingTextureUsageCheckFromPrefab(GameObject prefab)
    {
        
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            Material[] mats = renderer.sharedMaterials;
            foreach (var mat in mats)
            {              
                if (m_MatCheckLimitDirEnable && !AssetDatabase.GetAssetPath(mat).StartsWith(m_MatCheckLimitDirPath))
                {
                    continue;
                }

                //排除Built-in材质
                if (mat != null && AssetDatabase.IsNativeAsset(mat))
                {
                    ProcessingTextureUsageCheckFromMaterial(mat);
                }               
            }
        }
        return true;
    }

    private bool ProcessingBatchingParticleSystemFromPrefab(GameObject prefab)
    {
        //收集可以合并的材质，并统计材质的使用频率
        ParticleSystem[] particles = prefab.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
        {
            Texture mainTex = null;
            if (ps.textureSheetAnimation.enabled)
            {
                if (ps.textureSheetAnimation.mode == ParticleSystemAnimationMode.Grid)
                {
                    continue;
                }
                else 
                {
                    if (ps.textureSheetAnimation.spriteCount > 1)
                    {
                        continue;
                    }
                    Sprite sprite = ps.textureSheetAnimation.GetSprite(0);
                    if (sprite != null)
                    {
                        mainTex = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GetAssetPath(sprite));
                    }                    
                }
              
            }
            ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
            if (renderer  == null || renderer.sharedMaterial == null || !AssetDatabase.IsNativeAsset(renderer.sharedMaterial))
            {
                continue;
            }

            if (m_MatCheckLimitDirEnable && !AssetDatabase.GetAssetPath(renderer.sharedMaterial).StartsWith(m_MatCheckLimitDirPath))
            {
                continue;
            }

            if (mainTex == null)
            {
                mainTex = TextureUtil.GetMainTexture(renderer.sharedMaterial);
                if (mainTex == null)
                {
                    continue;
                }
            }
            
            if (mainTex.width > MaxBatchingTexSize || mainTex.height > MaxBatchingTexSize)
            {
                continue;
            }
            if (renderer.renderMode != ParticleSystemRenderMode.None)
            {
                PSRenderMode rm = renderer.renderMode == ParticleSystemRenderMode.Mesh ? PSRenderMode.Mesh : PSRenderMode.Billboard;
                int groupID = CheckMaterialBatching(renderer.sharedMaterial,mainTex,rm);
                m_ParticleSystemBatchingTable.Add(ps, groupID);
                m_ParticleSystemMainTexTable.Add(ps, mainTex);
            } 
        }
        return true;
    }

    private bool ProcessingTextureUsageCheckFromMaterial(Material mat)
    {
        if (m_OnlyCheckMainTex)
        {
            AddTextureUsage(mat.mainTexture);
        }
        else
        {
            Shader shader = mat.shader;
            for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
            {
                if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    Texture texture = mat.GetTexture(ShaderUtil.GetPropertyName(shader, i));
                    AddTextureUsage(texture);
                }
            }
        }       
        return true;
    }

    public bool ProcessingMaterialBatchingCheck(Material mat)
    {
        Texture mainTex = TextureUtil.GetMainTexture(mat);
        CheckMaterialBatching(mat, mainTex, PSRenderMode.Billboard);
        return true;
    }

    private int CheckMaterialBatching(Material mat, Texture mainTex, PSRenderMode renderMode)
    {       
        MaterialPreBatchingData data = null;
        if (!m_MaterialBatchingTable.TryGetValue(mat, out data))
        {
            for (int i = 0; i < m_MaterialBatchingDatas.Count; i++)
            {
                if (m_MaterialBatchingDatas[i].CanBatching(mat))
                {
                    data = m_MaterialBatchingDatas[i];
                    break;
                }
            }
            if (data == null)
            {
                data = new MaterialPreBatchingData(mat, TextureUtil.GetMainTextureName(mat));
                m_MaterialBatchingDatas.Add(data);
            }
            m_MaterialBatchingTable[mat] = data;
        }        

        int groupID = data.AddMainTexture(renderMode, mainTex);        
        return groupID;
    }

    private void AnalysisBatchingData()
    {
        CustomEditorUtility.ShowProgress("Executing", "Analysis Batching Data...", 0, 1);
        //检测贴图使用最频繁的可合批组
        Dictionary<Texture, int> maxUsageCountDic = new Dictionary<Texture, int>();//value-maxUsageCount
        Dictionary<Texture, int> texGroupDic = new Dictionary<Texture, int>();//value-groupId
        
        
        foreach (var batchingData in m_MaterialBatchingDatas)
        {
            foreach (var subData in batchingData.subDataList)
            {                
                foreach (var tex in subData.mainTexs)
                {
                    int usageCount;
                    if (!maxUsageCountDic.TryGetValue(tex.Key, out usageCount))
                    {
                        maxUsageCountDic.Add(tex.Key, tex.Value);
                        texGroupDic.Add(tex.Key, subData.groupID);
                    }
                    else if (usageCount < tex.Value)
                    {
                        maxUsageCountDic[tex.Key] = tex.Value;
                        texGroupDic[tex.Key] = subData.groupID;
                    }
                }//tex foreach end

                BatchingData bd = new BatchingData(subData.groupID, batchingData.baseMat, batchingData.mainTexName,subData.haveAlpha, subData.renderMode);
                m_BatchingDataTable.Add(subData.groupID, bd);
            }
        }//outer foreach end

        foreach (var item in texGroupDic)
        {
            BatchingData bd;
            if (m_BatchingDataTable.TryGetValue(item.Value, out bd))
            {
                bd.AddMainTex(item.Key);
            }
        }

        //计算绘制顺序
        List<BatchingData> orderAdjustBatchingData = new List<BatchingData>();
        foreach (var item in m_BatchingDataTable)
        {
            if (item.Value.CanMaterialAdjustRenderOrder())
            {
                orderAdjustBatchingData.Add(item.Value);
            }
        }

        for (int i = 0; i < orderAdjustBatchingData.Count; i++)
        {
            orderAdjustBatchingData[i].sortIndex = i+1;
        }   
    }

    private void BatchingParticleSystems()
    {
        bool anyChanged = false;
        int index = 0;
        foreach (var item in m_BatchingDataTable)
        {
            CustomEditorUtility.ShowProgress("Batching", "Adjust Material & Texture...", index++, m_BatchingDataTable.Count);
            anyChanged |= item.Value.Batching();            
        }

        index = 0;
        foreach (var item in m_ParticleSystemBatchingTable)
        {
            CustomEditorUtility.ShowProgress("Batching", "Adjust ParticleSystem....", index++, m_ParticleSystemBatchingTable.Count);
            BatchingData data;
            if (m_BatchingDataTable.TryGetValue(item.Value, out data))
            {
                ParticleSystem ps = item.Key;
                ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
                if (data.sortIndex != 0)
                {
                    renderer.sortingOrder = data.sortIndex;
                    anyChanged = true;
                }

                if (data.NeedBatching())
                {
                    Texture mainTex = m_ParticleSystemMainTexTable[ps];
                    //因为同一张主贴图可能归属于多个预合批组，但是最后只会合批到一个组里面，所以这里需要判断最终的合批组里面是否包含对应主贴图
                    if (!data.ContainMainTex(mainTex))
                    {
                        continue;
                    }

                    ParticleSystem.TextureSheetAnimationModule sheetAnimModule = ps.textureSheetAnimation;
                    sheetAnimModule.enabled = true;
                    sheetAnimModule.mode = ParticleSystemAnimationMode.Sprites;
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(mainTex));
                    sheetAnimModule.SetSprite(0, sprite);
                    sheetAnimModule.uvChannelMask = UVChannelFlags.UV0;

                    renderer.sharedMaterial = data.baseMat;
                    EditorUtility.SetDirty(ps.gameObject);
                    anyChanged = true;
                }//needbatching check end
            }
        }//outer if end

        if (anyChanged)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        EditorUtility.ClearProgressBar();
    }

    private void AddTextureUsage(Texture texture)
    {
        if (texture != null && AssetDatabase.IsForeignAsset(texture))
        {
            TextureData data;
            if (!m_TextureUsage.TryGetValue(texture, out data))
            {
                data = new TextureData(texture.width, texture.height, TextureUtil.IsTextureHaveAlpha(texture));
                m_TextureUsage.Add(texture, data);
            }
            data.usageCount++;
        }
    }
   
    private void SaveUsageDataToCsv()
    {
        CustomEditorUtility.ShowProgress("Executing", "Save Batching Data To Csv...", 0, 1);
        IOrderedEnumerable<KeyValuePair<Texture, TextureData>> orderedDatas =  m_TextureUsage.OrderBy(item => item.Value.usageCount);
        string csvMapFilePath = Path.Combine(GetCurrentDirectory(), "TextureUsage.csv");
        using (StreamWriter sw = new StreamWriter(csvMapFilePath))
        {
            string tmpStr;
            int tmpIndex = 0;
            sw.WriteLine("#,Texture,Usage,Width,Height,HaveAlpha");
            foreach (var assetData in orderedDatas)
            {
                TextureData data = assetData.Value;
                tmpStr = string.Format("{0},{1},{2},{3},{4},{5}", tmpIndex, assetData.Key.name, data.usageCount, data.width, data.height, data.haveAlpha);
                //Debug.Log(tmpStr,assetData.Key);
                sw.WriteLine(tmpStr);
                tmpIndex++;
            }
        }
        AssetDatabase.Refresh();        
        EditorUtility.ClearProgressBar();
        EditorUtility.RevealInFinder(csvMapFilePath);
    }

    private void SaveBatchingDataToCsv()
    {
        CustomEditorUtility.ShowProgress("Executing", "Save Batching Data To Csv...", 0, 1);
        int batchedCount = 0;
        string csvMapFilePath = Path.Combine(GetCurrentDirectory(), "BatchingData.csv");
        using (StreamWriter sw = new StreamWriter(csvMapFilePath))
        {
            string tmpStr;
            int tmpIndex = 0;
            sw.WriteLine("#,MainTexture,Width,Height,HaveAlpha,RenderMode,GroupID,BaseMaterial,Shader,UsageCount,Batched");
            foreach (var item in m_BatchingDataTable)
            {
                foreach (var tex in item.Value.mainTexs)
                {
                    if (item.Value.NeedBatching())
                    {
                        batchedCount++;
                    }
                    tmpStr = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", tmpIndex, tex.name, tex.width, tex.height,item.Value.haveAlpha, item.Value.renderMode, item.Key, item.Value.baseMat.name, item.Value.baseMat.shader.name, item.Value.mainTexs.Count, item.Value.NeedBatching());
                    //Debug.Log(tmpStr, tex);
                    sw.WriteLine(tmpStr);
                    tmpIndex++;
                }               
            }
        }//using end
        Debug.Log("Batched texture count:" + batchedCount);
        AssetDatabase.Refresh();       
        EditorUtility.ClearProgressBar();
        EditorUtility.RevealInFinder(csvMapFilePath);
    }

    private string GetCurrentDirectory()
    {        
        MonoScript ms = MonoScript.FromScriptableObject(this);
        string path = AssetDatabase.GetAssetPath(ms);
        return path.Substring(0, path.LastIndexOf('/') + 1);
    }

    public enum PSRenderMode
    {
        Billboard,
        Mesh,
    }

    public class TextureData
    {
        public int usageCount = 0;
        public int width = 0;
        public int height = 0;
        public bool haveAlpha = false;

        public TextureData(int width, int height, bool haveAlpha)
        {
            this.width = width;
            this.height = height;
            this.haveAlpha = haveAlpha;
        }
    }

    public class BatchingData
    {
        public string packTag;
        public Material baseMat;
        public string mainTexName;
        public int sortIndex = 0;
        public bool haveAlpha;               //仅用于输出报告数据及调试
        public PSRenderMode renderMode;      //仅用于输出报告数据及调试
        public HashSet<Texture> mainTexs = new HashSet<Texture>();
        private static readonly int MinBatchingTexNum = 4;

        public BatchingData(int groupID, Material srcBaseMat, string mainTexName, bool haveAlpha, PSRenderMode renderMode)
        {
            this.packTag = "Effect_" + groupID;
            this.baseMat = srcBaseMat;
            this.mainTexName = mainTexName;
            this.haveAlpha = haveAlpha;
            this.renderMode = renderMode;
        }

        public void AddMainTex(Texture tex)
        {
            mainTexs.Add(tex);
        }

        public bool ContainMainTex(Texture tex)
        {
            return mainTexs.Contains(tex);
        }

        public bool NeedBatching()
        {
            return mainTexs.Count >= MinBatchingTexNum;
        }

        public bool CanMaterialAdjustRenderOrder()
        {
            if (!AdjustRenderOrder)
            {
                return false;
            }           
            Shader shader = baseMat.shader;
            if (shader.name.Contains("Additive"))
            {
                return true;
            }

            if (baseMat.HasProperty("_DstBlend") && baseMat.GetFloat("_DstBlend") == (int)BlendMode.One)
            {
                return true;
            }
            return false;
        }

        public bool Batching()
        {
            if (!NeedBatching())
            {
                return false;
            }
            baseMat.SetTexture(mainTexName, null);
            EditorUtility.SetDirty(baseMat);
            foreach (var tex in mainTexs)
            {
                bool changed = false;
                TextureImporter ti = TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)) as TextureImporter;
                changed |= ChangeSetting<TextureImporter,string>(ti, x => x.spritePackingTag,  packTag);
                changed |= ChangeSetting<TextureImporter,TextureImporterType>(ti, x => x.textureType, TextureImporterType.Sprite);
                changed |= ChangeSetting<TextureImporter,SpriteImportMode>(ti, x => x.spriteImportMode, SpriteImportMode.Single);                

                if (IgnoreAlpha)
                {                    
                    changed |= ChangePlatformSetting(ti, "Android");
                    changed |= ChangePlatformSetting(ti, "iPhone");                   
                }

                if (changed)
                {                   
                    ti.SaveAndReimport();
                }               
            }
            return true;
        }

        //压缩格式设置分ASTC-RGB和ASTC-RGBA版本的Unity需要设置为ASTC-RGBA的压缩格式
        private bool ChangePlatformSetting(TextureImporter ti, string platformName)
        {
            bool changed = false;
            TextureImporterPlatformSettings platformSetting = ti.GetPlatformTextureSettings(platformName);
            changed |= ChangeSetting<TextureImporterPlatformSettings, bool>(platformSetting, x => x.overridden, true);
            changed |= ChangeSetting<TextureImporterPlatformSettings, TextureImporterFormat>(platformSetting, x => x.format, TextureImporterFormat.ASTC_8x8);
            changed |= ChangeSetting<TextureImporterPlatformSettings, int>(platformSetting, x => x.compressionQuality, 100);
            
            if (changed)
            {
                ti.SetPlatformTextureSettings(platformSetting);
            }
            return changed;
        }

        private bool ChangeSetting<T,TValue>(T importer, Expression<System.Func<T, TValue>> outExpr, TValue value)
        {
            MemberExpression expr = (MemberExpression)outExpr.Body;
            System.Reflection.PropertyInfo prop = (System.Reflection.PropertyInfo)expr.Member;
            TValue srcValue = (TValue)prop.GetValue(importer, null);
            if (!srcValue.Equals(value))
            {
                prop.SetValue(importer, value, null);
                return true;
            }
            return false;
        }
    }

    public class MaterialPreBatchingData
    {
        public class SubData
        {
            public int groupID;
            //同一种RenderMode的能进行动态合批（前提VertexStreams要一样，模型VBO数据不一样会导致不能合批）
            public PSRenderMode renderMode;
            public bool haveAlpha;
            public Dictionary<Texture, int> mainTexs = new Dictionary<Texture, int>();//value-usageCount           

            public SubData(int groupID, PSRenderMode renderMode, bool haveAlpha)
            {
                this.groupID = groupIndex;
                this.renderMode = renderMode;
                this.haveAlpha = haveAlpha;
            }

            public void AddTexture(Texture tex)
            {
                if (!mainTexs.ContainsKey(tex))
                {
                    mainTexs.Add(tex, 0);
                }
                mainTexs[tex]++;
            }
        }

        public Material baseMat;
        public string mainTexName;
        public List<SubData> subDataList = new List<SubData>();
        public static int groupIndex = 0;
        

        public MaterialPreBatchingData(Material baseMat, string mainTexName)
        {
            this.baseMat = baseMat;
            this.mainTexName = mainTexName;
        }

        public int AddMainTexture(PSRenderMode renderMode, Texture mainTex)
        {
            bool haveAlpha = TextureUtil.IsTextureHaveAlpha(mainTex);
            SubData subData = null;
            for (int i = 0; i < subDataList.Count; i++)
            {
                if (subDataList[i].renderMode == renderMode && (IgnoreAlpha || subDataList[i].haveAlpha == haveAlpha))
                {
                    subData = subDataList[i];
                    break;
                }
            }

            if (subData == null)
            {
                subData = new SubData(groupIndex++, renderMode, haveAlpha);
                subDataList.Add(subData);
            }
            if (mainTex != null)
            {
                subData.AddTexture(mainTex);
            }
            return subData.groupID;
        }

        public bool CanBatching(Material mat)
        {
            if (mat.shader != baseMat.shader)
            {
                return false;
            }
            if (mat.renderQueue != baseMat.renderQueue)
            {
                return false;
            }

            if (!mat.shaderKeywords.SequenceEqual(baseMat.shaderKeywords))
            {
                return false;
            }

            Shader shader = mat.shader;

            for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
            {
                string propertyName = ShaderUtil.GetPropertyName(shader, i);

                bool propEqual = true;
                switch (ShaderUtil.GetPropertyType(shader, i))
                {
                    case ShaderUtil.ShaderPropertyType.Color:
                        propEqual = mat.GetColor(propertyName) == baseMat.GetColor(propertyName);
                        break;
                    case ShaderUtil.ShaderPropertyType.Vector:
                        propEqual = mat.GetVector(propertyName) == baseMat.GetVector(propertyName);
                        break;
                    case ShaderUtil.ShaderPropertyType.Float:
                        propEqual = mat.GetFloat(propertyName) == baseMat.GetFloat(propertyName);
                        break;
                    case ShaderUtil.ShaderPropertyType.Range:
                        propEqual = mat.GetFloat(propertyName) == baseMat.GetFloat(propertyName);
                        break;
                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        //跳过主贴图的判断（合批的条件是除了主贴图不一样之外，其他的属性值需要完全一样）
                        if (propertyName.Equals(mainTexName))
                        {
                            propEqual = true;
                        }
                        else
                        {
                            Texture tex = mat.GetTexture(propertyName);
                            propEqual = tex == baseMat.GetTexture(propertyName);
                        }
                        break;
                    default:
                        break;
                }
                if (!propEqual)
                {
                    return false;
                }
            }//Property for end
            return true;
        }
    }

    public class TextureUtil
    {
        public static Texture GetMainTexture(Material mat)
        {
            string mainTexName = GetMainTextureName(mat);
            if (!string.IsNullOrEmpty(mainTexName))
            {
                return mat.GetTexture(mainTexName);
            }
            return null;

        }

        public static string GetMainTextureName(Material mat)
        {
            if (mat.HasProperty("_MainTex"))
            {
                return "_MainTex";
            }
            else if (mat.HasProperty("_BaseMap"))
            {
                return "_BaseMap";
            }
            else
            {
                for (int i = 0; i < ShaderUtil.GetPropertyCount(mat.shader); i++)
                {
                    if (ShaderUtil.GetPropertyType(mat.shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                    {
                        return ShaderUtil.GetPropertyName(mat.shader, i);
                    }
                }
            }
            return null;
        }

        public static bool IsTextureHaveAlpha(Texture tex)
        {
            TextureImporter ti = TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)) as TextureImporter;
            bool haveAlpha = (ti.DoesSourceTextureHaveAlpha() && ti.alphaSource == TextureImporterAlphaSource.FromInput) || ti.alphaSource == TextureImporterAlphaSource.FromGrayScale;
            return haveAlpha;
        }       
    }

}





