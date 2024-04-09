using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class CategoryTransition : MonoBehaviour
{
    [SerializeField] GameObject PagesParent;
    GameObject[] _pages;
    List<Animator> _animators;
    int current;

    // Start is called before the first frame update
    void Start()
    {
        _animators = new List<Animator>();

        _pages = GameObject.FindGameObjectsWithTag("ConfigPage");

        // make a list of animators of pages
        foreach(GameObject page in _pages){
            if (page.transform.parent == PagesParent.transform)
            {
                _animators.Add(page.GetComponent<Animator>());
            }
        }

        current = -1;
        transition(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void transition(int pageIdToAppear){
        if (pageIdToAppear != current)
        {
            for(int i = 0; i < _animators.Count; i++){
                if (i == pageIdToAppear)
                {
                    _animators[i].SetTrigger("Appear");
                }else if (i == current)
                {
                    _animators[current].SetTrigger("Disappear");
                }
            }

            current = pageIdToAppear;
        }
    }
}
