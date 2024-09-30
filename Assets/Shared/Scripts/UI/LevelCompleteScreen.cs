using System;
using System.Numerics;
using Cysharp.Threading.Tasks;
using HyperCasual.Core;
using Immutable.Passport;
using Shared.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    ///     This View contains celebration screen functionalities
    /// </summary>
    public class LevelCompleteScreen : View
    {
        [SerializeField] private HyperCasualButton m_NextButton;

        [SerializeField] private HyperCasualButton m_TryAgainButton;

        [SerializeField] private HyperCasualButton m_ContinuePassportButton;

        [SerializeField] private Image[] m_Coins;

        [SerializeField] private TextMeshProUGUI m_FoodText;

        [SerializeField] private Slider m_XpSlider;

        [SerializeField] private GameObject m_Loading;

        [SerializeField] private GameObject m_CompletedContainer;

        [SerializeField] private TextMeshProUGUI m_ErrorMessage;

        [SerializeField] private AbstractGameEvent m_NextLevelEvent;

        [SerializeField] private AbstractGameEvent m_SetupWalletEvent;

        [SerializeField] private AbstractGameEvent m_UnlockedSkinEvent;

        private int m_CoinCount = -1;

        private int m_FoodValue;

        private float m_XpValue;

        /// <summary>
        ///     The slider that displays the XP value
        /// </summary>
        public Slider XpSlider => m_XpSlider;

        /// <summary>
        ///     The amount of food to display on the celebration screen.
        ///     The setter method also sets the celebration screen text.
        /// </summary>
        public int FoodValue
        {
            get => m_FoodValue;
            set
            {
                if (m_FoodValue != value)
                {
                    m_FoodValue = value;
                    m_FoodText.text = FoodValue.ToString();
                }
            }
        }

        /// <summary>
        ///     The amount of XP to display on the celebration screen.
        ///     The setter method also sets the celebration screen slider value.
        /// </summary>
        public float XpValue
        {
            get => m_XpValue;
            set
            {
                if (!Mathf.Approximately(m_XpValue, value))
                {
                    m_XpValue = value;
                    m_XpSlider.value = m_XpValue;
                }
            }
        }

        /// <summary>
        ///     The number of tokens to display on the celebration screen.
        /// </summary>
        public int CoinCount
        {
            get => m_CoinCount;
            set
            {
                if (m_CoinCount != value)
                {
                    m_CoinCount = value;
                    DisplayCoins(m_CoinCount);
                }
            }
        }

        public async void OnEnable()
        {
            // Set listener to 'Next' button
            m_NextButton.RemoveListener(OnNextButtonClicked);
            m_NextButton.AddListener(OnNextButtonClicked);

            // Set listener to "Continue with Passport" button
            m_ContinuePassportButton.RemoveListener(OnContinueWithPassportButtonClicked);
            m_ContinuePassportButton.AddListener(OnContinueWithPassportButtonClicked);

            // Set listener to "Try again" button
            m_TryAgainButton.RemoveListener(OnTryAgainButtonClicked);
            m_TryAgainButton.AddListener(OnTryAgainButtonClicked);

            ShowError(false);
            ShowLoading(false);

            // If player is logged into Passport mint coins to player
            if (SaveManager.Instance.IsLoggedIn)
            {
                // Mint collected coins to player
                await MintCoins();
            }
            else
            {
                // Show 'Next' button if player is already logged into Passport
                ShowNextButton(SaveManager.Instance.IsLoggedIn);
                // Show "Continue with Passport" button if the player is not logged into Passport
                ShowContinueWithPassportButton(!SaveManager.Instance.IsLoggedIn);
            }
        }

        /// <summary>
        ///     Mints collected coins (i.e. Immutable Runner Token) to the player's wallet
        /// </summary>
        private async UniTask MintCoins()
        {
            // This function is similar to MintCoins() in MintScreen.cs. Consider refactoring duplicate code in production.
            Debug.Log("Minting coins...");
            var success = false;

            // Show loading
            ShowLoading(true);
            ShowNextButton(false);
            ShowError(false);

            try
            {
                // Don't mint any coins if player did not collect any
                if (m_CoinCount == 0)
                {
                    success = true;
                }
                else
                {
                    // Get the player's wallet address to mint the coins to
                    var address = SaveManager.Instance.WalletAddress;
                    if (address != null)
                    {
                        // Calculate the quantity to mint
                        // Need to take into account Immutable Runner Token decimal value i.e. 18
                        var quantity = BigInteger.Multiply(new BigInteger(m_CoinCount), BigInteger.Pow(10, 18));
                        Debug.Log($"Quantity: {quantity}");
                        success = await ApiService.MintCoins(address, quantity.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to mint coins: {ex.Message}");
            }

            ShowLoading(false);
            ShowNextButton(success);
            ShowError(!success);
        }

        private async void OnContinueWithPassportButtonClicked()
        {
            try
            {
                // Show loading
                ShowContinueWithPassportButton(false);
                ShowLoading(true);

                // Log into Passport
#if (UNITY_ANDROID && !UNITY_EDITOR_WIN) || (UNITY_IPHONE && !UNITY_EDITOR_WIN) || UNITY_STANDALONE_OSX || UNITY_WEBGL
                await Passport.Instance.LoginPKCE();
#else
                await Passport.Instance.Login();
#endif

                // Successfully logged in
                // Save a persistent flag in the game that the player is logged in
                SaveManager.Instance.IsLoggedIn = true;
                // Show 'Next' button
                ShowNextButton(true);
                ShowLoading(false);
                // Take the player to the Setup Wallet screen
                m_SetupWalletEvent.Raise();
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to log into Passport: {ex.Message}");
                // Show Continue with Passport button again
                ShowContinueWithPassportButton(true);
                ShowLoading(false);
            }
        }

        private async void OnTryAgainButtonClicked()
        {
            await MintCoins();
        }

        private void OnNextButtonClicked()
        {
            // Check if the player is already using a new skin
            if (!SaveManager.Instance.UseNewSkin)
                // Player is not using a new skin, take player to Unlocked Skin screen
                m_UnlockedSkinEvent.Raise();
            else
                // Player is already using a new skin, take player to the next level
                m_NextLevelEvent.Raise();
        }

        private void ShowCompletedContainer(bool show)
        {
            m_CompletedContainer.gameObject.SetActive(show);
        }

        private void ShowContinueWithPassportButton(bool show)
        {
            m_ContinuePassportButton.gameObject.SetActive(show);
        }

        private void ShowNextButton(bool show)
        {
            m_NextButton.gameObject.SetActive(show);
        }

        private void ShowLoading(bool show)
        {
            m_Loading.gameObject.SetActive(show);
        }

        private void ShowError(bool show)
        {
            m_ErrorMessage.gameObject.SetActive(show);
            m_TryAgainButton.gameObject.SetActive(show);
        }

        private void DisplayCoins(int count)
        {
            count = Mathf.Clamp(count, 0, m_Coins.Length);

            if (m_Coins.Length > 0 && count >= 0 && count <= m_Coins.Length)
                for (var i = 0; i < m_Coins.Length; i++)
                    m_Coins[i].gameObject.SetActive(i < count);
        }
    }
}