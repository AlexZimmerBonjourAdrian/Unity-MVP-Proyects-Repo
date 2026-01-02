using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Draconiano.Items
{
    public class Items 

    {
        public int Id { get => m_Id; }

        public string Name { get { return m_Name; } }


        [SerializeField]
        private int m_Id;

        [SerializeField]
        private string m_Name;

        [SerializeField]
        private int m_CurrentStackSize;
    }
        
}


