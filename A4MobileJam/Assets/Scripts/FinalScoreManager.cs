using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreManager : MonoBehaviour
{
    [SerializeField] GameObject _holder;
    [SerializeField] GameObject _playerScorePrefab;
    [SerializeField] VerticalLayoutGroup _verticalBoxHolder;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowScore(List<Player> pList)
    {
        _holder.SetActive(true);

        var l = pList.OrderByDescending(i => i.Points);

        for (int i = 0; i < l.Count(); i++)
        {
            Player el = l.ElementAt(i);
            PlayerScorePrefabData pS = Instantiate(_playerScorePrefab, _verticalBoxHolder.transform).transform.GetComponent<PlayerScorePrefabData>();
            pS.SetData(new PSPData
            (
                el.Img,
                el.Name,
                el.Points,
                i+1
            ));
        }
    }
}
