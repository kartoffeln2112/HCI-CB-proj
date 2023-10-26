using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class selectscene : MonoBehaviour
{
    public Button style, real;
    // Start is called before the first frame update
    void Start()
    {
        style.onClick.AddListener(() => loadScene("style-apt"));
        real.onClick.AddListener(() => loadScene("real-apt"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
