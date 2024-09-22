using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Minimalist.Inverse
{
    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
            // Attributes are read here
            GameAttributes.OnInit();
        }

        //TODO: Apply Read Data to sub systems here.
    }
}