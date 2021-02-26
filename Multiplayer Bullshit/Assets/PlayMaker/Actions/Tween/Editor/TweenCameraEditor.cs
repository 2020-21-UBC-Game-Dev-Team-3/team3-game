// (c) Copyright HutongGames, all rights reserved.
// See also: EasingFunctionLicense.txt

using System;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEditor;
using UnityEngine;

namespace HutongGames.PlayMakerEditor
{
    [CustomActionEditor(typeof(PlayMaker.Actions.TweenCamera))]
    public class TweenCameraEditor : TweenEditorBase
    {
        private TweenCamera tweenAction;

        public override void OnEnable()
        {
            base.OnEnable();

            tweenAction = (PlayMaker.Actions.TweenCamera) target;
        }

        public override bool OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditField("gameObject");
            
            DoPropertySelector();

            EditField("tweenDirection", "Tween");

            switch (tweenAction.property)
            {
                case TweenCamera.CameraProperty.BackgroundColor:
                    EditField("targetColor", "Color");
                    break;
                case TweenCamera.CameraProperty.Aspect:
                case TweenCamera.CameraProperty.FieldOfView:
                case TweenCamera.CameraProperty.OrthoSize:
                    EditField("targetFloat", "Value");
                    break;
                case TweenCamera.CameraProperty.PixelRect:
                case TweenCamera.CameraProperty.ViewportRect:
                    EditField("targetRect", "Rect");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DoEasingUI();

            return EditorGUI.EndChangeCheck();
        }

        private void DoPropertySelector()
        {
            EditorGUI.BeginChangeCheck();
            EditField("property");
            if (EditorGUI.EndChangeCheck())
            {
                tweenAction.targetColor = new FsmColor { Value = Color.black };
                tweenAction.targetFloat = new FsmFloat { Value = 0 };
                tweenAction.targetRect = new FsmRect { Value = new Rect(0,0,1,1)};
            }
        }

    }
}