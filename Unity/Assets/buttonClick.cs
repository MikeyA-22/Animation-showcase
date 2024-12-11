using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonClick : MonoBehaviour
{
    private Animator animator;
    private Button button;
    public LevelLoader levelLoader;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Transition()
    {
        //animator.SetTrigger("Active");
        //Debug.Log("OnButtonClicked");
        levelLoader.LoadNextLevel();
    }


    public void Quit()
    {
        Application.Quit();
    }
    
}
