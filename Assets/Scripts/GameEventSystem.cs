using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using voidargf = System.Action<System.Collections.Generic.Dictionary<string, object>>;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Instance;
    public GameObject DeathScreen;
    void Awake()
    {
        DeathScreen.SetActive(false);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //registerCallback("sus", new List<voidargf>{ (Dictionary<string, object> dic) => Debug.Log(dic["ses"]) });
        //callEvent("sus", new Dictionary<string, object> { {"yeag", 123 }});
        //registerCallback("sus", new() { dic => Debug.Log(dic["yeag"]) });
        //callEvent("sus", new() { { "yeag", 123 } });
    }
    //-----
    
    private class GameAction
    {
        static Regex rx = new Regex(@"^The given key '(.*)' was not present in the dictionary.$");
        /// <summary>
        /// Inner exception for DSL execution issues.
        /// </summary>
        class InnerDSLException : Exception
        {   
            public InnerDSLException(string message, Exception inner) : base(message, inner) { }
        }
        public event voidargf callback = delegate { };
        public bool? call(Dictionary<string, object> ob) 
        {
            try
            {
                callback(ob);
            }
            catch(KeyNotFoundException e)
            {
                throw new InnerDSLException($"Key in object dictionary not found: \"{rx.Match(e.Message).Groups[1].Value}\"", e);
            }
            catch(NullReferenceException e)
            {
                throw new InnerDSLException("nullBointer", e);
            }
            catch(Exception e)
            {
                throw new InnerDSLException("Yeah something's fucky", e);
            }
            return true;
        }

        public GameAction(List<voidargf> vals)
        {
            if(vals!=null)
            {
                foreach (voidargf a in vals)
                    callback += a;
            }
        }

        public void appendCallback(voidargf f)
        {
            callback += f;
        }
    }
    private Dictionary<string, GameAction> callbacks = new();
    /// <summary>
    /// add a new callback with an optional list of gameactions
    /// you NEED to call removeCallback after it stops being useful
    /// </summary>
    /// <param name="key">key for callback; doesn't care if it exist, it will overrite it</param>
    /// <param name="vals">yeag</param>
    public void registerCallback(string key, List<voidargf> vals = null)
    {
        if (key == "") return;
        callbacks.Add(key, new GameAction(vals));
    }

    /// <summary>
    /// add a new callback with exactly one gameaction
    /// you NEED to call removeCallback after it stops being useful
    /// </summary>
    /// <param name="key">key for callback; doesn't care if it exist, it will overrite it</param>
    /// <param name="val">yeag</param>
    public void registerCallback(string key, voidargf val)
    {
        callbacks.Add(key, new GameAction(new() { val }));
    }
    public void addCallback(string key, voidargf a)
    {
        if (callbacks.TryGetValue(key, out var act))
        {
            act.appendCallback(a);
        }
        else throw new DSLException($"Cannot find event with \"{key}\" to append callback", null);
    }
    public void removeCallback(string key)
    {
        callbacks.Remove(key);
    }
    /// <summary>
    /// spicy
    /// </summary>
    public void clearCallbacks()
    {
        callbacks.Clear();
    }
    /// <summary>
    /// Top-level exception for everything that's fucky in DSL execution.
    /// </summary>
    public class DSLException : Exception
    {
        public DSLException(string message, Exception inner) : base(message, inner) { }
    }
    public void callEvent(string key, Dictionary<string, object> ob = null)
    {
        if (key == null) return;
        if (getCallback(key)?.call(ob) == null)
        {
            throw new DSLException("No callback for given key found", null);
        }
    }

    private GameAction getCallback(string key)
    {
        callbacks.TryGetValue(key, out GameAction a);
        return a;
    }
    //-----

    //Health system
    public event Action OnPlayerTakeDamage;
    public event Action OnPlayerHeal;//to be replaced with regeneration
    public event Action OnPlayerDead;
    public event Action OnPlayerRespawn;
    public event Action OnPlayerHurt;
    public event Action OnPlayerHealthy;

    //Level1
    public event Action OnLightsTurnOn;
    public event Action OnLightsTurnOff;
    public event Action OnKeyCardScanned;

    //Utils
    public event Action OnFlashlightClick;

    //Level2
    public event Action OnGeneratorButtonPressed;
    public event Action OnGeneratorWrongOrder;

    public event Action OnFuelAdded;

    //health system implementation
    public void PlayerStartHeartbeatEffect()
    {
        OnPlayerHurt?.Invoke();
    }

    public void PlayerStopHeartbeatEffect()
    {
        OnPlayerHealthy?.Invoke();
    }

    public void PlayerGetDamage(int damage)
    {
        OnPlayerTakeDamage?.Invoke();
    }

    public void PlayerDeath()
    {
        StartCoroutine(PlayerDeathCoroutine());
        //SceneManager.LoadScene(0);//todo change that to appropriete number when main menu is added todotodo check if that works on build
    }

    private IEnumerator PlayerDeathCoroutine()
    {
        OnPlayerRespawn?.Invoke();
        DeathScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        DeathScreen.SetActive(false);
    }

    //Level 1 implementation

    public void LightsTurnOn()
    {
        OnLightsTurnOn?.Invoke();
    }

    public void LightsTurnOff()
    {
        OnLightsTurnOff?.Invoke();
    }

    public void KeyCardScanned()
    {
        OnKeyCardScanned?.Invoke();
    }

    //utils implementation
    public void FlashlightClick()
    {
        OnFlashlightClick?.Invoke();
    }

    //Level 2 implementation
    public void ButtonActivated()
    {
        OnGeneratorButtonPressed?.Invoke();
    }

    public void ButtonReset()
    {
        OnGeneratorWrongOrder?.Invoke();
    }

    public void FuelAdded()
    {
        OnFuelAdded?.Invoke();
    }
}
