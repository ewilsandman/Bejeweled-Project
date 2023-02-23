using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameHandlerScript : MonoBehaviour
{
    private float[] PositionsX;
    private float[] PositionsY;
    private Dictionary<Vector2, GameObject> _getGemObject = new Dictionary<Vector2, GameObject>();
    private List<GameObject> MarkedForDeath;
    private GameObject gem1;
    private GameObject gem2;
    
    public ScoreScript score;
    public GameObject BlueGem;
    public GameObject RedGem;
    public GameObject YellowGem;
    public GameObject OrangeGem;
    public GameObject PurpleGem;
    public GameObject GreenGem;

    private bool Stopped = false;
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

   /* private void ReplaceGem(GameObject ToReplace)
    {
       GameObject newGem = GetRandomGem();
       newGem.GetComponent<GemPrefabScript>().InternalPos = ToReplace.GetComponent<GemPrefabScript>().InternalPos;
       newGem.transform.position = ToReplace.transform.position;
    }*/

    public void HandleMove(GameObject gem)
    {
        Vector2 Internalpos1;
        Vector2 Internalpos2;
        if (gem1 == null)
        {
            gem1 = gem;
        }
        else if (gem1 == gem)
        {
            gem1 = null;
            gem2 = null;
        }   
        else
        {
            gem2 = gem;
            Internalpos1 = gem1.GetComponent<GemPrefabScript>().InternalPos;
            Internalpos2 = gem2.GetComponent<GemPrefabScript>().InternalPos;
            if (Math.Abs(Internalpos1.x - Internalpos2.x) < 1)
            {
                Debug.Log("yep thats beside");
                SwapPos(gem1,gem2); 
                gem1 = null;
                gem2 = null;
                ScanGrid(); 
            }
            else if (Math.Abs(Internalpos1.y - Internalpos2.y) < 1)
            {
                Debug.Log("yep thats above");
                SwapPos(gem1,gem2); 
                gem1 = null;
                gem2 = null;
                ScanGrid();
            }
            else
            {
                Debug.Log("nice try buddy");
                gem2 = null;
                gem1 = null;
            }
        }
    }

    public void ScanGrid()
    {
        Stopped = true;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                GameObject CentralObject = _getGemObject[new Vector2(x, y)];
                if (x != 0 && x != 7)
                {
                    if (_getGemObject[new Vector2(x - 1, y)].name == CentralObject.name)
                    {
                        if (_getGemObject[new Vector2(x + 1, y)].name == CentralObject.name)
                        {
                            Debug.Log("Horizontal match");
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
                        if (_getGemObject[new Vector2(x, y + 1)].name == CentralObject.name) // expensive to do string compare, could do tag compare
                        {
                            Debug.Log("Vertical match");
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
        List<GameObject> Moved = new List<GameObject>();
        List<GameObject> Shuffled = new List<GameObject>();
        
       /* foreach (var VARIABLE in MarkedForDeath)
        {
            Shuffled.Add(VARIABLE);
        }*/
       foreach (GameObject Doomed in MarkedForDeath)
       {
           _getGemObject.Remove(Doomed.GetComponent<GemPrefabScript>().InternalPos);
           Destroy(Doomed);
           score.Addscore(10);
       }
       MarkedForDeath.Clear();
       MarkedForDeath.TrimExcess();

       for (int y = 0; y < 8; y++)
       {
           for (int x = 0; x < 8; x++)
           {
               // Debug.Log("Scanning" + x + y);
               if (!_getGemObject.ContainsKey(new Vector2(x, y)))
               {
                   // Debug.Log("Found hole at" + x + y);
                   Stopped = false;
                   if (y == 7)
                   {
                       GameObject newGem = Instantiate(GetRandomGem(),
                           new Vector3(PositionsX[x], PositionsY[y], 0f), Quaternion.identity);
                       newGem.GetComponent<GemPrefabScript>().InternalPos = new Vector2(x, y);
                       newGem.GetComponent<GemPrefabScript>().gameHandler = this;
                       _getGemObject.Add(new Vector2(x, y), newGem);
                   }
                   else
                   {
                       GameObject possibleNew = null;
                       int AboveCursor = y;
                       for (AboveCursor = y; AboveCursor < 8; AboveCursor++)
                       {
                           if (_getGemObject.ContainsKey(new Vector2(x, AboveCursor)))
                           {
                               possibleNew = _getGemObject[new Vector2(x, AboveCursor)];
                               possibleNew.transform.position = new Vector3(PositionsX[x], PositionsY[y]);
                               _getGemObject.Remove(possibleNew.GetComponent<GemPrefabScript>().InternalPos);
                               possibleNew.GetComponent<GemPrefabScript>().InternalPos = new Vector2(x, y);
                               _getGemObject.Add(new Vector2(x, y), possibleNew);
                               break;
                           }
                       }

                       if (possibleNew == null)
                       {
                           GameObject newGem = Instantiate(GetRandomGem(),
                               new Vector3(PositionsX[x], PositionsY[y], 0f), Quaternion.identity);
                           newGem.GetComponent<GemPrefabScript>().InternalPos = new Vector2(x, y);
                           newGem.GetComponent<GemPrefabScript>().gameHandler = this;
                           _getGemObject.Add(new Vector2(x, y), newGem);
                       }
                   }
               }
           }
       }

       if (!Stopped)
       {
           ScanGrid();
       }
       
    }

   /* private void PrepareNextRound()
    {
        for (int x = 8; x > 0; x--)
        {
            for (int y = 8; y > 0; y--)
            {
                GameObject CentralObject = _getGemObject[new Vector2(x, y)];
                if (_getGemObject[new Vector2(x , y - 1)] == null)
                {
                    
                }
            }
        }
    }*/
   private void SwapPos(GameObject ob1, GameObject ob2)
   {
       Vector3 temp1 = ob1.transform.position;
       Vector2 tempInternal1 = ob1.GetComponent<GemPrefabScript>().InternalPos;
       Vector3 temp2 = ob2.transform.position;
       Vector2 tempInternal2 = ob2.GetComponent<GemPrefabScript>().InternalPos;
       
       _getGemObject.Remove(tempInternal1);
       _getGemObject.Remove(tempInternal2);
       _getGemObject.Add(tempInternal1, ob2);
       ob2.GetComponent<GemPrefabScript>().InternalPos = tempInternal1;
       _getGemObject.Add(tempInternal2, ob1);
       ob1.GetComponent<GemPrefabScript>().InternalPos = tempInternal2;
      
       
       ob1.transform.position = temp2;
       ob2.transform.position = temp1;
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
