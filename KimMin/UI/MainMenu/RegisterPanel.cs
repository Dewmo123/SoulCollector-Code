using DewmoLib.Dependencies;
using Scripts.Network;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Work.UI.MainMenu
{
    public class RegisterPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private TextMeshProUGUI failText; //실패 텍스트
        [SerializeField] private Button registerButton;
        [SerializeField] private Button exitButton;
        [Inject] WebClient _webClient;
        private void Awake()
        {
            registerButton.onClick.AddListener(HandleRegisterClick);
            exitButton.onClick.AddListener(HandleExitClick);
            gameObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            registerButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
        
        private void HandleExitClick()
        {
            gameObject.SetActive(false);
        }

        private async void HandleRegisterClick()
        {
            //회원가입 시도
            CreateUserDTO dto = new()
            {
                Password = passwordField.text,
                UserId = nameField.text
            };
            bool success = await _webClient.SendPostRequest("authorize/sign-up", dto);
            if (success)
            {
                gameObject.SetActive(false);
            }
        }
    }
}