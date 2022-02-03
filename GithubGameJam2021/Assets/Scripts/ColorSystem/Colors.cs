using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    public class Colors
    {
        private static Colors m_Instance = null;
        static Dictionary<eColors, Color> m_Colors;

        private Colors()
        {
            initColorsDictionary();
        }

        private static void initColorsDictionary()
        {
            m_Colors = new Dictionary<eColors, Color>();
            m_Colors.Add(eColors.Red, Color.red);
            m_Colors.Add(eColors.Green, Color.green);
            m_Colors.Add(eColors.Blue, Color.blue);
            m_Colors.Add(eColors.Yellow, Color.yellow);
            m_Colors.Add(eColors.Orange, new Color(1, 0.6f, 0));
            m_Colors.Add(eColors.Violet, new Color(1, 0, 1));
            m_Colors.Add(eColors.White, Color.white);
            m_Colors.Add(eColors.Black, Color.black);
            m_Colors.Add(eColors.PlayerDefault, Color.white);
        }

        public static Colors GetInstance()
        {
            if(m_Instance == null)
            {
                m_Instance = new Colors();
            }

            return m_Instance;
        }

        public Color getColor(eColors iColorToGet)
        {
            Color color;

            color = m_Colors[iColorToGet];

            return color;
        }


    }

    public enum eColors
    {
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Violet,
        White,
        Black,
        PlayerDefault
    }
}

