// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class PoseWorldLandmarkListAnnotationController : AnnotationController<PoseLandmarkListAnnotation>
  {
    [SerializeField] private float _hipHeightMeter = 0.9f;
    [SerializeField] private Vector3 _scale = new Vector3(100, 100, 100);
    [SerializeField] private bool _visualizeZ = true;
    
    public float threshold = 0.3f;
    private IList<Landmark> _currentTarget;
    private IList<Landmark> _saveTarget;
    public event Action GreenLightRedLightFinsh = delegate { };
    public event Action EndByMove = delegate { };

    protected override void Start()
    {
      base.Start();
      transform.localPosition = new Vector3(0, _hipHeightMeter * _scale.y, 0);

    }

    public void DrawNow(IList<Landmark> target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawNow(LandmarkList target)
    {
      DrawNow(target?.Landmark);
    }

    public void DrawLater(IList<Landmark> target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    public void DrawLater(LandmarkList target)
    {
      DrawLater(target?.Landmark);
    }

    protected override void SyncNow()
    {
      // foreach(var target in _currentTarget)
      // {
      //   Debug.Log(target);
      // }
      isStale = false;
      annotation.Draw(_currentTarget, _scale, _visualizeZ);
    }


    #region Player_Move_Check
      
    public void StartRedLight()
    {
      _saveTarget = _currentTarget;
      StartCoroutine(RedLight());
    }

    public IEnumerator RedLight()
    {
      float time = 2f;

      while(time > 0)
      {
        time -= Time.deltaTime;
        for(int i = 0, cnt = _saveTarget.Count; i < cnt; i++)
        {
          try
            {

            Vector3 current2save = new Vector3(_saveTarget[i].X - _currentTarget[i].X, _saveTarget[i].Y - _currentTarget[i].Y, _saveTarget[i].Z - _currentTarget[i].Z);
            if(current2save.magnitude > threshold)
            {
              EndByMove();
            }
          } catch (Exception e)
          {
            ;
          }
        }
        yield return null;
      }
      Debug.Log("KKH Finish");
      GreenLightRedLightFinsh();
    }

    void Update()
    {
      if(Input.anyKey)
      {
        Debug.Log("KKH Press");
        StartRedLight();
      }
    }
    #endregion Player_Move_Check

  }
}
