using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ClickOn : MonoBehaviour
{

    [SerializeField]
    private Material Hover;
    [SerializeField]
    private Material Blank;
    [SerializeField]
    private Material Moves;
    [SerializeField]
    private Material Player1;
    [SerializeField]
    private Material Player2;


    private MeshRenderer myRend;

    [HideInInspector]
    public bool currentlySelected = false;
    [HideInInspector]
    public bool possibleMove = false;
    

    // Start is called before the first frame update
    void Start()
    {
        myRend = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        ClickMe();
    }

    public void ClickMe()
    {
        if(currentlySelected == true || possibleMove == true) 
        {
            if(currentlySelected == true)
            {
                myRend.material = Hover;
            }
            else
            {
                myRend.material = Moves;
            }
        }
        else
        {
            if (gameObject.CompareTag("Player1"))
            {
                myRend.material = Player1;
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (gameObject.CompareTag("Player2"))
            {
                myRend.material = Player2;
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                myRend.material = Blank;
            }
        }

       
    }
    
}
