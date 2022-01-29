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
        private eColors m_CurrentColorType;

        [SerializeField]
        private Material m_MaterialToColor;

        public ColorHolder(eColors i_ColorType, Material i_MaterialToColor)
        {
            m_MaterialToColor = i_MaterialToColor;
            setColor(i_ColorType);
        }

        /// <summary>
        /// Changes the color of the assinged material to the new color.
        /// </summary>
        /// <param name="i_ColorType"></param>
        /// <param name="i_Color"></param>
        public void setColor(eColors i_ColorType)
        {
            if(m_MaterialToColor != null)
            {
                Color color = Colors.GetInstance().getColor(i_ColorType);

                m_CurrentColorType = i_ColorType;
                m_MaterialToColor.color = color;
                
            }
        }

        public eColors CurrentColorType { get => m_CurrentColorType; }
    }
}
