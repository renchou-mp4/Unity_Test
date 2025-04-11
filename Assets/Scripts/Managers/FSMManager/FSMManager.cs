using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Managers
{
    // ReSharper disable once InconsistentNaming
    public class FSMManager : MonoSingleton<FSMManager>
    {
        public FSMManager()
        {
            GameObject obj = new GameObject("FSM")
            {
                transform =
                {
                    parent = transform
                }
            };
            
            obj.AddComponent<ProcessFSM>();
        }
    }
}