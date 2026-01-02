using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

namespace Draconiano_PC
{
    public class CPlayer : CController
    {

        private Animator _Anim;
        private SpriteRenderer _Renderer;
        // Start is called before the first frame update
        void Start()
        {
            _Anim = GetComponent<Animator>();
            _Renderer = GetComponent<SpriteRenderer>();
        }

        public override void Move()
        {
            base.Move();

            if (_Move.x < 0)
            {
                _Renderer.flipX = true;
                _Anim.SetBool("IsRun", true);
            }
            else if (_Move.x > 0)
            {
                _Renderer.flipX = false;
                _Anim.SetBool("IsRun", true);
            }
            else
            {
                _Anim.SetBool("IsRun", false);
            }
            if (_Move.y < 0)
            {
                _Anim.SetBool("IsRun", true);
            }
            else if (_Move.y > 0)
            {
                _Anim.SetBool("IsRun", true);
            }
            else
            {
                _Anim.SetBool("IsRun", false);
            }
        }
        // Update is called once per frame
    }
}

//namespace Draconiano_Android
//{
//    public class CPlayer : CController
//    {

//        private Animator _Anim;
//        private SpriteRenderer _Renderer;
//        // Start is called before the first frame update
//        void Start()
//        {
//            _Anim = GetComponent<Animator>();
//            _Renderer = GetComponent<SpriteRenderer>();

//        }

//        public override void Move()
//        {
//            base.Move();

//            if (_Move.x < 0)
//            {
//                _Renderer.flipX = true;
//                _Anim.SetBool("IsRun", true);
//            }
//            else if (_Move.x > 0)
//            {
//                _Renderer.flipX = false;
//                _Anim.SetBool("IsRun", true);
//            }
//            else
//            {
//                _Anim.SetBool("IsRun", false);
//            }
//            if (_Move.y < 0)
//            {
//                _Anim.SetBool("IsRun", true);
//            }
//            else if (_Move.y > 0)
//            {
//                _Anim.SetBool("IsRun", true);
//            }
//            else
//            {
//                _Anim.SetBool("IsRun", false);
//            }
//        }

//        // Update is called once per frame


//    }
//}
