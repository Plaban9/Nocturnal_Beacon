using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    private List<TextMeshProUGUI> _numberEffects = new List<TextMeshProUGUI>();
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public enum EFFECTS_NUMBER
    {
        DAMAGE,
        HEAL,
        GUARD,
        CORROSION
    }

    [SerializeField] TextMeshProUGUI _numberPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PushNumberEffect(TextMeshProUGUI textEffect)
    {
        _numberEffects.Add(textEffect);
    }

    public void CreateNumber(EFFECTS_NUMBER type, GameObject target, int number)
    {
        TextMeshProUGUI obj;
        Transform parent = target.transform.Find("UnitInterface").transform;
        if (_numberEffects.Count == 0)
        {
            obj = Instantiate<TextMeshProUGUI>(_numberPrefab, parent);
        }
        else
        {
            obj = _numberEffects[0];
            _numberEffects.RemoveAt(0);
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent.transform, false);
            obj.transform.position.Set(0, 0, 0);
            obj.rectTransform.position = new Vector2(0, 0);
            obj.rectTransform.anchoredPosition = new Vector2(0, 0);
        }
        obj.text = number.ToString();
        Animator anim = obj.GetComponent<Animator>();
        string animationToPlay = "NONE";
        switch (type)
        {
            case EFFECTS_NUMBER.DAMAGE:
                animationToPlay = "AnimationNumberDamage";
                break;
            case EFFECTS_NUMBER.HEAL:
                animationToPlay = "AnimationNumberHealing";
                break;
            case EFFECTS_NUMBER.GUARD:
                animationToPlay = "AnimationNumberBlock";
                break;
            case EFFECTS_NUMBER.CORROSION:
                animationToPlay = "AnimationNumberCorrosion";
                break;
        }
        anim.Play(animationToPlay);
    }
}
