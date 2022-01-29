using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    public class ColorReceiver : MonoBehaviour
    {
        [SerializeField]
        private Material m_MaterialToColor; // for self, will take the chosen color from mCurrentColorType.

        [SerializeField]
        private eColors m_CurrentColorType;

        private ColorHolder m_CurrentColorHolder;

        [SerializeField]
        private IColorReactor m_ColorReactor;

        private void Start()
        {
            m_CurrentColorHolder = new ColorHolder(m_CurrentColorType, m_MaterialToColor);
        }

        private void OnCollisionEnter(Collision collision)
        {
            checkIfPlayerCompareColorsAndReact(collision);
        }

        private void checkIfPlayerCompareColorsAndReact(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ColorHolder playerColorHolder = collision.gameObject.GetComponent<ColorHolder>();

                if (playerColorHolder == null)
                {
                    Debug.Log("Player does not have a ColorHolder script!");
                    return;
                }

                if (playerColorHolder.CurrentColorType == m_CurrentColorHolder.CurrentColorType)
                {
                    m_ColorReactor.React(collision.gameObject); // Send the player to the reactor
                }
            }
        }
    }
}

