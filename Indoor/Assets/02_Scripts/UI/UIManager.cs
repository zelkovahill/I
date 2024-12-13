using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("#2 컴포넌트")]
    [SerializeField] private QrCodeRecenter qrCodeRecenter;
    [SerializeField] private SetNavigationTarget setNavigationTarget;

    private Coroutine updateDistanceCoroutine;

    private void Awake()
    {
        Screen.fullScreen = false;
        QRCodeScanButton.onClick.AddListener(EnableQRCodeScanning);
    }

    private void EnableQRCodeScanning()
    {
        qrCodeRecenter.SetScanningState(true);
        ARNavGuidePanel.SetActive(false);
        Crosshair_Panel.SetActive(true);

        // 스캐닝 시작 시 거리 업데이트 중지
        if (updateDistanceCoroutine != null)
        {
            StopCoroutine(updateDistanceCoroutine);
        }
    }

    public void StartUpdatingDistanceUI()
    {
        if (updateDistanceCoroutine == null) // 코루틴이 실행 중이지 않을 때만 시작
        {
            updateDistanceCoroutine = StartCoroutine(UpdateDistanceUI());
        }
    }

    public void StopUpdatingDistanceUI()
    {
        if (updateDistanceCoroutine != null)
        {
            StopCoroutine(updateDistanceCoroutine);
            updateDistanceCoroutine = null;
        }
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
}
