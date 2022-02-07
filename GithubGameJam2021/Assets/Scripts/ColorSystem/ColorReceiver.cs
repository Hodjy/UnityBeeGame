using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    public class ColorReceiver : MonoBehaviour
    {
        [SerializeField] private eColors m_CurrentColorType;
        private IColorReactor m_ColorReactor;
        private ColorHolder m_CurrentColorHolder;

        private void Awake()
        {
            m_CurrentColorHolder = gameObject.AddComponent<ColorHolder>();
            m_CurrentColorHolder.setColor(m_CurrentColorType);
            m_ColorReactor = gameObject.GetComponent<IColorReactor>();
            if(m_ColorReactor == null)
            {
                Debug.Log(gameObject + "Missing IColorReactor implementation!");
            }
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

                // Send the player to the reactor
                m_ColorReactor.React(collision.gameObject, playerColorHolder, m_CurrentColorHolder); 
            }
        }
    }
}

