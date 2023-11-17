﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    
    public string remakeSceneName;
    public Vector3 remakePoint;
    
    public GameObject cat;
    public GameObject dog;
    private void Start()
    {
        DontDestroyOnLoad(this);
        FindCatAndDog();
    }

    private void OnEnable()
    {
        // 订阅事件, 在场景加载的时候尝试去找猫和狗部件
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    // 
    private void OnSceneChanged(Scene scene, LoadSceneMode loadSceneMode)
    {
        FindCatAndDog();  
    }
    
    public void FindCatAndDog()
    {
        cat = GameObject.Find("Cat");
        dog = GameObject.Find("Dog");
    }
    public void Dead()
    {
        Debug.Log("死了");
        cat.gameObject.SetActive(false);
        dog.gameObject.SetActive(false);
   
        StartCoroutine(WaitToRemake());
    }
    //计时几秒后跳转至复活点
    IEnumerator WaitToRemake()
    {
        // Debug.Log("进入了携程");
        // yield return new WaitForSeconds(3f); 
        if (SceneManager.GetActiveScene().name!=remakeSceneName)
        {
            LoadSceneManager.Instance.LoadScene(remakeSceneName);
             // yield return new WaitForSeconds(0.001f); //Wait期间会自动调用Update函数 这里直接写Instance.FindCatAndDog()是找不到猫狗的 
                cat.transform.position = remakePoint;
                dog.transform.position = remakePoint;
        }
        else
        {
            cat.GetComponent<Damageable>().IsAlive = true;
            dog.GetComponent<Damageable>().IsAlive = true;

            cat.gameObject.SetActive(true);
            dog.gameObject.SetActive(true);
            cat.transform.position = remakePoint;
            dog.transform.position = remakePoint;
        }
        yield break;
    }
}
