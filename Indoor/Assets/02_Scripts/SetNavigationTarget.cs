using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class SetNavigationTarget : MonoBehaviour
{
    // [SerializeField] private Camera topDownCamera;
    // [SerializeField] private GameObject navTargetObject;

    [SerializeField] private TMP_Dropdown navigationTargetDropDown;
    [SerializeField] private List<Target> navigationTargetObjects = new List<Target>();
    [SerializeField] private Slider navigationYOffset;

    [SerializeField] private UIManager uiManager;

    private NavMeshPath path;
    private LineRenderer line;
    private Vector3 targetPosition = Vector3.zero;

    private int currentFloor = 1;

    private bool lineToggle = false;

    private void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;
    }

    private void Update()
    {
        if (lineToggle && targetPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            Vector3[] calculatedPathAndOffset = AddLineOffset();
            line.SetPositions(calculatedPathAndOffset);

            // // 거리 계산
            // float straightLineDistance = Vector3.Distance(transform.position, targetPosition);
            // float pathDistance = CalculatePathDistance(path);

            // Debug.Log($"직선 거리: {(int)straightLineDistance}m, 경로 거리: {(int)pathDistance}m");

            // 타겟 도달 여부 확인
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget < 1.0f) // 도달 거리 기준 설정 (예: 1.0m)
            {
                Debug.Log("타겟에 도달했습니다!");
                ToggleVisibility(); // 라인 비활성화
                targetPosition = Vector3.zero; // 타겟 초기화
            }
        }
    }

    public void SetCurrentNavigationTarget(int selecedValue)
    {
        uiManager.SetActiveDistancePanel(true);
        targetPosition = Vector3.zero;
        string selectedText = navigationTargetDropDown.options[selecedValue].text;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText.ToLower()));

        if (currentTarget != null)
        {
            if (!line.enabled)
            {
                ToggleVisibility();
            }

            targetPosition = currentTarget.PositionObject.transform.position;
        }
    }

    public float GetPathDistance()
    {
        if (path == null || path.corners.Length < 2)
        {
            return 0f; // 유효한 경로가 없을 경우 거리 0 반환
        }

        float totalDistance = 0f;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return totalDistance;
    }




    private float CalculatePathDistance(NavMeshPath path)
    {
        if (path.corners.Length < 2)
        {
            return 0f; // 코너가 2개 미만이면 유효한 경로가 아님
        }

        float totalDistance = 0f;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return totalDistance;
    }

    public void ToggleVisibility()
    {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;

        if (lineToggle) // 라인이 활성화될 때 거리 업데이트를 시작
        {
            uiManager.StartUpdatingDistanceUI();
        }
        else // 라인이 비활성화될 때 거리 업데이트를 중지
        {
            uiManager.StopUpdatingDistanceUI();
        }
    }

    public void ChangeActiveFloor(int floorNumber)
    {
        currentFloor = floorNumber;
        SetNavigationTargetDropDownOptions(currentFloor);
    }

    private Vector3[] AddLineOffset()
    {
        if (navigationYOffset.value == 0)
        {
            return path.corners;
        }

        Vector3[] calculatedLine = new Vector3[path.corners.Length];
        for (int i = 0; i < path.corners.Length; i++)
        {
            calculatedLine[i] = path.corners[i] + new Vector3(0, navigationYOffset.value, 0);
        }

        return calculatedLine;
    }

    private void SetNavigationTargetDropDownOptions(int floorNumber)
    {
        navigationTargetDropDown.ClearOptions();
        navigationTargetDropDown.value = 0;

        if (line.enabled)
        {
            ToggleVisibility();
        }
        if (floorNumber == 1)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("102"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("103"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("104"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("108"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("109"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("StartPoint"));
        }
        // if (floorNumber == 2)
        // {
        //     navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Song"));
        //     navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Kang"));
        // }
    }
}
