using Components;
using UnityEngine;

namespace Remainders
{
    public class RemainderView : ViewBase
    {
        [SerializeField] private Transform _oneRemainder;
        [SerializeField] private Transform _twoRemainder;
        public Transform OneRemainder => _oneRemainder;
        public Transform TwoRemainder => _twoRemainder;
    }
}
