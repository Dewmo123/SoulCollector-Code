using System;
using UnityEngine;

namespace Work.Core
{
    public class InactiveOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}