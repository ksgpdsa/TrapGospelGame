using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager _instance;
    public static CoroutineManager StaticCoroutineManager
    {
        get
        {
            if (_instance) return _instance;
            
            var gameObject = new GameObject("CoroutineManager");
            DontDestroyOnLoad(gameObject);
            _instance = gameObject.AddComponent<CoroutineManager>();

            return _instance;
        }
    }

    private readonly List<Coroutine> _activeCoroutines = new List<Coroutine>();

    /// <summary>
    /// Inicia uma coroutine e a adiciona à lista de controle.
    /// </summary>
    public IEnumerator RunCoroutine(IEnumerator routine)
    {
        yield return StartCoroutine(TrackCoroutine(routine));
    }

    /// <summary>
    /// Para uma coroutine específica e remove da lista.
    /// </summary>
    public void StopTrackedCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            _activeCoroutines.Remove(coroutine);
        }
    }

    /// <summary>
    /// Espera até todas as coroutines rastreadas terminarem.
    /// </summary>
    public IEnumerator WaitForAll()
    {
        while (_activeCoroutines.Count > 1)
        {
            Debug.Log(_activeCoroutines[0].ToString());
            yield return null;
        }
    }

    private IEnumerator TrackCoroutine(IEnumerator routine)
    {
        var coroutine = StartCoroutine(routine);
        _activeCoroutines.Add(coroutine);
        yield return coroutine;
        _activeCoroutines.Remove(coroutine);
    }
}