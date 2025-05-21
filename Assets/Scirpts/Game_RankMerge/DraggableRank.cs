using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class DraggableRank : MonoBehaviour
{
    public int rankLevel = 1;               //����� ����
    public float dragSpeed = 10f;           //�巡�׽� �̵� �ӵ�
    public float snapBackSpeed = 20f;       //����ġ�� ���ư��� �ӵ�

    public bool isDragging = false;         //���� �巡�� ������
    public Vector3 originalPosition;        //���� ��ġ
    public GridCell currentCell;            //���� ��ġ�� ĭ

    public Camera mainCamera;               //���� ī�޶�
    public Vector3 dragOffset;              //�巡�� �� ������(������)
    public SpriteRenderer spriteRenderer;   //����� �̹��� ������
    public GameManager gameManager;         //���� �Ŵ���

    private void Awake()
    {
        //�ʿ��� ������Ʈ ��������
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)     //�巡�� ���̸� ���콺 ���� �̵�
        {
            Vector3 targetPosition = GetMouseWorldPosition() + dragOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, dragSpeed * Time.deltaTime);
        }
        else if(transform.position != originalPosition && currentCell != null)      //�巡�װ� �����µ� ���� ��ġ�� ���ư��� �ϴ°��
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, snapBackSpeed * Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        StopDragging();
    }

    void StartDragging()        //�巡�� ����
    {
        isDragging = true;      //�巡�� ���·� ����
        dragOffset = transform.position - GetMouseWorldPosition();  //���콺�� ����� ��ġ�� ���� ���
        spriteRenderer.sortingOrder = 10;                           //�巡�� ���� �� ������� ������ ������
    }

    void StopDragging()
    {
        isDragging = false;
        spriteRenderer.sortingOrder = 1;
        GridCell targetCell = gameManager.FindClosestCell(transform.position);

        if(targetCell != null)
        {
            if(targetCell.currentRank == null)      //�� ĭ�� ���
            {
                MoveToCell(targetCell);
            }
            else if(targetCell.currentRank != this && targetCell.currentRank.rankLevel == rankLevel)        //���� ��ũ�� ���
            {
                MergeWithCell(targetCell);
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }
        else
        {
            ReturnToOriginalPosition();     //��ȿ�� ĭ�̾����� ��ġ�� ����
        }
    }

    public void MoveToCell(GridCell targetCell)     //Ư�� ĭ���� �̵�
    {
        if (currentCell != null)
        {
            currentCell.currentRank = null;         //���� ĭ���� ����
        }

        currentCell = targetCell;                   //�� ĭ���� �̵�
        targetCell.currentRank = this;

        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0f);
        transform.position = originalPosition;
    }

    public void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
    }

    public void MergeWithCell(GridCell targetCell)
    {
        if(targetCell.currentRank == null || targetCell.currentRank.rankLevel !=rankLevel)
        {
            ReturnToOriginalPosition();     //������ġ�� ���ư���
            return;
        }

        if (currentCell != null)
        {
            currentCell.currentRank = null;     //����ĭ��������
        }
        //��ġ�� ���� MergeRanks �Լ��� ���ؼ� ����

        gameManager.MergeRanks(this, targetCell.currentRank);
    }
    public Vector3 GetMouseWorldPosition()      //���콺 ���� ��ǥ ���ϱ�
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    public void SetRankLevel(int level)
    {
        rankLevel = level;

        if(gameManager != null && gameManager.rankSprites.Length > level -1)
        {
            spriteRenderer.sprite = gameManager.rankSprites[level - 1];     //������ �´� ��������Ʈ ����
        }
    }
}
