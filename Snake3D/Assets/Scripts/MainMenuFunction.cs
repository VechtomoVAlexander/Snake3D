using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuFunction : MonoBehaviour
{
    
    void Start()
    {
        
    }

  
    void Update()
    {
        
    }

    public void StartGame()
    {
            SceneManager.LoadScene(1);
    }
    public void Restart()
    {
            SceneManager.LoadScene(1);
    }    
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    } 
        
}
