using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class DialogLoading : MonoBehaviour
    {
        public Transform _loadingTf;

        private void Update()
        {
            _loadingTf.Rotate(new Vector3(0f, 0f, -Time.deltaTime * 20f));
        }
    }
}
