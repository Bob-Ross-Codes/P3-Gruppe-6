using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace DissolveExample

/*{
    public class DissolveChilds : MonoBehaviour
    {
        // Start is called before the first frame update
        List<Material> materials = new List<Material>();
        bool PingPong = false;
        void Start()
        {
            var renders = GetComponentsInChildren<Renderer>();
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

        // Update is called once per frame
        void Update()
        {

            var value = Mathf.PingPong(Time.time * 0.5f, 1f);
            SetValue(value);
        }

        //IEnumerator enumerator()
        //{

        //    //float value =         while (true)
        //    //{
        //    //    Mathf.PingPong(value, 1f);
        //    //    value += Time.deltaTime;
        //    //    SetValue(value);
        //    //    yield return new WaitForEndOfFrame();
        //    //}
        //}

        public void SetValue(float value)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_Dissolve", value);
            }
        }
    }
}*/

//namespace DissolveExample

    public class DissolveChilds : MonoBehaviour
    {
        List<Material> materials = new List<Material>();

        void Start()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                materials.AddRange(renderer.materials);
            }
        }

        void Update()
        {
            // Example dissolve effect: X-axis based dissolve
            float dissolveValue = Mathf.PingPong(Time.time * 0.5f, 1f);
            SetValue(dissolveValue);
        }

/*public void SetValue(float value)
{
    foreach (var material in materials)
    {
        if (material.HasProperty("_DissolveOffset"))
        {
            material.SetVector("_DissolveOffset", new Vector4(value, 0, 0, 0));
        }
        else if (material.HasProperty("_DissolveDirection"))
        {
            material.SetVector("_DissolveDirection", new Vector3(value, 0, 0));
        }
    }
}
*/

        public void SetValue(float value)
        {
            foreach (var material in materials)
            {
                if (material.HasProperty("_Dissolve"))
                {
                    material.SetFloat("_Dissolve", value); // Adjust dissolve value
                }
            }
        }
    }

