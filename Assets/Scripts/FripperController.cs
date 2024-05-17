﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FripperController : MonoBehaviour
{
    private struct InputKeyFripper
    {
        private string tag;
        public string Tag
        {
            private set { this.tag = value; }
            get { return this.tag; }
        }
        private List<KeyCode> keyCodes;

        public InputKeyFripper(string tag, KeyCode[] keyCodes)
        {
            this.tag = tag;
            this.keyCodes = new List<KeyCode>(keyCodes);
            for (int i = 0; i < keyCodes.Length; i++) this.keyCodes.Add(keyCodes[i]);
        }

        public bool IsKeyFripperOn()
        {
            for(int i = 0; i < this.keyCodes.Count; i++) if (Input.GetKeyDown(this.keyCodes[i])) return true;
            return false;
        }
        public bool IsKeyFripperOff()
        {
            for (int i = 0; i < this.keyCodes.Count; i++) if (Input.GetKeyUp(this.keyCodes[i])) return true;
            return false;
        }
    }
    //HingeJointコンポーネント
    private HingeJoint myHingeJoint;
    //初期の傾き
    private float defaultAngle = 20;
    //弾いた時の傾き
    private float flickAngle = -20;
    private InputKeyFripper leftKeyFripper;
    private InputKeyFripper rightKeyFripper;
    private enum FripperTag { LeftFripperTag, RightFripperTag }
    private KeyCode[] leftKeys = { KeyCode.LeftArrow, KeyCode.A, KeyCode.S};
    private KeyCode[] rightKeys = { KeyCode.RightArrow, KeyCode.D, KeyCode.S};

    // Start is called before the first frame update
    void Start()
    {
        //HingeJointコンポーネント取得
        this.myHingeJoint = GetComponent<HingeJoint>();
        //フリッパーの傾きを設定
        SetAngle(this.defaultAngle);

        leftKeyFripper = new InputKeyFripper(FripperTag.LeftFripperTag.ToString(), leftKeys);
        rightKeyFripper = new InputKeyFripper(FripperTag.RightFripperTag.ToString(), rightKeys);
    }

    // Update is called once per frame
    void Update()
    {
        //左矢印キーを押した時左フリッパーを動かす
        SetFripper(leftKeyFripper.IsKeyFripperOn(), leftKeyFripper.Tag, this.flickAngle);
        //右矢印キーを押した時右フリッパーを動かす
        SetFripper(rightKeyFripper.IsKeyFripperOn(), rightKeyFripper.Tag, this.flickAngle);

        //矢印キーが離された時フリッパーを元に戻す
        SetFripper(leftKeyFripper.IsKeyFripperOff(), leftKeyFripper.Tag, this.defaultAngle);
        SetFripper(rightKeyFripper.IsKeyFripperOff(), rightKeyFripper.Tag, this.defaultAngle);
    }

    private void SetFripper(bool isActive, string tag, float angle)
    {
        if (!isActive || tag != this.tag) return;
        SetAngle(angle);
    }

    //フリッパーの傾きを設定
    public void SetAngle(float angle)
    {
        JointSpring jointSpr = this.myHingeJoint.spring;
        jointSpr.targetPosition = angle;
        this.myHingeJoint.spring = jointSpr;
    }
}
