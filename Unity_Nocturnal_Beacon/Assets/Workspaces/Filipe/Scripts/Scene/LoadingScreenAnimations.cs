using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenAnimations : MonoBehaviour
{
    // Start is called before the first frame update

    public static LoadingScreenAnimations Instance { get; private set; }

    [SerializeField] float radius;
    [SerializeField] float fallOff;
    [SerializeField] GameObject target;
    [SerializeField] Transform umbrellaHolder;

    Vector3 _targetLastPos = Vector3.zero;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);

        InitializeChildrenStatuses();
        _targetLastPos = target.transform.position;
    }

    private void InitializeChildrenStatuses()
    {
        foreach (Transform child in umbrellaHolder)
        {
            if (child.TryGetComponent<Image>(out Image img))
            {
                img.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                float sizeVar = Random.Range(-0.2f, 0.2f);
                child.localScale = new Vector3(1.8f + sizeVar, 1.8f + sizeVar, 1.8f + sizeVar);
            }
            if (child.TryGetComponent<Animator>(out Animator anim))
            {
                anim.speed = Random.Range(0f, 1.0f);
                anim.SetBool("reverse", Random.Range(0f, 1f) >= 0.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetLastPos == target.transform.position) return;
        foreach (Transform child in umbrellaHolder)
        {
            var value = Distance(child.gameObject, target);
            var multiplier = 0f;
            if (value < radius)
            {
                multiplier = 1.8f;
            }
            else
            {
                multiplier =
                    Mathf.Lerp(
                        1.8f,
                        0f,
                        Mathf.Clamp(
                            value / fallOff,
                            0f,
                            1f)
                        );
            }
            value = Mathf.Lerp(1.8f, 0f, value);
            child.localScale = new Vector3(multiplier, multiplier, 0f);
        }
        _targetLastPos = target.transform.position;
    }


    float Distance(GameObject go1, GameObject go2)
    {
        var value = Vector3.Distance(go1.transform.position, go2.transform.position);
        return value;
    }

    public void ToLoading()
    {
        if(TryGetComponent<Animator>(out var anim))
        {
            anim.Play("ToLoading");
        }
    }

    public void DoneLoading()
    {
        Debug.Log("Aie!");
        if (TryGetComponent<Animator>(out var anim))
        {
            anim.Play("DoneLoading");
        }
    }
}
