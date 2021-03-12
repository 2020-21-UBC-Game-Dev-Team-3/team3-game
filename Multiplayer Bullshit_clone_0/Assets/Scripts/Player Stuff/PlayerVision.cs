using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerVision : MonoBehaviour {
/*    public struct CurrentFadeDistance
    {
        public float currFadeInDistance;
        public float currFadeOutDistance;
        public CurrentFadeDistance(float currFadeInDistance, float currFadeOutDistance)
        {
            this.currFadeInDistance = currFadeInDistance;
            this.currFadeOutDistance = currFadeOutDistance;
        }
    }*/

    private PhotonView pv;

    [SerializeField] private float visionRadius; /*
    [SerializeField] private float distanceForNextFadeIn;
    [SerializeField] private float distanceForNextFadeOut;
    [SerializeField] private float fadeInAmount;*/


    //private Dictionary<GameObject, CurrentFadeDistance> playersInCollider = new Dictionary<GameObject, CurrentFadeDistance>();

    private void Awake() {
        pv = GetComponent<PhotonView>();
    }

    void Start() {
        GetComponent<SphereCollider>().radius = visionRadius;
        if (pv.IsMine) EnableChildren(gameObject);
    }

    private void Update()
    {
        //CheckPlayersInCollider();
    }
/*    void CheckPlayersInCollider()
    {
        foreach(KeyValuePair<GameObject, CurrentFadeDistance> currPlayer in playersInCollider)
        {
            GameObject player = currPlayer.Key;
            if (Mathf.Abs(player.transform.position.x - transform.position.x) <= Mathf.Abs(currPlayer.Value.currFadeInDistance - transform.position.x))
            {
                Renderer[] playerMaterials = player.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in playerMaterials)
                {
                    Color color = renderer.material.color;
                    color.a = color.a + fadeInAmount;
                    renderer.material.color = color;
                }
                
                playersInCollider[player] = new CurrentFadeDistance(Mathf.Abs(player.transform.position.x - distanceForNextFadeIn), Mathf.Abs(player.transform.position.x - distanceForNextFadeOut));
            } else if (Mathf.Abs(player.transform.position.x - transform.position.x) >= Mathf.Abs(currPlayer.Value.currFadeOutDistance - transform.position.x))
            {
                Renderer[] playerMaterials = player.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in playerMaterials)
                {
                    Color color = renderer.material.color;
                    color.a = color.a - fadeInAmount;
                    renderer.material.color = color;
                }
                playersInCollider[player] = new CurrentFadeDistance(Mathf.Abs(player.transform.position.x + distanceForNextFadeIn), Mathf.Abs(player.transform.position.x + distanceForNextFadeOut));
            }
        }
    }

    void EnablePlayer(GameObject c)
    {
        Renderer[] playerMaterials = c.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in playerMaterials)
        {
            Color color = renderer.material.color;
            color.a = color.a + fadeInAmount;
            renderer.material.color = color;
        }
        playersInCollider.Add(c, new CurrentFadeDistance(Mathf.Abs(c.transform.position.x - distanceForNextFadeIn) , Mathf.Abs(c.transform.position.x - distanceForNextFadeOut)));
    }

    void DisablePlayer(GameObject c)
    {
        Renderer[] playerMaterials = c.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in playerMaterials)
        {
            Color color = renderer.material.color;
            color.a = color.a - fadeInAmount;
            renderer.material.color = color;
        }
        playersInCollider.Remove(c);

    }*/

    void EnableChildren(GameObject c)
    {
        c.transform.Find("Meshes").gameObject.SetActive(true);
        c.transform.Find("NameCanvas").gameObject.SetActive(true);
    }

    void DisableChildren(GameObject c)
    {
        c.transform.Find("Meshes").gameObject.SetActive(false);
        c.transform.Find("NameCanvas").gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider) {
        if (pv.IsMine && collider.gameObject != gameObject && collider.gameObject.tag == "Player") EnableChildren(collider.gameObject);
    }

    private void OnTriggerExit(Collider collider) {
        if (pv.IsMine && collider.gameObject != gameObject && collider.gameObject.tag == "Player") DisableChildren(collider.gameObject);
    }
}
