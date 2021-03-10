// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using UnityEditor;
using UnityEngine;
using HutongGames.PlayMakerEditor;
using Photon.Pun;
using Photon.Realtime;

#pragma warning disable 0414


namespace HutongGames.PlayMaker.Pun2.Editor
{
    [InitializeOnLoad]
    public class PlayMakerPhotonWizard : PhotonEditor
    {
        const string PlayMakerPhotonMenuRoot = "PlayMaker/Addons/Photon Networking/";

        private static float _topSpace = 15f;


        private static string PlaymakerHelpUrl = "http://hutonggames.fogbugz.com/default.asp?W928";
        private static string PlaymakerForumUrl = "http://hutonggames.com/playmakerforum/index.php?board=14.0";


        private static string PhotonSetupHelpUrl = "http://hutonggames.fogbugz.com/default.asp?W982";

        private static string PhotonGOPhotonProxyHelpUrl = "http://hutonggames.fogbugz.com/default.asp?W991";
        private static string PhotonGOFsmSetupHelpUrl = "http://hutonggames.fogbugz.com/default.asp?W989";
        private static string PhotonGOPhotonViewSetupHelpUrl = "http://hutonggames.fogbugz.com/default.asp?W989";


        private static bool userHasSeenWizardIntro = false;

        // set the new, more specific type for the window
        static PlayMakerPhotonWizard()
        {
            WindowType = typeof(PlayMakerPhotonWizard);
          //  CustomToken = "hwVXOE5Sh/LUGC74f2oZ8yX5CDCZFD/V8VX6KArZzbgWn5xVAH55lA==";
            
#if UNITY_2018_1_OR_NEWER
            EditorApplication.hierarchyChanged += EditorRefresh;
#else
            EditorApplication.hierarchyWindowChanged += EditorRefresh;
#endif

#if UNITY_2017_2_OR_NEWER
            EditorApplication.playModeStateChanged += HandleOnPlayModeStateChanged;
#else
            EditorApplication.playmodeStateChanged += EditorRefresh;
#endif
        }

#if UNITY_2017_2_OR_NEWER
        static void HandleOnPlayModeStateChanged(PlayModeStateChange obj)
        {   
            EditorRefresh();
        }
#endif

        private static bool shouldRefresh;

        private static void EditorRefresh()
        {
            shouldRefresh = true;
        }

        [MenuItem(PlayMakerPhotonMenuRoot, false)]
        protected static void photonMenuTemp()
        {
        }

        [MenuItem(PlayMakerPhotonMenuRoot, true)]
        protected static bool photonMenuValidator()
        {
            return true;
        }


#pragma warning disable 0108
        [MenuItem(PlayMakerPhotonMenuRoot + "Set up Photon Networking")]
        public static void Init()
        {
            // custom title in custom, additinal menu entry
            PhotonEditor.CurrentLang.WindowTitle = "Photon Wizard";

            EditorWindow.GetWindow(WindowType, false, PhotonEditor.CurrentLang.WindowTitle);
            //ShowRegistrationWizard();
            userHasSeenWizardIntro = false;
        }


        /*
        protected override void SwitchMenuState(GUIState newState)
        {
            Debug.Log("SwitchMenuState "+newState);
            this.guiState = newState;

        }
        */

        public void OnInspectorUpdate()
        {
            if (shouldRefresh)
            {
                Repaint();
            }
        }

        protected override void OnGUI()
        {
            if (shouldRefresh)
            {
                Repaint();
            }

            FsmEditorStyles.Init();

            FsmEditorGUILayout.ToolWindowLargeTitle(this, "Photon Setup Wizard");
            GUILayout.Space(_topSpace);
#if UNITY_3_5
		EditorGUIUtility.LookLikeControls(200);
#endif

            // check pun version support
            if (!PlayMakerPhotonEditorUtility.IsPunVersionSupported())
            {
                GUI.color = PlayMakerPhotonEditorUtility.lightOrange;
                GUILayout.Label("WARNING: Photon Network version is newer: " + PhotonNetwork.PunVersion + ". We only support version " + PlayMakerPhotonEditorUtility.supportedPunVersion + ".\n It is recommended to only use the Photon Network assets provided with PlayMaker, as compatibility issue may be found otherwise.", "box", GUILayout.ExpandWidth(true));
                GUI.color = Color.white;
            }



#if PLAYMAKER_1_9_OR_NEWER

            if (!FsmEditorSettings.UseLegacyNetworking || !FsmEditorSettings.ShowNetworkSync)
            {
                FsmEditorSettings.UseLegacyNetworking = true;
                FsmEditorSettings.ShowNetworkSync = true;
            }

            // doesn't work, we need to check with PLAYMAKER_LEGACY_NETWORK compile flag
            //EditorStartupPrefs.UseLegacyNetworking = GUILayout.Toggle(EditorStartupPrefs.UseLegacyNetworking,"enabled");

            //			#if !PLAYMAKER_LEGACY_NETWORK
            //			GUI.color =  PlayMakerPhotonEditorUtility.lightOrange;
            //			GUILayout.Label("WARNING: PlayMaker needs to have its network UI system enabled","box",GUILayout.ExpandWidth(true));
            //			GUI.color = Color.white;
            //
            //			if (GUILayout.Button(new GUIContent("Enable PlayMaker Network UI", "Displays the network sync property for Fsm Variables")))
            //			{
            //				PlayMakerEditorUtils.MountScriptingDefineSymbolToAllTargets("PLAYMAKER_LEGACY_NETWORK");
            //			}
            //
            //			#endif
#endif

            base.OnGUI();
        }


        void BuildSceneWizard()
        {
            // JFF : Adds the playmaker proxy, and only display that button if the scene doesn't have it already.
            // mabe make it in red or a lot clearer
            GUILayout.BeginHorizontal();
            GUILayout.Label("Scene", EditorStyles.boldLabel, GUILayout.Width(100));

            if (!PlayMakerPhotonEditorUtility.IsSceneSetup())
            {
                GUI.color = PlayMakerPhotonEditorUtility.lightOrange;
                if (GUILayout.Button(new GUIContent("Add Photon System to the scene", "Required for Photon and PlayMaker to work together")))
                {
                    PlayMakerPhotonEditorUtility.DoCreateRpcProxy();
                }
                GUI.color = Color.white;
                if (FsmEditorGUILayout.HelpButton("Required for scenes that will use Photon"))
                {
                    EditorUtility.OpenWithDefaultApp(PhotonGOPhotonProxyHelpUrl);
                }
            }
            else
            {
                GUI.color = Color.green;
                GUILayout.Label("The scene is set up properly.", "box", GUILayout.ExpandWidth(true));
            }
            GUI.color = Color.white;
            GUILayout.EndHorizontal();

            if (!PlayMakerPhotonEditorUtility.IsSceneSetup())
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(100));
                GUILayout.Label("You need to first add the Photon System Proxy Prefab if you want this scene to work for multi users and PlayMaker", "box", GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(22);


            // JFF : Adds a photon view to the selected gameObject, will automatically add the gameObject proxy if required.
            if (Selection.activeGameObject)
            {

                // check that it is not the photon proxy
                PlayMakerPhotonProxy prox = Selection.activeGameObject.GetComponent<PlayMakerPhotonProxy>();

                if (prox != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Photon Proxy", EditorStyles.boldLabel, GUILayout.Width(100));
                    GUILayout.Label("This GameObject must not be edited. It is the bridge between Photon and PlayMaker", "box", GUILayout.ExpandWidth(true));


                    if (FsmEditorGUILayout.HelpButton("Photon Proxy must not be edited"))
                    {
                        EditorUtility.OpenWithDefaultApp(PhotonGOPhotonProxyHelpUrl);
                    }

                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Component", EditorStyles.boldLabel, GUILayout.Width(100));

                    //object[] objects = Selection.GetFiltered(typeof(PlayMakerFSM),SelectionMode.Unfiltered);

                    //PlayMakerFSM fsm = objects[0] as PlayMakerFSM;
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(new GUIContent("Add network-ready FSM to  '" + Selection.activeGameObject.name + "'", "Required for Fsm that will use Photon")))
                    {
                        PlayMakerPhotonEditorUtility.DoCreateFsmWithPhotonView();
                    }

                    if (FsmEditorGUILayout.HelpButton("Required on GameObject that will use Photon network engine"))
                    {
                        EditorUtility.OpenWithDefaultApp(PhotonGOFsmSetupHelpUrl);
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.Space(12);

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(new GUIContent("Add Photon View to '" + Selection.activeGameObject.name + "'", "Required on GameObject that will use Photon")))
                    {
                        PlayMakerPhotonEditorUtility.DoAddPhotonView();
                    }

                    if (FsmEditorGUILayout.HelpButton("Required on GameObject that will be instantiated over the Photon network"))
                    {
                        EditorUtility.OpenWithDefaultApp(PhotonGOPhotonViewSetupHelpUrl);
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();
                }


                GUILayout.Space(22);
            }

        }

        void BuildDocSection()
        {

            // Playmaker photont documentation
            GUILayout.BeginHorizontal();
            GUILayout.Label("PlayMaker Docs", EditorStyles.boldLabel, GUILayout.Width(100));
            GUILayout.BeginVertical();

            if (GUILayout.Button(new GUIContent("Open Online documentation", "Online documentation for PlayMaker and Photon.")))
            {

                EditorUtility.OpenWithDefaultApp(PlaymakerHelpUrl);

            }

            if (GUILayout.Button(new GUIContent("Open PlayMaker Forum", "Online support for Playmaker and Photon.")))
            {
                EditorUtility.OpenWithDefaultApp(PlaymakerForumUrl);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(22);

            // Photon documentation
            GUILayout.BeginHorizontal();
            GUILayout.Label("Photon Docs", EditorStyles.boldLabel, GUILayout.Width(100));
            GUILayout.BeginVertical();
            if (GUILayout.Button(new GUIContent("Open PDF", "Opens the local documentation pdf.")))
            {
                EditorUtility.OpenWithDefaultApp(DocumentationLocation);
            }
            // JFF: rewording
            if (GUILayout.Button(new GUIContent("Open Online documentation", "Online documentation for Photon.")))
            {
                EditorUtility.OpenWithDefaultApp(UrlDevNet);

            }

            /*
                    // JFF: better routine
                    if (PlayMakerPhotonWizard.serverSetting.HostType == ServerSettings.HostingOption.PhotonCloud)
                    {
                        if (GUILayout.Button(new GUIContent("Open Cloud Dashboard", "Review cloud information and statistics.")))
                        {
                            EditorUtility.OpenWithDefaultApp(UrlAccountPage + emailAddress);
                        }
                    }else if (PlayMakerPhotonWizard.serverSetting.HostType == ServerSettings.HostingOption.SelfHosted)
                    {
                         if (GUILayout.Button(new GUIContent("Open Server Dashboard", "Review Server information and statistics.")))
                        {
                            EditorUtility.OpenWithDefaultApp(UrlAccountPage + emailAddress);
                        }
                    }
        */

            if (GUILayout.Button(new GUIContent("Open Forum", "Online support for Photon.")))
            {
                EditorUtility.OpenWithDefaultApp(UrlForum);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(22);
        }



        protected override void UiMainWizard() //OnGuiMainWizard()
        {
            OnBuildPlayMakerMainWizard();
        }

        protected void OnBuildPlayMakerMainWizard()
        {
            //	GUILayout.Space(_topSpace);
            
            // JFF : modified the look, but need more checks then just "notSet", not sure if we need to go that far tho.
            // settings
            GUILayout.BeginHorizontal();
            GUILayout.Label("Settings", EditorStyles.boldLabel, GUILayout.Width(100));

            bool isNotSet = false;// PhotonNetwork.PhotonServerSettings. == ServerSettings.;

            if (!EditorApplication.isPlaying)
            {
                if (isNotSet)
                {

                    GUI.color = PlayMakerPhotonEditorUtility.lightOrange;
                    if (GUILayout.Button(new GUIContent("Setup", "Setup wizard for setting up your own server or the cloud.")))
                    {
                        userHasSeenWizardIntro = true;

                        PhotonEditor.ShowRegistrationWizard();
                        //base.photonSetupState = PhotonEditor.PhotonSetupStates.RegisterForPhotonCloud;
                        //base.isSetupWizard = false;
                        //InitPhotonSetupWindow();

                    }
                    GUI.color = Color.white;



                    //	EditorUtility.DisplayDialog("Warning", "You have not yet run the Photon setup wizard! Your game won't be able to connect. See Windows -> Photon Unity Networking.", "Ok");
                }
                else
                {

                    if (GUILayout.Button(new GUIContent("Setup", "Setup wizard for setting up your own server or the cloud.")))
                    {

                        PhotonEditor.ShowRegistrationWizard();
                        //base.photonSetupState = PhotonEditor.PhotonSetupStates.RegisterForPhotonCloud;
                        //base.isSetupWizard = false;
                        //InitPhotonSetupWindow();
                    }
                    GUI.color = Color.green;
                    GUILayout.Label("Photon server is set up properly.", "box", GUILayout.ExpandWidth(true));
                    GUI.color = Color.white;
                }

                if (FsmEditorGUILayout.HelpButton("Photon must be set up before working on a multi user scene"))
                {
                    EditorUtility.OpenWithDefaultApp(PhotonSetupHelpUrl);
                }

            }
            else
            {
                GUILayout.Label("Game is playing. To edit Photon Settings, stop playing", "box", GUILayout.ExpandWidth(true));
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(12);

            if (!EditorApplication.isPlaying)
            {
                if (!isNotSet)
                {
                    BuildSceneWizard();
                }
                else
                {

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(100));
                    GUILayout.Label("You need to first set up Photon if you want to build a multi user scene", "box", GUILayout.ExpandWidth(true));
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.FlexibleSpace();
            BuildDocSection();
        }


        protected override void UiSetupApp() // OnGuiRegisterCloudApp()
        {
            if (EditorApplication.isPlaying)
            {
                OnBuildPlayMakerMainWizard();
                //OnGuiMainWizard();
            }
            else
            {
                bool isNotSet = false; //TODO: PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.NotSet;

                if (isNotSet)
                {
                    if (userHasSeenWizardIntro)
                    {
                        base.UiSetupApp();
                    }
                    else
                    {
                        //OnGuiMainWizard();
                        OnBuildPlayMakerMainWizard();

                    }
                }
                else
                {

                    GUI.color = Color.green;
                    GUILayout.Label("Photon server is set up properly.", "box", GUILayout.ExpandWidth(true));
                    GUI.color = Color.white;

                    base.UiSetupApp();
                }

            }
        }


    }
}