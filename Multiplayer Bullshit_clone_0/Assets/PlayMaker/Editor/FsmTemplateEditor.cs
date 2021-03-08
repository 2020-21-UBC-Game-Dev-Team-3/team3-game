using System.ComponentModel;
using HutongGames.PlayMakerEditor;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(FsmTemplate))]
public class FsmTemplateEditor : Editor
{
    private SerializedProperty categoryProperty;
    private SerializedProperty descriptionProperty;
    private GUIStyle multiline;

    private GUIContent findInBrowser;

    [Localizable(false)]
    public void OnEnable()
    {
        categoryProperty = serializedObject.FindProperty("category");
        descriptionProperty = serializedObject.FindProperty("fsm.description");

        findInBrowser = new GUIContent("Find", "Find in Template Browser");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(categoryProperty);

        if (multiline == null)
        {
            multiline = new GUIStyle(EditorStyles.textField) { wordWrap = true };
        }
        descriptionProperty.stringValue = EditorGUILayout.TextArea(descriptionProperty.stringValue, multiline, GUILayout.MinHeight(60));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            FsmTemplateSelector.Refresh();
        }
        GUILayout.BeginHorizontal();
        
        //GUILayout.FlexibleSpace();

        if (GUILayout.Button(findInBrowser))
        {
            FsmTemplateSelector.FindTemplateInBrowser((FsmTemplate) target);
        }

        if (GUILayout.Button(Strings.FsmTemplateEditor_Open_In_Editor))
        {
            FsmEditorWindow.OpenWindow((FsmTemplate) target);
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.HelpBox(Strings.Hint_Exporting_Templates, MessageType.None );
    }
}
