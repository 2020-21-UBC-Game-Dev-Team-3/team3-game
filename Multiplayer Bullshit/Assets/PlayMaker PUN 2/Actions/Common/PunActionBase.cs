// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

#if UNITY_EDITOR

using UnityEditor;
#endif

using UnityEngine;

namespace HutongGames.PlayMaker.Pun2.Actions
{
    // Base class for cinemachine actions.
    public abstract class PunActionBase : FsmStateAction
    {

#if UNITY_EDITOR && PLAYMAKER_1_9_OR_NEWER

        [DisplayOrder(0)]
        [HideIf("HideActionHeader")]
        public PunActionHeader header;

        const string HideActionHeaderPrefsKey = "PlayMaker.ecosystem.pun2.HideActionHeader";

		public override void InitEditor (Fsm fsmOwner)
		{
            PunActionHeader.HideActionHeader = EditorPrefs.GetBool(HideActionHeaderPrefsKey, false);

        }

        public bool HideActionHeader()
        {
            return PunActionHeader.HideActionHeader;
        }

        [SettingsMenuItem("Hide Pun Header")]
        public static void ToggleActionHeader()
        {
            PunActionHeader.HideActionHeader = !PunActionHeader.HideActionHeader;
            EditorPrefs.SetBool(HideActionHeaderPrefsKey, PunActionHeader.HideActionHeader);
        }

        [SettingsMenuItemState("Hide Pun Header")]
        public static bool ToggleActionHeaderState()
        {
            return PunActionHeader.HideActionHeader;
        }


#endif

    }
}