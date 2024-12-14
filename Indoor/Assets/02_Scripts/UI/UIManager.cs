using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("#1 UI 요소")]
    [SerializeField] private GameObject ARNavGuidePanel;
    [SerializeField] private Button QRCodeScanButton;
    [SerializeField] private GameObject DistancePanel;
    [SerializeField] private TextMeshProUGUI pathDistanceText;
    [SerializeField] private GameObject MiniMap_Panel;
    [SerializeField] private GameObject Crosshair_Panel;
    [SerializeField] private GameObject NavigationTagetDropdown;
    [SerializeField] private GameObject ArrivePlacePanel;
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject DebugPanel;
    [SerializeField] private GameObject SearchPanel;
    [SerializeField] private Button CountinueButton;
    [SerializeField] private Button DebugButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button MenuButton;
    [SerializeField] private Button GameExit2Button;

    [Header("#2 컴포넌트")]
    [SerializeField] private QrCodeRecenter qrCodeRecenter;
    [SerializeField] private SetNavigationTarget setNavigationTarget;

    private Coroutine updateDistanceCoroutine;

    private void Awake()
    {
        Screen.fullScreen = false;


        GameExit2Button.onClick.AddListener(ExitApp);
        MenuButton.onClick.AddListener(MainMenuPanelButtonClicked);
        DebugButton.onClick.AddListener(DebugButtonClicked);
        QRCodeScanButton.onClick.AddListener(EnableQRCodeScanning);
        CountinueButton.onClick.AddListener(SetActiveFalseMainMenuPanel);
        ExitButton.onClick.AddListener(ExitApp);
    }

    private void EnableQRCodeScanning()
    {
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        qrCodeRecenter.SetScanningState(true);
        ARNavGuidePanel.SetActive(false);
        Crosshair_Panel.SetActive(true);
        GameExit2Button.gameObject.SetActive(true);

        // 스캐닝 시작 시 거리 업데이트 중지
        if (updateDistanceCoroutine != null)
        {
            StopCoroutine(updateDistanceCoroutine);
        }
    }

    public void SetActiveSearchPanel(bool isActive)
    {
        SearchPanel.SetActive(isActive);
    }

    private void MainMenuPanelButtonClicked()
    {
        MainMenuPanel.SetActive(!MainMenuPanel.activeSelf);
    }

    private void DebugButtonClicked()
    {
        DebugPanel.SetActive(!DebugPanel.activeSelf);
    }

    public void StartUpdatingDistanceUI()
    {
        if (updateDistanceCoroutine == null) // 코루틴이 실행 중이지 않을 때만 시작
        {
            updateDistanceCoroutine = StartCoroutine(UpdateDistanceUI());
        }
    }

    public void SetActivemainMenuButton(bool isActive)
    {
        MenuButton.gameObject.SetActive(isActive);
    }

    public void SetActiveTrueMainMenuPanel()
    {
        MainMenuPanel.SetActive(true);
    }

    public void SetActiveFalseMainMenuPanel()
    {
        MainMenuPanel.SetActive(false);
    }

    public void StopUpdatingDistanceUI()
    {
        if (updateDistanceCoroutine != null)
        {
            StopCoroutine(updateDistanceCoroutine);
            updateDistanceCoroutine = null;
        }
    }

    public void SetActiveArrivePlacePanel(bool isActive)
    {
        ArrivePlacePanel.SetActive(isActive);
    }

    public void SetActiveDistancePanel(bool isActive)
    {
        DistancePanel.SetActive(isActive);
    }

    public void SetActiveMiniMapPanel(bool isActive)
    {
        MiniMap_Panel.SetActive(isActive);
    }

    public void SetActiveCrosshairPanel(bool isActive)
    {
        Crosshair_Panel.SetActive(isActive);
    }

    public void SetActiveNavigationTagetDropdown(bool isActive)
    {
        NavigationTagetDropdown.SetActive(isActive);
    }

    private IEnumerator UpdateDistanceUI()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            float pathDistance = setNavigationTarget.GetPathDistance();
            pathDistanceText.text = $"{(int)pathDistance}m";
        }
    }

    public void SetActiveExitButton2(bool isActive)
    {
        GameExit2Button.gameObject.SetActive(isActive);
    }

    private void ExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
