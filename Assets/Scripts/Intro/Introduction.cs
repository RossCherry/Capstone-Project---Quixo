using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Introduction : MonoBehaviour
{
    //cooltext.com for the title
    public GamePiece catCar;
    public GamePiece dogCar;
    public GameObject title;

    // Start is called before the first frame update
    void Start()
    {
        //catCar.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>() = catColor;
        //dogCar.transform.GetComponent<MeshRenderer>().material = Resources.Load("bumpercar-01-03-body", typeof(Material)) as Material;
        StartCoroutine(StartAnimations());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    IEnumerator StartAnimations()
    {
        float elapsedTime = 0f;
        float speed = .5f;
        Vector3 catStartingPosition = catCar.transform.position;
        Vector3 dogStartingPosition = dogCar.transform.position;

        //movement 1 by cat
        Vector3 targetPosition1 = new Vector3(-5, 0, -2);

        //movement 2 by dog
        Vector3 targetPosition2 = new Vector3(20, 0, -2);

        //movement 3 by both cat and dog crashing into eachother
        Vector3 targetPosition3 = new Vector3(10.5f, 0, -2);
        Vector3 targetPosition4 = new Vector3(11.5f, 0, -2);

        //movement 4 by both cat and dog bumping back and reveil title
        Vector3 targetPosition5 = new Vector3(5, 0, -2);
        Vector3 targetPosition6 = new Vector3(16, 0, -2);

        //movement 1
        while (elapsedTime < 1f)
        {
            catCar.transform.position = Vector3.Lerp(catStartingPosition, targetPosition1, elapsedTime);
            elapsedTime += Time.deltaTime * (speed);
            yield return null;
        }
        catStartingPosition = targetPosition1;

        //movement 2
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            dogCar.transform.position = Vector3.Lerp(dogStartingPosition, targetPosition2, elapsedTime);
            elapsedTime += Time.deltaTime * (speed);
            yield return null;
        }
        dogStartingPosition = targetPosition2;

        //movement 3
        catCar.transform.localEulerAngles = new Vector3(0, 90, 0);
        dogCar.transform.localEulerAngles = new Vector3(0, 270, 0);
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            catCar.transform.position = Vector3.Lerp(catStartingPosition, targetPosition3, elapsedTime);
            dogCar.transform.position = Vector3.Lerp(dogStartingPosition, targetPosition4, elapsedTime);
            elapsedTime += Time.deltaTime * (speed * 5);
            yield return null;
        }
        catStartingPosition = targetPosition3;
        dogStartingPosition = targetPosition4;

        //movement 4
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            catCar.transform.position = Vector3.Lerp(catStartingPosition, targetPosition5, elapsedTime);
            dogCar.transform.position = Vector3.Lerp(dogStartingPosition, targetPosition6, elapsedTime);
            elapsedTime += Time.deltaTime * (speed * 5);
            yield return null;
        }
        
        ShowTitle();

        //1 second after the title is shown it goes to main menu
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("Main Menu");
    }

    void ShowTitle()
    {
        Debug.Log("Show Title");
        title.GetComponent<MeshRenderer>().enabled = true;
    }
}
