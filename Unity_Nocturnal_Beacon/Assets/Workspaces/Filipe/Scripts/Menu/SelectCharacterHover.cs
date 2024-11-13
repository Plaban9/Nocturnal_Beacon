using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCharacterHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update

    [SerializeField] public PlayableData _unitData;


    [SerializeField] public TextMeshProUGUI _name;
    [SerializeField] public TextMeshProUGUI _description;
    [SerializeField] public Image _sprite;


    Animator _animator;

    public void OnPointerClick(PointerEventData eventData)
    {
        _animator.SetTrigger("selected");
        StartCoroutine(StartGame()); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.SetBool("hovering", false);

    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _animator.SetBool("hovering", true); 
    }


    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.6f); 
        PlayerPrefs.DeleteKey("Map");
        PlayerUnitData newGameData = new PlayerUnitData();
        newGameData.Setup(_unitData, 30); 
        NoctBeaconRunData.Instance.SetPlayer(newGameData);
        SceneController.Instance.ToMap();
    }


    void Start()
    {
        _animator = GetComponent<Animator>();
        _name.text = _unitData.unitName;
        _description.text = _unitData.description;
        _sprite.sprite = _unitData.sprite;
        if (!_unitData.flipSprite)
            _sprite.transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
