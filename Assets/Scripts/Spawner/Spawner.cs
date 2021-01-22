using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform _container;
    [SerializeField] private int _repeatCount;
    [SerializeField] private int _distanceBetweenFullLine;
    [SerializeField] private int _distanceBetweenRandomLine;
    [Header("Block")]
    [SerializeField] private Block _blockTemplate;
    [SerializeField] private int _blockSpawnChance;
    [Header("Wall")]
    [SerializeField] private Wall _wallTemplate;
    [SerializeField] private int _wallSpawnChance;
    [Header("Bonus")]
    [SerializeField] private Bonus _bonusTemplate;
    [SerializeField] private int _bonusSpawnChance;
    [Header("Side Wall")]
    [SerializeField] private Transform _leftSideWall;
    [SerializeField] private Transform _rightSideWall;

    private BlockSpawnPoint[] _blockSpawnPoints;
    private WallSpawnPoint[] _wallSpawnPoints;
    private BonusSpawnPoint[] _bonusSpawnPoints;

    private void Start()
    {
        _blockSpawnPoints = GetComponentsInChildren<BlockSpawnPoint>();
        _wallSpawnPoints = GetComponentsInChildren<WallSpawnPoint>();
        _bonusSpawnPoints = GetComponentsInChildren<BonusSpawnPoint>();

        for (int i = 0; i < _repeatCount; i++)
        {
            MoveSpawner(_distanceBetweenFullLine);
            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _distanceBetweenFullLine, _distanceBetweenFullLine / 2f);

            GenerateFullLine(_blockSpawnPoints, _blockTemplate.gameObject);
            MoveSpawner(_distanceBetweenRandomLine);

            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _distanceBetweenRandomLine, _distanceBetweenRandomLine / 2f);

            GenerateRandomElements(_blockSpawnPoints, _blockTemplate.gameObject, _blockSpawnChance, 0.15f);

            GenerateRandomElements(_bonusSpawnPoints, _bonusTemplate.gameObject, _bonusSpawnChance, 0.23f);
        }

        GenerateSideWall(_leftSideWall.position);
        GenerateSideWall(_rightSideWall.position);
    }

    private void GenerateFullLine(SpawnPoint[] spawnPoints, GameObject generateElement)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GenerateElement(spawnPoints[i].transform.position, generateElement);
        }
    }

    private void GenerateRandomElements(SpawnPoint[] spawnPoints, GameObject generateElement, int spawnChance, float scaleY = 1, float offsetY = 0)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Random.Range(0, 100) < spawnChance)
            {
                GameObject element = GenerateElement(spawnPoints[i].transform.position, generateElement, offsetY);
                element.transform.localScale = new Vector3(element.transform.localScale.x, scaleY, element.transform.localScale.z);
            }
        }
    }

    private GameObject GenerateElement(Vector3 spawnPoint, GameObject generatedElement, float offsetY = 0)
    {
        spawnPoint.y -= offsetY;
        return Instantiate(generatedElement, spawnPoint, Quaternion.identity);
    }

    private void MoveSpawner(int distanceY)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + distanceY, transform.position.z);
    }

    private void GenerateSideWall(Vector3 spawnPoint)
    {
        float height = CalculateHeight();
        GameObject element = Instantiate(_wallTemplate.gameObject, new Vector3(spawnPoint.x, height / 2f, spawnPoint.y), Quaternion.identity);
        element.transform.localScale = new Vector3(element.transform.localScale.x, height, element.transform.localScale.z);
    }

    private float CalculateHeight()
    {
        return (_distanceBetweenRandomLine + _distanceBetweenFullLine) * _repeatCount;
    }
}
