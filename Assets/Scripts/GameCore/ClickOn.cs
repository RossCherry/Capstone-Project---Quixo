using System.Collections;
using System.Collections.Generic;
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
        myRend = GetComponent<MeshRenderer>();
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
            if (myRend.CompareTag("Player1"))
            {
                myRend.material = Player1;
            }
            else if (myRend.CompareTag("Player2"))
            {
                myRend.material = Player2;
            }
            else
            {
                myRend.material = Blank;
            }
        }

       
    }
    
}
