using UnityEditor;
using HutongGames.PlayMakerEditor;
using UnityEngine;

namespace HutongGames.PlayMaker.Pun2.Editor
{
    [InitializeOnLoad]
    public class PlayMakerPhotonEditorSetup
    {
        static PlayMakerPhotonEditorSetup()
        {
#if PLAYMAKER_1_9_OR_NEWER
            FsmEditorSettings.ShowNetworkSync = true;
#endif

#if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                CreateGlobalEventIfNecessary();
            }
#endif
        }

        public static void CreateGlobalEventIfNecessary()
        {
         //   Debug.Log("CreateGlobalEventIfNecessary");
            PlayMakerEditorUtils.CreateGlobalEventsIfNeeded(PlayMakerPunLUT.PhotonEvents.ToArray());
            
        }
    }
}