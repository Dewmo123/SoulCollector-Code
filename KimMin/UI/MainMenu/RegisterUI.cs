using DewmoLib.Dependencies;
using Scripts.Network;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Work.UI.MainMenu
{
    public class RegisterUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button registerButton;
        [SerializeField] private RegisterPanel registerPanel;
        [SerializeField] private TextMeshProUGUI failText; //실패 텍스트
        [Inject] WebClient _webClient;
        private void Awake()
        {
            loginButton.onClick.AddListener(HandleLoginClick);
            registerButton.onClick.AddListener(HandleRegisterClick);
        }

        private void OnDestroy()
        {
            loginButton.onClick.RemoveAllListeners();
            registerButton.onClick.RemoveAllListeners();
        }
        
        private async void HandleLoginClick()
        {
            //로그인 시도
            LoginDTO login = new()
            {
                UserId = nameField.text,
                Password = passwordField.text
            };
            bool success = await _webClient.SendPostRequest<LoginDTO>("authorize/log-in", login);
            if (success)
                SceneManager.LoadScene("LoadScene");
        }

        private void HandleRegisterClick()
        {
            registerPanel.gameObject.SetActive(true);
        }
    }
}