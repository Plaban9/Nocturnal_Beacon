using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDev : MonoBehaviour
{
    [SerializeField] HPData _testHpData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            _testHpData.AddShield(30);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _testHpData.RecoverHealth(30);
        }
    }
}
