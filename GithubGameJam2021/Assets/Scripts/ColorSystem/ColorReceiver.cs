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
            m_ColorReactor = gameObject.GetComponent<IColorReactor>();
        }

        private void Start()
        {
            m_CurrentColorHolder.setColor(m_CurrentColorType);
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

