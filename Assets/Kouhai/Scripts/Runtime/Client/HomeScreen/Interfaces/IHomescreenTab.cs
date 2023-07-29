using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kouhai.Runtime.Client
{
    public interface IHomescreenTab
    {
        public void Initialise(float toolbarWidth);
        public void OnClose();
    }
}

