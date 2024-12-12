using CardAttribute;
using Minimalist.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    private List<TextMeshProUGUI> _numberEffects = new List<TextMeshProUGUI>();

    private Dictionary<(Element,EffectTarget), List<GameObject>> _particlePool = new Dictionary<(Element,EffectTarget), List<GameObject>>();

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
                AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.Player_Hit);
                animationToPlay = "AnimationNumberDamage";
                break;
            case EFFECTS_NUMBER.HEAL:
                animationToPlay = "AnimationNumberHealing";
                AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.Companion_DogBark);
                break;
            case EFFECTS_NUMBER.GUARD:
                animationToPlay = "AnimationNumberBlock";
                AudioManager.PlaySFX(Minimalist.Audio.Sound.SoundType.Companion_DogInteract);
                break;
            case EFFECTS_NUMBER.CORROSION:
                animationToPlay = "AnimationNumberCorrosion";
                break;
        }
        anim.Play(animationToPlay);
    }

    public float PlayCardEffect(BattleUnit owner, List<BattleUnit> enemies, Card card, EffectTarget target)
    {
        Vector3 center = new Vector3(0, 0, 0);
        foreach (BattleUnit unit in enemies) {
            center += unit.transform.position;
        }
        GameObject particle = null;

        switch (target){
            case EffectTarget.Self:
                particle = PlayElementalEffect(owner.transform.position,card, target);
                break;
            case EffectTarget.OpponentSingle:
            case EffectTarget.OpponentRandom:
                particle = PlayElementalEffect(center, card,target);
                break;
            case EffectTarget.OpponentAll:
                particle = PlayElementalEffect(center/enemies.Count, card, target);
                break;
            case EffectTarget.Global:
                center += owner.transform.position;
                particle = PlayElementalEffect((center)/(enemies.Count+1), card,target);
                break;
            case EffectTarget.Both:
                PlayElementalEffect(owner.transform.position, card, target);
                particle = PlayElementalEffect(center, card, target);
                break;
        }
        
        if(particle != null) StartCoroutine(StopParticle(particle));
        return -1f;
    }

    private IEnumerator StopParticle(GameObject pr)
    {
        yield return new WaitForSeconds(0.5f);
        pr.GetComponent<ParticleSystem>().Stop();
    }

    public GameObject PlayElementalEffect(Vector3 position, Card card, EffectTarget target){
        Debug.Log($"Attempting to get ({card.element}, {target})...");
        if (!_particlePool.ContainsKey((card.element,target)))
        {
            _particlePool[(card.element, target)] = new List<GameObject>();
            GameObject newEffect = Instantiate(GetPrefabTargets(card.element, target), position, Quaternion.identity, null);
            if (target == EffectTarget.Self || target == EffectTarget.OpponentSingle || target == EffectTarget.OpponentRandom || target == EffectTarget.Both)
            {
                newEffect.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                newEffect.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
            _particlePool[(card.element, target)].Add(newEffect);

            return newEffect;
        }
        else
        {
            if (!_particlePool.ContainsKey((card.element, target))) return null;
            GameObject viableParticle = _particlePool[(card.element, target)].Find(it =>
            {
                if (it == null) return false;
                if (it.TryGetComponent<ParticleSystem>(out ParticleSystem ps))
                {
                    if (!ps.isPlaying)
                    {
                        return true;
                    }
                }
                return false;
            });
            if (viableParticle == null)
            {
                GameObject newEffect = Instantiate(GetPrefabTargets(card.element, target), position, Quaternion.identity, null);
                if (target == EffectTarget.Self || target == EffectTarget.OpponentSingle || target == EffectTarget.OpponentRandom || target == EffectTarget.Both)
                {
                    newEffect.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    if (card.rarity == Rarity.Enemy)
                    {
                        newEffect.transform.rotation = Quaternion.Euler(-90f+Random.Range(10f,-10f), Random.Range(10f, -10f), 180f);
                    }
                    else
                    {
                        newEffect.transform.rotation = Quaternion.Euler(-90f + Random.Range(10f, -10f), Random.Range(10f, -10f), 0f);

                    }
                }
                else
                {
                    newEffect.transform.position = new Vector3(newEffect.transform.position.x, 2f, newEffect.transform.position.z);
                }
                _particlePool[(card.element, target)].Add(viableParticle);
                return newEffect;
            }
            else
            {
                viableParticle.transform.position = position;
                viableParticle.GetComponent<ParticleSystem>().Play();
                return viableParticle;
            }
        }

        
    }

    public GameObject GetPrefabTargets(Element element, EffectTarget target)
    {
        switch (target)
        {
            case EffectTarget.Self:
            case EffectTarget.OpponentSingle:
            case EffectTarget.OpponentRandom:
            case EffectTarget.Both:
                return GetSingleTargetPrefab(element);
            case EffectTarget.OpponentAll:
            case EffectTarget.Global:
                return GetMultiTargetPrefab(element);
        }
        return null;
    }

    public GameObject GetSingleTargetPrefab(Element element)
    {
        switch (element)
        {
            case Element.NONE:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/Single/NoneSlash");
            case Element.WATER:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/Single/WaterSlash");
            case Element.FIRE:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/Single/FireSlash");
            case Element.EARTH:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/Single/EarthSlash");
            case Element.WIND:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/Single/WindSlash");
            case Element.GHOST:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/Single/GhostSlash");
            case Element.DARK:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/Single/DarkSlash");
            case Element.LIGHT:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/Single/LightSlash");
        }
        return null;
    }

    public GameObject GetMultiTargetPrefab(Element element)
    {
        switch (element)
        {
            case Element.NONE:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/AoE effects/NoneAoE");
            case Element.WATER:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/AoE effects/WaterAoE");
            case Element.FIRE:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/AoE effects/FireAoE");
            case Element.EARTH:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/AoE effects/EarthAoE");
            case Element.WIND:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/AoE effects/WindAoE");
            case Element.GHOST:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/AoE effects/GhostAoE");
            case Element.DARK:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/AoE effects/DarkAoE");
            case Element.LIGHT:
                return Resources.Load<GameObject>("VFX/Hovl Studio/Magic effects pack/Prefabs/AoE effects/LightAoE");
        }
        return null;
    }
}
