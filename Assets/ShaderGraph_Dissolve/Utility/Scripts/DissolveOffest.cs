using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 public class DissolveOffest : MonoBehaviour

/*

{  
        // Start is called before the first frame update
        List<Material> materials = new List<Material>();
        bool PingPong = false;
        void Start()
        {
            var renders = GetComponents<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);
            }
        }

        private void Reset()
        {
            Start();
            SetValue(0);
        }

        // when the player enters the trigger the corutine will start

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                //StartCoroutine(enumerator());
                PingPong = true;
            }
            if (PingPong)
            {
                var value = Mathf.PingPong(Time.time * 0.5f, 1.6f);
                value -= 0.5f;
                SetValue(value);
            }
        }

        
          private IEnumerator DissolveCoroutine()
    {
        while (true)
        {
            float value = Mathf.PingPong(Time.time * 0.5f, 1.6f) - 0.5f;
            SetValue(value);
            yield return new WaitForEndOfFrame();
        }
    }
        
     //  IEnumerator enumerator()
     
     //   {
        
     //       float value = while (true)
     //       {
     //           Mathf.PingPong(value, 1f);
     //           value += Time.deltaTime;
     //           SetValue(value);
     //          yield return new WaitForEndOfFrame();
     //       }
     //   }

          IEnumerator enumerator()
    {
        float value = 0f; // Initialize the value
        while (true)
        {
            value = Mathf.PingPong(Time.time * 0.5f, 1f);
            SetValue(value);
            yield return new WaitForEndOfFrame();
        }
    }

        public void SetValue(float value)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetVector("_DissolveOffest", new Vector4(0f,value,0f,0f));
            }
        }
    
}*/

{
    [Header("Dissolve Settings")]
    [SerializeField] private float defaultDissolveValue = -300f; // Default dissolve state for this hallway
    [SerializeField] private float goalDissolveValue = 300f;    // Target dissolve state
    [SerializeField] private float dissolveSpeed = 10f;         // Speed of dissolve

    private Material material;
    private bool isDissolving = false; // Flag to track if dissolve is active
    private float currentDissolveValue; // Tracks the current dissolve value

    void Start()
    {
        // Get the material and initialize dissolve value
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material; // Ensure unique instance of material
        }

        if (material == null)
        {
            Debug.LogError($"No material found on {gameObject.name}!");
            return;
        }

        // Initialize the dissolve value to the default state
        currentDissolveValue = defaultDissolveValue;
        material.SetFloat("_Dissolve", currentDissolveValue);
    }

    /// <summary>
    /// Activates the dissolve effect, moving towards the specified goal.
    /// </summary>
    public void ActivateDissolve()
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve activated for {gameObject.name}");
            StartCoroutine(DissolveCoroutine(goalDissolveValue));
        }
    }

    /// <summary>
    /// Activates the dissolve effect, specifying a custom goal dissolve value.
    /// </summary>
    /// <param name="targetDissolveValue">The desired end dissolve value.</param>
    public void ActivateDissolve(float targetDissolveValue)
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve activated for {gameObject.name} towards goal: {targetDissolveValue}");
            StartCoroutine(DissolveCoroutine(targetDissolveValue));
        }
    }

    /// <summary>
    /// Coroutine to handle the dissolve effect.
    /// </summary>
    /// <param name="targetValue">The target dissolve value to reach.</param>
    private IEnumerator DissolveCoroutine(float targetValue)
    {
        isDissolving = true;

        while (!Mathf.Approximately(currentDissolveValue, targetValue))
        {
            // Lerp towards the target dissolve value
            currentDissolveValue = Mathf.MoveTowards(currentDissolveValue, targetValue, Time.deltaTime * dissolveSpeed);
            material.SetFloat("_Dissolve", currentDissolveValue);
            yield return null;
        }

        Debug.Log($"Dissolve completed for {gameObject.name}, reached: {currentDissolveValue}");
        isDissolving = false;
    }

    /// <summary>
    /// Resets the dissolve effect to the default state.
    /// </summary>
    public void ResetDissolve()
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve reset for {gameObject.name}");
            StartCoroutine(DissolveCoroutine(defaultDissolveValue));
        }
    }
}

