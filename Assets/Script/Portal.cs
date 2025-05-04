using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("ī�޶� ��� (���� ��ǥ)")]
    public Vector2 newMinCameraPos;
    public Vector2 newMaxCameraPos;

    [Header("������ Ȱ��ȭ")]
    public bool deadZoneEnabled = true;

    [Header("��� ��Ż��")]
    [Tooltip("�÷��̾ ��Ż�� ��ȣ�ۿ� �� �̵��� ��� ��Ż���� Transform ����Ʈ")]
    public Transform[] destinationPortals;

    [Header("��ȣ�ۿ� UI")]
    public GameObject ITKey;

    [Header("ī�޶� �� ���̵� ����")]
    public float cameraZoom = 1.0f;
    public float fadeOutTime = 0.3f;
    public float fadeHoldTime = 1.0f;
    public float fadeInTime = 0.3f;

    private bool playerInRange = false;
    private CameraFollowShake CamFol;
    private bool nextMove = false;

    private void Start()
    {
        CamFol = CameraFollowShake.Instance;
        if (CamFol == null)
            Debug.LogError("No CameraFollowShake.Instance found in scene!");
        if (ITKey != null)
            ITKey.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (ITKey != null)
                ITKey.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (ITKey != null)
                ITKey.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKey(KeyCode.F))
        {
            StartCoroutine(PortalInteract());
        }
    }

    private void FixedUpdate()
    {
        if (nextMove)
        {
            CamFol.deadZoneEnabled = deadZoneEnabled;
            nextMove = false;
        }
    }

    private IEnumerator PortalInteract()
    {
        // ��� ��Ż �迭 Ȯ��
        if (destinationPortals == null || destinationPortals.Length == 0)
        {
            Debug.LogWarning("No destination portals assigned!");
            yield break;
        }

        // ���̵� ȿ�� ����
        CamFol.FadeOutThenIn(fadeOutTime, fadeHoldTime, fadeInTime, Color.black);
        yield return new WaitForSeconds(fadeOutTime);

        // �÷��̾� �����̵� - ���� ����
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            int idx = Random.Range(0, destinationPortals.Length);
            Transform dest = destinationPortals[idx];
            if (dest != null)
                player.transform.position = dest.position;
        }

        // ī�޶� ��� �� �� ����
        CamFol.SetCameraBorder(newMinCameraPos, newMaxCameraPos);
        nextMove = true;

        // ���̵� Ȧ�� �ð� ���
        yield return new WaitForSeconds(fadeHoldTime);
    }
}