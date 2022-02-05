using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    /// <summary>
    /// This class changes the assigned material color by the setColor input.
    /// </summary>
    public class ColorHolder : MonoBehaviour
    {
        [SerializeField] private eColors m_CurrentColorType;
        [SerializeField] private Renderer m_GameObjectRenderer;
        [SerializeField] private int m_MaterialToChangeIndex = 0;

        private void Awake()
        {
            if(m_GameObjectRenderer == null)
            {
                m_GameObjectRenderer = gameObject.GetComponent<Renderer>();
            }

            setColor(m_CurrentColorType);
        }

        /// <summary>
        /// Changes the color of the assinged material to the new color.
        /// </summary>
        /// <param name="i_ColorType"></param>
        /// <param name="i_Color"></param>
        virtual public void setColor(eColors i_ColorType)
        {
            if (m_GameObjectRenderer != null)
            {
                Color color = Colors.GetInstance().getColor(i_ColorType);

                m_CurrentColorType = i_ColorType;
                m_GameObjectRenderer.materials[m_MaterialToChangeIndex].color = color;
            }
        }

        public eColors CurrentColorType { get => m_CurrentColorType; }
    }


}
