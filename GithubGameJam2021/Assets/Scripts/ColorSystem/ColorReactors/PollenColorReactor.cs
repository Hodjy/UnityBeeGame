using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    public class PollenColorReactor : MonoBehaviour, IColorReactor
    {
        private void Start()
        {
            GetComponent<ColorSpreader>().SpreadToMain(
                GetComponent<ColorHolder>().CurrentColorType);
        }

        public void React(GameObject i_CollidedObject, ColorHolder i_CollidedObjColor, ColorHolder i_SelfColor)
        {
            i_CollidedObjColor.setColor(i_SelfColor.CurrentColorType);
            knockbackPlayer(i_CollidedObject);
        }

        private void knockbackPlayer(GameObject i_Player)
        {
            Vector3 knockbackDirection = i_Player.transform.position - transform.position;
            i_Player.GetComponent<PlayerController>().Knockbacked(knockbackDirection);
        }
    }
}

