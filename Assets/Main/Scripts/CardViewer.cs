using UnityEngine;

public class CardViewer : MonoBehaviour
{
    public static CardViewer Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);   
    }
 

 
}