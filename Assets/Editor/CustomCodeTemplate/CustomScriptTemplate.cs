using System.IO;
using UnityEditor;
using UnityEngine;
public class CustomScriptTemplate : UnityEditor.AssetModificationProcessor
{
    private static string mCodeConfigPath =
        "Assets/Editor/CustomCodeTemplate/CodeConfig.asset";

    private static string mTemplatePath = "Assets/Editor/CustomCodeTemplate/CodeTemplate.txt";

    private static CodeConfig mCodeConfig;
    /// <summary>  
    /// 此函数在asset被创建完，文件已经生成到磁盘上，但是没有生成.meta文件和import之前被调用  
    /// </summary>  
    /// <param name="newFileMeta">newfilemeta 是由创建文件的path加上.meta组成的</param>  
    public static void OnWillCreateAsset(string newFileMeta)
    {
        string newFilePath = newFileMeta.Replace(".meta", "");
        string fileExt     = Path.GetExtension(newFilePath);
        if (fileExt != ".cs") {
            return;
        }
        if (mCodeConfig == null) {
            mCodeConfig = AssetDatabase.LoadAssetAtPath<CodeConfig>(mCodeConfigPath);
        }


        //生成的代码文件的位置 
        string realPath = Application.dataPath.Replace("Assets", "") + newFilePath;

        //重新读取代码模板
        var txtAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(mTemplatePath);
        string scriptContent = txtAsset.text;

        //自定义代码内容  
        scriptContent = scriptContent.Replace("#SCRIPTNAME#",   Path.GetFileNameWithoutExtension(newFilePath));
        scriptContent = scriptContent.Replace("#Author#",       mCodeConfig.AuthorName);
        scriptContent = scriptContent.Replace("#Version#",      mCodeConfig.Version);
        scriptContent = scriptContent.Replace("#UnityVersion#", Application.unityVersion);
        scriptContent = scriptContent.Replace("#CreateTime#",   System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
        scriptContent = scriptContent.Replace("#Description#",  mCodeConfig.GetSingleWords());

        EditorUtility.SetDirty(mCodeConfig);
        File.WriteAllText(realPath, scriptContent);
    }
}