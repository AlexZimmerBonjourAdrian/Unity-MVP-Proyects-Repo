using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CLevelDialogue : CLevelGeneric
{

   [SerializeField] private TextMeshProUGUI SanityText;
   [SerializeField] private TextMeshProUGUI EmpatyText;
   [SerializeField] private TextMeshProUGUI CharmText;
   [SerializeField] private TextMeshProUGUI WitsText;
   [SerializeField] private TextMeshProUGUI ComposureText;
   [SerializeField] private TextMeshProUGUI NameArquetipe;


   void Start()   
   {

      
       // Sistema de Rol eliminado
       
 

   }

   private void Update()
   {
       // Sistema de Rol eliminado - UI de stats deshabilitada
       if(EmpatyText != null && SanityText != null && CharmText != null && WitsText != null && ComposureText != null)
       {
           // UI de stats deshabilitada
       }
   }
}
   
