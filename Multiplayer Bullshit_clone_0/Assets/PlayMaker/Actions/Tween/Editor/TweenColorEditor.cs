// (c) Copyright HutongGames, all rights reserved.
// See also: EasingFunctionLicense.txt

using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker.TweenEnums;
using UnityEditor;

namespace HutongGames.PlayMakerEditor
{
    [CustomActionEditor(typeof(PlayMaker.Actions.TweenColor))]
    public class TweenColorEditor : TweenPropertyEditor<FsmColor>
    {
        public override bool OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditField("target");

            var tweenColor = (TweenColor) target;

            switch (tweenColor.target)
            {
                case TweenColor.Target.GameObject:
                    EditField("gameObject");
                    break;
                case TweenColor.Target.Variable:
                    EditField("variable");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EditField("fromOption");
            DoTargetValueGUI(tweenAction.fromOption, "fromValue");
            if (tweenAction.fromOption == TargetValueOptions.Offset)
            {
                EditField("fromOffsetBlendMode","Blend Mode");
            }

            EditField("toOption");
            DoTargetValueGUI(tweenAction.toOption, "toValue");
            if (tweenAction.fromOption == TargetValueOptions.Offset)
            {
                EditField("toOffsetBlendMode","Blend Mode");
            }

            DoEasingUI();

            return EditorGUI.EndChangeCheck();
        }


    }
}