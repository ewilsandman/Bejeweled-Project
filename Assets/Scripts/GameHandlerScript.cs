using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameHandlerScript : MonoBehaviour
{
    private float[] PositionsX;
    private float[] PositionsY;
    private Dictionary<Vector2, GameObject> _getGemObject = new Dictionary<Vector2, GameObject>(); // name lol
    private List<GameObject> MarkedForDeath;
    private GameObject gem1;
    private GameObject gem2;
    
    public GameObject Canvas;
    public ScoreScript score;
    public GameObject BlueGem;
    public GameObject RedGem;
    public GameObject YellowGem;
    public GameObject OrangeGem;
    public GameObject PurpleGem;
    public GameObject GreenGem;
    // Start is called before the first frame update
    void Start()
    {
        // 
        MarkedForDeath = new List<GameObject>();
        PositionsX = new float[8];
        PositionsY = new float[8];
        float currentpositionX = transform.position.x;
        float currentpositionY = transform.position.y;
        // likely redundant
        for (int x = 0; x < 8; x++)
        {
            PositionsX[x] =  currentpositionX - currentpositionX / 3.5f * x;
        }
        for (int y = 0; y < 8; y++)
        {
            PositionsY[y] =  currentpositionY - currentpositionY / 3.5f * y;;
        }
        
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject newGem = Instantiate(GetRandomGem(), new Vector3(PositionsX[x], PositionsY[y], 0f), Quaternion.identity);
               // newGem.GetComponent<Button>().onClick.AddListener(HandleMove);
               GemPrefabScript newScript = newGem.GetComponent<GemPrefabScript>();
               Vector2 newVector = new Vector2(x, y);
               newScript.gameHandler = this;
               newScript.InternalPos = newVector;
               _getGemObject.Add(newVector, newGem);
            }
        }
        ScanGrid();
    }

    GameObject GetRandomGem()
    {
        int rand = Random.Range(1, 7);
        switch (rand)
        {
            case 1:
                return BlueGem;
           
            case 2:
                return RedGem;
                
            case 3:
                return YellowGem;
                
            case 4:
                return PurpleGem;
             
            case 5:
                return GreenGem;
             
            case 6:
                return OrangeGem;
        }

        return null;
    }

    private GameObject spawnNewGem()
    {
        return GetRandomGem();
    }

    public void HandleMove(GameObject gem)
    {
        if (gem1 == null)
        {
            gem1 = gem;
        }
        else // does not check if valid move yet, might be possible via checking if close in number
        {
            gem2 = gem;
            Vector3 temp1 = gem1.transform.position;
            Vector2 tempInternal1 = gem1.GetComponent<GemPrefabScript>().InternalPos;
            Vector3 temp2 = gem2.transform.position;
            Vector2 tempInternal2 = gem2.GetComponent<GemPrefabScript>().InternalPos;
            _getGemObject[tempInternal1] = gem2;
            _getGemObject[tempInternal2] = gem1;
            gem1.transform.position = temp2;
            gem2.transform.position = temp1;
            gem1 = null;
            gem2 = null;
            ScanGrid();
        }
    }

    public void ScanGrid() // time for spaghetti
    {
        for (int x = 0; x < 8; x++) // intentional
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject CentralObject = _getGemObject[new Vector2(x, y)];
                if (x != 0 && x != 7)
                {
                    if (_getGemObject[new Vector2(x - 1, y)].name == CentralObject.name)
                    {
                        if (_getGemObject[new Vector2(x + 1, y)].name == CentralObject.name)
                        {
                            CentralObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                            _getGemObject[new Vector2(x + 1, y)].GetComponent<SpriteRenderer>().color = Color.magenta;
                            _getGemObject[new Vector2(x - 1, y)].GetComponent<SpriteRenderer>().color = Color.magenta;
                            MarkedForDeath.Add(_getGemObject[new Vector2(x + 1, y)].gameObject);
                            MarkedForDeath.Add(_getGemObject[new Vector2(x - 1, y)].gameObject);
                            MarkedForDeath.Add(CentralObject);
                            
                        }
                    }
                }
                if (y != 0 && y != 7)
                {
                    if (_getGemObject[new Vector2(x, y - 1)].name == CentralObject.name)
                    {
                        if (_getGemObject[new Vector2(x, y + 1)].name == CentralObject.name)
                        {
                            CentralObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                            _getGemObject[new Vector2(x, y + 1)].GetComponent<SpriteRenderer>().color = Color.magenta;
                            _getGemObject[new Vector2(x, y - 1)].GetComponent<SpriteRenderer>().color = Color.magenta;
                            MarkedForDeath.Add(_getGemObject[new Vector2(x, y + 1)].gameObject);
                            MarkedForDeath.Add(_getGemObject[new Vector2(x, y - 1)].gameObject);
                            MarkedForDeath.Add(CentralObject);
                        }
                    }
                }
            }
        }
        foreach (var Obj in MarkedForDeath)
        {
            Destroy(Obj);
            score.Addscore(10);
        }
        MarkedForDeath.Clear();
    }
    
    private void OnDrawGizmos()
    {
       /* foreach (var X in PositionsX)
        {
            foreach (var Y in PositionsY)
            {
              Gizmos.DrawSphere(new Vector3(X,Y), 0.2f);
            }
        }*/
    }
}
