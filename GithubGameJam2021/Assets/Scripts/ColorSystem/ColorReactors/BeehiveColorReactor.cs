using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    public class BeehiveColorReactor : MonoBehaviour, IColorReactor
    {
        [SerializeField] private List<GameObject> m_HiddenGameObjects;

        //Is needed for painting all the game objects relating to this objects reaction.
        [SerializeField] private ColorSpreader m_ColorSpreader;

        public void React(GameObject i_GameObject)
        {
            
            m_ColorSpreader.Spread(gameObject.GetComponent<ColorHolder>().CurrentColorType);

            foreach(GameObject obj in m_HiddenGameObjects)
            {
                obj.SetActive(true);
            }

            i_GameObject.GetComponent<ColorHolder>().setColor(eColors.PlayerDefault); // + sfx
            Debug.Log("Knockback!");
        }


    }

}
