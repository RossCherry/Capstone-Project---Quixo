using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DogClick : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { DoClick((PointerEventData)data); }); 

        GameObject parentObject = GameObject.Find("NPCs");
        if (parentObject != null )
        {
            Transform parentTransform = parentObject.transform;

            Transform[] children = new Transform[parentTransform.childCount];

            for (int i = 0; i < parentTransform.childCount; i++)
            {
                children[i] = parentTransform.GetChild(i);
                Button button = children[i].GetComponent<Button>();
                EventTrigger eventTrigger = button.AddComponent<EventTrigger>();
                eventTrigger.triggers.Add(entry);
            }
        }
        else
        {
            Debug.Log("Parent Object Not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoClick(PointerEventData data)
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("isClicked", true);
        Debug.Log("DogClick is being hit");
    }
}
