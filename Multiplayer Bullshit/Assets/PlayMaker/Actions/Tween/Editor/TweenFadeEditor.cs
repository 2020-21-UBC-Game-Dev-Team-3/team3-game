// (c) Copyright HutongGames, all rights reserved.
// See also: EasingFunctionLicense.txt

using HutongGames.PlayMaker.Actions;
using UnityEditor;
using UnityEngine;

namespace HutongGames.PlayMakerEditor
{
    [CustomActionEditor(typeof(PlayMaker.Actions.TweenFade))]
    public class TweenFadeEditor : TweenEditorBase
    {
        public override bool OnGUI()
        {
            var tweenFade = target as TweenFade;

            EditorGUI.BeginChangeCheck();

            EditField("gameObject");

            FsmEditorGUILayout.ReadonlyTextField("Fade Type: " + tweenFade.type);

            EditField("tweenDirection", "Fade");
            EditField("value");

            DoEasingUI();

            return EditorGUI.EndChangeCheck();
        }


    }
}