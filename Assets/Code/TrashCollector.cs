using UnityEngine;

namespace Code
{
    public class TrashCollector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent<TrashFloating>(out var trash)) return;
            Destroy(trash.gameObject);
        }
    }
}