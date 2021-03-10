
using Photon.Pun;
using UnityEngine;

namespace HutongGames.PlayMaker.Pun2.Actions
{
    public abstract class PunComponentActionBase<T> : PunActionBase where T : Component
    {
        /// <summary>
        /// The cached GameObject. Call UpdateCache() first
        /// </summary>
        protected GameObject cachedGameObject;

        /// <summary>
        /// The cached component. Call UpdateCache() first
        /// </summary>
        protected T cachedComponent;

        protected PhotonView photonView
        {
            get { return cachedComponent as PhotonView; }
        }

        // Check that the GameObject is the same
        // and that we have a component reference cached
        protected bool UpdateCache(GameObject go,bool searchParent = false)
        {
            if (go == null) return false;

            if (cachedComponent == null || cachedGameObject != go)
            {
                cachedComponent = go.GetComponent<T>();
                if (cachedComponent == null && searchParent)
                {
                    cachedComponent = go.GetComponentInParent<T>();
                }
                
                cachedGameObject = go;

                if (cachedComponent == null)
                {
                    LogWarning("Missing component: " + typeof(T).FullName + " on: " + go.name + " (searched Parents: "+searchParent+")");
                }
            }

            return cachedComponent != null;
        }

    }
}