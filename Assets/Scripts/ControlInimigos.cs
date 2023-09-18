using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ControlInimigos : MonoBehaviour
{
    float _checkTime;
    [SerializeField] float _timeLimit;

    public Transform[] _posIni;
    public Transform _grupo;
    public int _numberPos;

    void Start()
    {
    
    }

    
    void Update()
    { 
         _checkTime -= Time.deltaTime;
         if (_checkTime < 0)
         {
            InimigoOn1();    
            _checkTime = _timeLimit;
         }
    }

    void InimigoOn1()
    {
        GameObject bullet = InimigoPool_1.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            _numberPos = Random.Range(0, 3);

            bullet.transform.localPosition = _posIni[_numberPos].transform.position;

            bullet.transform.SetParent(_grupo);
            //bullet.transform.rotation = turret.transform.rotation;
            bullet.SetActive(true);
        }
    }
}
