using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tanks.Complete
{
    // Handle a simple pause menu displaying the control and allowing to restart the game or quit it.
    public class PauseMenu : MonoBehaviour
    {
        public RectTransform m_PauseMenuRoot;           // Reference to the root Transform of the Pause Menu
        public RectTransform m_PauseMenuButtonsRoot;    // Reference to the root containing the button of the menu
        public Button m_ControlScreenButton;            // Reference to the button opening the control screen

        public RectTransform m_ControlMenuRoot;         // Reference to the root of the Control Screen
        public Button m_ControlMenuBackButton;          // Reference to the button that allow to go back to the Pause Menu from Control screen

        public Button m_SelectTankButton;               // Reference to the button that go back to tank selection
        public Button m_QuitButton;                     // Reference to the button that quit the Game

        public void Init()
        {
            if (m_ControlMenuBackButton != null)
            {
                m_ControlMenuBackButton.onClick.AddListener(() =>
                {
                    if (m_ControlMenuRoot != null) m_ControlMenuRoot.gameObject.SetActive(false);
                    if (m_PauseMenuButtonsRoot != null) m_PauseMenuButtonsRoot.gameObject.SetActive(true);
                });
            }

            if (m_PauseMenuButtonsRoot != null)
                m_PauseMenuButtonsRoot.gameObject.SetActive(false);

            if (m_ControlScreenButton != null)
            {
                m_ControlScreenButton.onClick.AddListener(() =>
                {
                    if (m_ControlMenuRoot != null) m_ControlMenuRoot.gameObject.SetActive(true);
                    if (m_PauseMenuButtonsRoot != null) m_PauseMenuButtonsRoot.gameObject.SetActive(false);
                });
            }

            if (m_SelectTankButton != null)
            {
                m_SelectTankButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                });
            }

            if (m_QuitButton != null)
            {
                if (Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor)
                {
                    m_QuitButton.gameObject.SetActive(false);
                }
                else
                {
                    m_QuitButton.gameObject.SetActive(true);
                    m_QuitButton.onClick.AddListener(Application.Quit);
                }
            }

            if (m_PauseMenuRoot != null) m_PauseMenuRoot.gameObject.SetActive(false);
            if (m_PauseMenuButtonsRoot != null) m_PauseMenuButtonsRoot.gameObject.SetActive(true);
        }

        public void TogglePause()
        {
            if (m_PauseMenuRoot != null)
            {
                // When toggling, swap the value of active for the pause menu root.
                bool state = !m_PauseMenuRoot.gameObject.activeSelf;
                m_PauseMenuRoot.gameObject.SetActive(state);

                // set the time scale to 0.0f, which is a simple way of "pausing" the game as everything will return 0 as
                // delta time and nothing will move anymore.
                Time.timeScale = state ? 0.0f : 1.0f;

                if (m_ControlMenuRoot != null) m_ControlMenuRoot.gameObject.SetActive(false);
                if (m_PauseMenuButtonsRoot != null) m_PauseMenuButtonsRoot.gameObject.SetActive(true);
            }
        }
    }
}