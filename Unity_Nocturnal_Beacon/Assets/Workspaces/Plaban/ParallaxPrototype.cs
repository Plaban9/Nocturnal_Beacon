using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ParallaxPrototype : MonoBehaviour
{
    [Tooltip("Start from thurthest to the nearest object.")]
    [SerializeField] private GameObject[] _parallaxObjects;
    [SerializeField] private float _mouseSpeedX = 1f, _mouseSpeedY = .2f;
    [SerializeField] private Camera _camera;

    //Paralax effect will be applied as an ofset to the original positions
    private Vector3[] OriginalPositions;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        OriginalPositions = new Vector3[_parallaxObjects.Length];
        for (int i = 0; i < _parallaxObjects.Length; i++)
        {
            OriginalPositions[i] = _parallaxObjects[i].transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x, y;
        x = (Input.mousePosition.x - (Screen.width / 2)) * _mouseSpeedX / Screen.width;
        y = (Input.mousePosition.y - (Screen.height / 2)) * _mouseSpeedY / Screen.height;
        //For each object in ParalaxObjects calculate and applly an offset based on cursor position
        for (int i = 1; i < _parallaxObjects.Length + 1; i++)
        {
            _parallaxObjects[i - 1].transform.position = OriginalPositions[i - 1] + (new Vector3(x, y, 0f) * i * ((i - 1) - (_parallaxObjects.Length / 2)));
        }
    }
}
