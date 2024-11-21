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
    private List<Material> materials = new List<Material>();
    private bool isDissolving = false;
    private float dissolveValue = -0.5f;
    public float dissolveSpeed = 0.5f;

    void Start()
    {
        var renders = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renders)
        {
            materials.AddRange(renderer.materials);
        }

        Debug.Log($"Collected {materials.Count} materials for {gameObject.name}");
    }

    public void AssignTextures(Texture2D baseMap, Texture2D normalMap)
    {
        foreach (var material in materials)
        {
            if (material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", baseMap);
            }
            if (material.HasProperty("_NormalMap"))
            {
                material.SetTexture("_NormalMap", normalMap);
            }
        }
        Debug.Log($"Textures assigned to {gameObject.name}");
    }

    public void ActivateDissolve()
    {
        if (!isDissolving)
        {
            Debug.Log($"Dissolve activated for {gameObject.name}");
            StartCoroutine(DissolveOnce());
        }
    }

    private IEnumerator DissolveOnce()
    {
        isDissolving = true;

        while (dissolveValue < 1.1f)
        {
            dissolveValue += Time.deltaTime * dissolveSpeed;
            SetValue(dissolveValue);
            yield return null;
        }

        SetValue(1.1f);
        Debug.Log($"Dissolve completed for {gameObject.name}");
        isDissolving = false;
    }

    public void SetValue(float value)
    {
        foreach (var material in materials)
        {
            if (material.HasProperty("_DissolveOffest"))
            {
                material.SetVector("_DissolveOffest", new Vector4(0f, value, 0f, 0f));
            }
            else
            {
                Debug.LogWarning($"Material {material.name} does not have _DissolveOffest property!");
            }
        }
    }
}




