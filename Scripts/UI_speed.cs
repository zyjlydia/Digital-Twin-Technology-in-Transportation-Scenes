using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_speed : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speedText.text = playerController.speed.ToString() + " KM/H";
    }
}
