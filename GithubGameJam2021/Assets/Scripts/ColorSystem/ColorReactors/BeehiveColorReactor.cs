using AudioSystem;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    public class BeehiveColorReactor : MonoBehaviour, IColorReactor
    {
        [SerializeField] private List<GameObject> m_HiddenGameObjects;
        private bool m_IsHiveActivated = false;
        private const string rm_HiveHitSFXName = "BeehiveHit";

        //Is needed for painting all the game objects relating to this objects reaction.
        [SerializeField] private ColorSpreader m_ColorSpreader;

        //[SerializeField] private eColors m_SecondaryColor;

        private void Start()
        {
            m_ColorSpreader.SpreadToMain(GetComponent<ColorHolder>().CurrentColorType);
            //m_ColorSpreader.SpreadSecondary(m_SecondaryColor);
        }

        public void React(GameObject i_GameObject, ColorHolder i_CollidedObjColor, ColorHolder i_SelfColor)
        {
            if(!m_IsHiveActivated)
            {
                if (i_CollidedObjColor.CurrentColorType == i_SelfColor.CurrentColorType)
                {
                    activateHiddenObjsAndReactPlayer(i_GameObject, i_CollidedObjColor, i_SelfColor);
                    m_IsHiveActivated = true;
                }
            }
        }

        private void activateHiddenObjsAndReactPlayer(GameObject i_Player, ColorHolder i_CollidedObjColor,
            ColorHolder i_SelfColorHolder)
        {
            foreach (GameObject obj in m_HiddenGameObjects)
            {
                obj.SetActive(true);
            }

            FindObjectOfType<AudioManager>().PlaySfx(rm_HiveHitSFXName);
            i_CollidedObjColor.setColor(eColors.PlayerDefault); 
            knockbackPlayer(i_Player);
        }

        private void knockbackPlayer(GameObject i_Player)
        {
            Vector3 knockbackDirection = i_Player.transform.position - transform.position;
            i_Player.GetComponent<PlayerController>().Knockbacked(knockbackDirection);
        }
    }

}
