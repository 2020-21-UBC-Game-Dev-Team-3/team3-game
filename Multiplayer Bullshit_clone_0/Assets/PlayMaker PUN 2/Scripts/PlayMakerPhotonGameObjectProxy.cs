// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using UnityEngine;


#pragma warning disable 0414

namespace HutongGames.PlayMaker.Pun2
{
    /// <summary>
    /// This component is required on gameObjects and prefabs you are planning to instanciate over the Photon network.
    /// 
    /// It receives the OnPhotonInstantiate message when instanciated and forward it as an Event for Fsm component attached this gameObject and all its childrens.
    /// 
    /// It also verifies the proper setup for fsm components on that gameObject that have networked synched variables:
    /// *It assumes like for the Unity networking that you have a PhotonView observing the fsm.
    /// *I insert at runtime a bridge ( PlayMakerPhotonView ) that goes inbetween the fsm and the photonView. This is required because the fsmComponent doesn't implement photon networking nativly
    /// ( that is not implementing OnPhotonInstantiate() nor OnPhotonSerializeView
    /// It can be set up manually of course, else Iwill do it for the user at runtime when the gameObject is instanciated. 
    /// Note: for gameObject sitting in the hierarchy when starting, the check is also happening but it's directly call within the "PlayMaker photon proxy" mandatory prefab
    /// </summary>
    public class PlayMakerPhotonGameObjectProxy : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {

        const string DebugLabelPrefix = "<color=navy>PlayMaker Photon GameObject proxy: </color>";
        
        /// <summary>
        /// output in the console activities of the various elements.
        /// Is taken form the debug value of the "PlayMaker Photon Proxy"
        /// </summary>
        bool debug = true;
        bool LogMessageInfo = true;

        private PhotonMessageInfo _lastCallbackInfo;
        
        [ContextMenu("Help")]
        public void help()
        {
            Application.OpenURL("https://hutonggames.fogbugz.com/default.asp?W990");
        }


        // get the Playmaker Photon proxy fsm.
        void Awake()
        {


            Debug.Log("Player awake",this);

            // get the photon proxy for Photon Fsm Proxy to send event.
            GameObject go = GameObject.Find("PlayMaker Photon Proxy");

            if (go == null)
            {
                Debug.LogError("Working with photon network require that you add a 'PlayMaker Photon Proxy' component to the gameObject. You can do so from the menu 'PlayMaker Photon/components/Add photon proxy to scene'");
                return;
            }


            //// get the proxy to set the debug flag.
            PlayMakerPhotonProxy _proxy = go.GetComponent<PlayMakerPhotonProxy>();
            if (_proxy!=null)
            {
            	debug = _proxy.debug;
            	LogMessageInfo = _proxy.LogMessageInfo;
            }

            _proxy.SanitizeGameObject(this.gameObject);

        }// Awake

        #region Photon Messages

        /// <summary>
        /// compose this message to dispatch the associated global Fsm Event. 
        /// TOFIX: The problem is, It's called before Fsm are instanciated, so I had to implement a slight delay...
        /// </summary>
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            _lastCallbackInfo = info;
            
            if (debug)
            {
                if (!info.Equals(new PhotonMessageInfo()))
                {
                    if (info.Sender != null)
                    {
                        Debug.Log("PlayMaker Photon proxy:OnPhotonInstantiate " + info.Sender.NickName);
                    }
                    else
                    {
                        Debug.Log("PlayMaker Photon proxy:OnPhotonInstantiate with no sender info");
                    }
                }
                else
                {
                    Debug.Log("PlayMaker Photon proxy:OnPhotonInstantiate with no PhotonMessageInfo");
                }
            }

            // TOFIX: if we found a better solution, I am all up for it... How stable this can be if framerate is low for example?
            Invoke("sendPhotonInstantiationFsmEvent", 0.1f);


        }// OnPhotonInstantiate


        /// <summary>
        /// Sends the photon instantiation fsm event to ALL Fsm of the instantiated gameObject AND its children.
        /// </summary>
        void sendPhotonInstantiationFsmEvent()
        {
            string _fsmEvent = PlayMakerPunLUT.CallbacksEvents[PunCallbacks.OnPhotonInstantiate];

            if (debug)
            {
                Debug.Log("sending "+_fsmEvent+" event to " + this.gameObject.name);
            }
            
            // set the target to be this gameObject.
            FsmOwnerDefault goTarget = new FsmOwnerDefault();
            goTarget.GameObject = new FsmGameObject();
            goTarget.GameObject.Value = this.gameObject;
            goTarget.OwnerOption = OwnerDefaultOption.SpecifyGameObject;

            // send the event to this gameObject and all its children
            FsmEventTarget eventTarget = new FsmEventTarget();
            eventTarget.excludeSelf = false;
            eventTarget.target = FsmEventTarget.EventTarget.GameObject;
            eventTarget.gameObject = goTarget;
            eventTarget.sendToChildren = true;

            
            // create the event.
            FsmEvent fsmEvent = new FsmEvent(_fsmEvent);

            string _data = "<color=darkblue>" + "ActorNumber" + "</color>=<color=<darkblue>" + _lastCallbackInfo.Sender.ActorNumber + "</color> ";
            
            _data += "<color=darkblue>" + "NickName" + "</color>=<color=<darkblue>" + _lastCallbackInfo.Sender.NickName + "</color> ";
         
            
            Debug.Log( "PlayMakerPhotonGameObjectProxy Received Callback <color=fuchsia>" + PunCallbacks.OnPhotonInstantiate + "</color> " +
                      "Broadcasting global Event <color=fuchsia>" + fsmEvent.Name  + "</color>\n"+
                      _data
                , this);
            
            FsmEventData _d = new FsmEventData();
            _d.StringData = _lastCallbackInfo.Sender.NickName;
            _d.IntData = _lastCallbackInfo.Sender.ActorNumber;
            PlayMakerUtils.SendEventToTarget(null,eventTarget, fsmEvent.Name,_d); 

        }// sendPhotonInstantiationFsmEvent

        #endregion




    }
}