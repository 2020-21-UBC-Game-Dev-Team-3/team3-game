// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;

using HutongGames.PlayMakerEditor;

namespace HutongGames.PlayMaker.Pun2.Actions
{
    [PropertyDrawer(typeof(PunActionHeader))]
    public class PunActionHeaderPropertyDrawer : PropertyDrawer
    {

        Rect _rect;

        Color _punBlue = Color.black;

        public override object OnGUI(GUIContent label, object obj, bool isSceneObject, params object[] attributes)
        {
            // always keep thsi enabled to avoid transparency artifact ( unless someone tells me how to style up GUIStyle for disable state)
            bool _enabled = GUI.enabled;
            GUI.enabled = true;


            if (_punBlue == Color.black)
            {
                ColorUtility.TryParseHtmlString("#15508AFF", out _punBlue);
            }
            _rect = GUILayoutUtility.GetLastRect();
            GUIDrawRect(_rect, _punBlue);

            _rect.Set(_rect.x, _rect.y + 1, _rect.width, _rect.height - 2);
            GUI.DrawTexture(_rect, PunHeader, ScaleMode.ScaleToFit);


            GUI.enabled = _enabled;

            return null;
        }


        private static Texture2D sPunHeader = null;
        internal static Texture2D PunHeader
        {
            get
            {
                if (sPunHeader == null)
                    sPunHeader = Resources.Load<Texture2D>("Pun_playmaker_action_header");
                ;
                if (sPunHeader != null)
                    sPunHeader.hideFlags = HideFlags.DontSaveInEditor;
                return sPunHeader;
            }
        }

        private static Texture2D _staticRectTexture;
        private static GUIStyle _staticRectStyle;

        // Note that this function is only meant to be called from OnGUI() functions.
        public static void GUIDrawRect(Rect position, Color color)
        {
            if (_staticRectTexture == null)
            {
                _staticRectTexture = new Texture2D(1, 1);
            }

            if (_staticRectStyle == null)
            {
                _staticRectStyle = new GUIStyle();
            }

            _staticRectTexture.SetPixel(0, 0, color);
            _staticRectTexture.Apply();

            _staticRectStyle.normal.background = _staticRectTexture;

            GUI.Box(position, GUIContent.none, _staticRectStyle);


        }


    }
}