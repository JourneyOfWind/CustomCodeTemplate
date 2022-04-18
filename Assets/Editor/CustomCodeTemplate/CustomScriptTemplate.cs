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
    /// �˺�����asset�������꣬�ļ��Ѿ����ɵ������ϣ�����û������.meta�ļ���import֮ǰ������  
    /// </summary>  
    /// <param name="newFileMeta">newfilemeta ���ɴ����ļ���path����.meta��ɵ�</param>  
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


        //���ɵĴ����ļ���λ�� 
        string realPath = Application.dataPath.Replace("Assets", "") + newFilePath;

        //���¶�ȡ����ģ��
        var txtAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(mTemplatePath);
        string scriptContent = txtAsset.text;

        //�Զ����������  
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