using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovement _playerMovement;
        private IInteractable _currentInteractable;
        private Vector3 _currentPos;

        private void Update()
        {
            UpdatePos();
            // インタラクションキーのチェック
            if (!Input.GetKeyDown(KeyCode.E) || _currentInteractable == null) return;
            _currentInteractable.Interact();
        }

        private void UpdatePos()
        {
            if (_currentPos == _playerMovement.CurrentDirection) return;            
            _currentPos = _playerMovement.CurrentDirection;
            transform.localPosition = _currentPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable == null) return;
            _currentInteractable = interactable;
        }

        private void OnTriggerExit(Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable == null || _currentInteractable != interactable) return;
            _currentInteractable = null;
        }
    }
}