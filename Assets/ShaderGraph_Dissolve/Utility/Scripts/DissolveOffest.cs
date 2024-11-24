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
    private Material material;
    private bool isDissolving = false;
    private float dissolveValue = -300f; // Start at the "intact" value
    public float dissolveSpeed = 10f;   // Adjust dissolve speed
    public float maxDissolve = 1f;      // Fully dissolved value

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material; // Ensure a unique instance of the material
        }

        if (material == null)
        {
            Debug.LogError($"No material found on {gameObject.name}!");
        }
    }

    public void ActivateDissolve()
    {
        if (!isDissolving && material != null)
        {
            Debug.Log($"Dissolve activated for {gameObject.name}");
            StartCoroutine(DissolveCoroutine());
        }
    }

    private IEnumerator DissolveCoroutine()
    {
        isDissolving = true;

        while (dissolveValue <= maxDissolve) // Progress dissolve until fully dissolved
        {
            dissolveValue += Time.deltaTime * dissolveSpeed;
            material.SetFloat("_Dissolve", dissolveValue); // Update dissolve value
            yield return null;
        }

        material.SetFloat("_Dissolve", maxDissolve); // Ensure fully dissolved at the end
        Debug.Log($"Dissolve completed for {gameObject.name}");
        isDissolving = false;
    }
}



