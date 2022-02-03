using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    /// <summary>
    /// Required class to spread a color to a GameObject.
    /// </summary>
    public class ColorSpreaderSignal : MonoBehaviour
    {
        [SerializeField] private int m_MaterialToChangeIndex = 0;

        public int MaterialIndex
        {
            get
            {
                return m_MaterialToChangeIndex;
            }
        }

        public void ChangeColor(eColors i_ColorToChange)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();

            if(renderer == null)
            {
                renderer = gameObject.GetComponentInChildren<Renderer>();
            }

            Material mat = renderer.materials[m_MaterialToChangeIndex];

            mat.color = Colors.GetInstance().getColor(i_ColorToChange);
        }

    }
}
