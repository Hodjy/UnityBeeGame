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
        //[SerializeField] private List<GameObject> m_ObjectsParentsToSpreadSecondary;

        public void SpreadToMain(eColors i_ColorToSpread)
        {
            spread(i_ColorToSpread, m_ObjectsParentsToSpread);
        }

        private void spread(eColors i_ColorToSpread, List<GameObject> io_ObjectsToColor)
        {
            ColorSpreaderSignal[] signals;

            foreach (GameObject gameObject in io_ObjectsToColor)
            {
                signals = gameObject.GetComponentsInChildren<ColorSpreaderSignal>();

                foreach (ColorSpreaderSignal sig in signals)
                {
                    sig.ChangeColor(i_ColorToSpread);
                }
            }
        }

        private void SpreadSecondary(eColors i_ColorToSpread)
        {
            //spread(i_ColorToSpread, m_ObjectsParentsToSpreadSecondary);
        }
    }
}