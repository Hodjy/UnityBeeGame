using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    /// <summary>
    /// Will spread the given color to the assigned objects which have the ColorSpreaderSignal component.
    /// Does not work for deep nested items.
    /// </summary>
    public class ColorSpreader : MonoBehaviour
    {
        [SerializeField] private List<GameObject> m_ObjectsParentsToSpread;

        public void Spread(eColors i_ColorToSpread)
        {
            ColorSpreaderSignal[] signals;

            foreach (GameObject gameObject in m_ObjectsParentsToSpread)
            {
                signals = gameObject.GetComponentsInChildren<ColorSpreaderSignal>();

                foreach (ColorSpreaderSignal sig in signals)
                {
                    sig.ChangeColor(i_ColorToSpread);
                }
            }
        }
    }
}