// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;
using UnityEditor;
using HutongGames.PlayMakerEditor;

namespace HutongGames.PlayMaker.Pun2.Actions
{
    [PropertyDrawer(typeof(PlayerReferenceProperty))]
    public class PlayerReferencePropertyDrawer : PlayMakerEditor.PropertyDrawer
    {
        public override object OnGUI(GUIContent label, object obj, bool isSceneObject, params object[] attributes)
        {
            PlayerReferenceProperty _class = obj as PlayerReferenceProperty;
            if (_class == null)
            {
                EditorGUILayout.HelpBox("PlayerReferenceProperty = null", MessageType.Error);
                return null;
            }

            EditorGUI.indentLevel++;

            EditField("reference", _class.reference, attributes);

            EditorGUI.indentLevel++;
  

            PlayerReferenceProperty.PlayerReferences _ref = _class.reference;

            if (_ref == PlayerReferenceProperty.PlayerReferences.ByUserId)
            {
                EditField("userId", _class.userId, attributes);
            }
            
            if (_ref == PlayerReferenceProperty.PlayerReferences.ByActorNumber)
            {
                EditField("actorNumber", _class.actorNumber, attributes);
            }

            if (_ref == PlayerReferenceProperty.PlayerReferences.ByNickName)
            {
                EditField("nickname", _class.nickname, attributes);
            }

            if (_ref == PlayerReferenceProperty.PlayerReferences.ByOwnedObject)
            {
                EditField("gameObject", _class.gameObject, attributes);
            }

            if (_ref == PlayerReferenceProperty.PlayerReferences.ByRoomNumber)
            {
                EditField("roomNumber", _class.roomNumber, attributes);
            }

          
            EditorGUI.indentLevel--;


            EditField("playerNotFound", _class.playerNotFound, attributes);

            EditorGUI.indentLevel--;

            return obj;
        }
    }
}