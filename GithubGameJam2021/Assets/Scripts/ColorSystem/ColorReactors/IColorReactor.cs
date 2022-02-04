using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    public interface IColorReactor
    {
        void React(GameObject i_GameObjectToReactTo, ColorHolder i_CollidedObjColor, ColorHolder i_SelfColor);
    }
}
