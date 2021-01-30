using UnityEngine;

public class VotingButtonInteract : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
           !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (gameObject.transform.GetChild(0).gameObject.activeInHierarchy) gameObject.transform.GetChild(0).transform.gameObject.SetActive(false);
        }

    }
}
