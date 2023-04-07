using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public float userSpeed = 1;
    public TMP_Text speedText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) userSpeed /= 2;
        if (Input.GetKeyDown(KeyCode.P)) userSpeed *= 2;
        speedText.text = "Speed: " + userSpeed.ToString();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Time.timeScale = userSpeed;
    }
}
