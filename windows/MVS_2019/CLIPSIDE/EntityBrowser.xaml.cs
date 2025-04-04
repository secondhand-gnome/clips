﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Drawing;

using CLIPSNET;

namespace CLIPSIDE
  {
   public class FactInstanceCollection : ObservableCollection<FactInstance>
     {
      public FactInstanceCollection(List<FactInstance> theEntityList) : base(theEntityList)
        {
        }
     }

   public class SlotValueCollection : ObservableCollection<SlotValue>
     {
      public SlotValueCollection(List<SlotValue> theSlotValues) : base(theSlotValues)
        {
        }
     }

   public partial class EntityBrowser : UserControl
     { 
      private MainWindow ide = null;
      private List<Module> modules;
      private List<FactInstance> entityList;
      private Dictionary<ulong,BitArray> scopes;
      private CollectionViewSource entitySourceList;
      private CollectionViewSource slotSourceList;
      private FactInstanceCollection entityCollection;
      private SlotValueCollection slotCollection;
      private string lastModule;
      private int lastModuleRow;
      private string lastEntity;
      private int lastEntityRow;

      /*****************/
      /* EntityBrowser */
      /*****************/
      public EntityBrowser()
        {
         InitializeComponent();
        }

      public EntityBrowser(MainWindow theMW) : this()
        {
         ide = theMW;
         if (ide != null)
           { 
            lastModule = null;
            lastModuleRow = -1;
            lastEntity = null;
            lastEntityRow = -1;
            SetFontFromPreferences();
           }
        }
                 
      /**************************/
      /* SetFontFromPreferences */
      /**************************/
      public void SetFontFromPreferences()
        { 
         if (ide == null) return;

         TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));
         Font theFont = (Font) fontConverter.ConvertFromString(ide.GetPreferences().GetBrowserFont());

         System.Windows.Media.FontFamily theFamily =  new System.Windows.Media.FontFamily(theFont.FontFamily.Name);
         double theSize =  theFont.Size;
         FontWeight theWeight = theFont.Bold ? FontWeights.Bold : FontWeights.Regular;
         System.Windows.FontStyle theStyle = theFont.Italic ? FontStyles.Italic : FontStyles.Normal;

         this.moduleDataGridView.FontFamily = theFamily;
         this.moduleDataGridView.FontSize = theSize;
         this.moduleDataGridView.FontWeight = theWeight;
         this.moduleDataGridView.FontStyle = theStyle;
            
         this.entityDataGridView.FontFamily = theFamily;
         this.entityDataGridView.FontSize = theSize;
         this.entityDataGridView.FontWeight = theWeight;
         this.entityDataGridView.FontStyle = theStyle;

         this.slotDataGridView.FontFamily = theFamily;
         this.slotDataGridView.FontSize = theSize;
         this.slotDataGridView.FontWeight = theWeight;
         this.slotDataGridView.FontStyle = theStyle;
        }

      /*************/
      /* DetachIDE */
      /*************/
      public void DetachIDE()
        {
        }
        
     /**************/
     /* UpdateData */
     /**************/
     public void UpdateData(
        List<Module> theModules,
        List<FactInstance> theEntityList,
        Dictionary<ulong,BitArray> theScopes)
        {
         Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                                new Action(delegate { AssignData(theModules,theEntityList,theScopes); }));
        }
       
      /**********************/
      /* CreateEntitySource */
      /**********************/
      private void CreateEntitySource()
        {
         entityCollection = new FactInstanceCollection(entityList);
         entitySourceList = new CollectionViewSource() { Source = entityCollection };

         entitySourceList.Filter += EntityFilter;

         entityDataGridView.ItemsSource = entitySourceList.View;
        }  

      /************************/
      /* CreateSlotListSource */
      /************************/
      private void CreateSlotListSource(
        FactInstance theEntity)
        {
         slotCollection = new SlotValueCollection(theEntity.GetSlotValues());
         slotSourceList = new CollectionViewSource() { Source = slotCollection };

         slotSourceList.Filter += SlotFilter;

         slotDataGridView.ItemsSource = slotSourceList.View;
        }  
      
      /**************/
      /* AssignData */
      /**************/
      private void AssignData(
        List<Module> theModules,
        List<FactInstance> theEntityList,
        Dictionary<ulong,BitArray> theScopes)
        {
         SaveSelection();

         modules = theModules;
         entityList = theEntityList;
         scopes = theScopes;

         moduleDataGridView.ItemsSource = theModules;

         CreateEntitySource();
 
         RestoreSelection();
        }

      /*****************/
      /* SaveSelection */
      /*****************/
      private void SaveSelection()
        {
         int theRow;
   
         theRow = moduleDataGridView.SelectedIndex;
         if (theRow != -1)
           {
            lastModuleRow = theRow;
         
            Module theModule = moduleDataGridView.Items[theRow] as Module;
            lastModule = theModule.ModuleName;
           }
         else
           {
            lastModule = null;
            lastModuleRow = -1;
           }

         theRow = entityDataGridView.SelectedIndex;
         if (theRow != -1)
           {
            lastEntityRow = theRow;

            FactInstance theEntity = entityDataGridView.Items[theRow] as FactInstance;
            lastEntity = theEntity.Name;
           }
         else
           {
            lastEntity = null;
            lastEntityRow = -1;
           }
        }
      
      /********************/
      /* RestoreSelection */
      /********************/
      private void RestoreSelection()
        {
         int i, count;
         bool found;
         Module theModule;
         FactInstance theEntity;

         moduleDataGridView.SelectedItem = null;
         entityDataGridView.SelectedItem = null;

         if (lastModuleRow == -1)
           {
            if (moduleDataGridView.Items.Count > 0)
              {
               theModule = moduleDataGridView.Items[0] as Module;
               moduleDataGridView.SelectedItem = theModule; 
              }
           }
         else
           {
            count = moduleDataGridView.Items.Count;
            found = false;
         
            if (lastModuleRow < count)
              {
               theModule = moduleDataGridView.Items[lastModuleRow] as Module;

               if (theModule.ModuleName.Equals(lastModule))
                 {
                  moduleDataGridView.SelectedItem = theModule;
                  found = true;
                 }
              }
           
            if (! found)
              {
               for (i = 0; i < count; i++)
                 {
                  theModule = moduleDataGridView.Items[i] as Module;

                  if (theModule.ModuleName.Equals(lastModule))       
                    {
                     found = true;
                     moduleDataGridView.SelectedItem = theModule; 
                     break;
                    }
                 }
              }
           
            if (! found)
              { 
               lastEntityRow = -1;
               lastEntity = null;
               if (count > 0)
                 {
                  if (lastModuleRow < count)
                    { 
                     theModule = moduleDataGridView.Items[lastModuleRow] as Module;
                     moduleDataGridView.SelectedItem = theModule;
                    }
                  else 
                    { 
                     theModule = moduleDataGridView.Items[count-1] as Module;
                     moduleDataGridView.SelectedItem = theModule;
                    }
                 }
              }
           }
        
         if (lastEntityRow == -1)
           { 
            if (entityDataGridView.Items.Count > 0)
              { 
               theEntity = entityDataGridView.Items[0] as FactInstance;
               entityDataGridView.SelectedItem = theEntity;  
              }
           }
         else
           {
            count = entityDataGridView.Items.Count;
            found = false;
         
            if (lastEntityRow < count)
              {
               theEntity = entityDataGridView.Items[lastEntityRow] as FactInstance;
                
               if (theEntity.Name.Equals(lastEntity))
                 {
                  entityDataGridView.SelectedItem = theEntity; 
                  found = true;
                 }
              }
           
            if (! found)
              {
               for (i = 0; i < count; i++)
                 {
                  theEntity = entityDataGridView.Items[i] as FactInstance;             

                  if (theEntity.Name.Equals(lastEntity))
                    {
                     found = true;
                     entityDataGridView.SelectedItem = theEntity; 
                     break;
                    }
                 }
              }
           
            if (! found)
              {
               if (count > 0)
                 {
                  if (lastEntityRow < count)
                    { 
                     theEntity = entityDataGridView.Items[lastEntityRow] as FactInstance;             
                     entityDataGridView.SelectedItem = theEntity;
                    }
                  else 
                    { 
                     theEntity = entityDataGridView.Items[count -1] as FactInstance;             
                     entityDataGridView.SelectedItem = theEntity;
                    }
                 }
              }
           }

         if (entityDataGridView.SelectedItem != null)
           {
            theEntity = entityDataGridView.SelectedItem as FactInstance;

            CreateSlotListSource(theEntity);

            if (slotDataGridView.Items.Count != 0)
              { 
               SlotValue theSlotValue = slotDataGridView.Items[0] as SlotValue;
               slotDataGridView.SelectedItem = theSlotValue; 
              }
           }
         else
           { 
            slotDataGridView.ItemsSource = null; 
            slotSourceList = null;
            entitySourceList = null;
            entityCollection = null;
            slotCollection = null;
           }

         lastModuleRow = -1;
         lastModule = null;
         lastEntity = null;
         lastEntityRow = -1;
        }

      /*****************/
      /* ModuleChanged */
      /*****************/
      private void ModuleChanged(object sender,SelectionChangedEventArgs e)
        {
         if (e.RemovedItems.Count != 1) return;
         if (e.AddedItems.Count != 1) return;

         /*===============================================*/
         /* Reapply the entity filter to display the list */
         /* of facts/instances for the selected module.   */
         /*===============================================*/

         if (entitySourceList == null) return;

         entitySourceList.Filter -= EntityFilter;
         entitySourceList.Filter += EntityFilter;

         ReselectEntityAndSlots();
        }

      /*****************/
      /* EntityChanged */
      /*****************/
      private void EntityChanged(object sender,SelectionChangedEventArgs e)
        {
         FactInstance theEntity;

         if (e.RemovedItems.Count != 1) return;
         if (e.AddedItems.Count != 1) return;

         theEntity = (FactInstance) e.AddedItems[0];
         
         CreateSlotListSource(theEntity);

         if (slotDataGridView.Items.Count != 0)
           {
            SlotValue theSV = slotDataGridView.Items[0] as SlotValue;
            slotDataGridView.SelectedItem = theSV;
           }
        }

      /****************/ 
      /* EntityFilter */
      /****************/ 
      private void EntityFilter(object sender, FilterEventArgs e)
        {
         FactInstance fact = e.Item as FactInstance;

         if (fact == null) return;

         int moduleIndex = moduleDataGridView.SelectedIndex;

         if (moduleIndex < 0) 
           { moduleIndex = 0; }

         BitArray theArray = scopes[fact.TypeAddress];

         if (theArray.Get(moduleIndex))
           { 
            if (fact.SearchForString(searchField.Text))
              { e.Accepted = true; }
            else
              { e.Accepted = false; }
           }
         else
           { e.Accepted = false; }
        }

      /**************/ 
      /* SlotFilter */
      /**************/ 
      private void SlotFilter(object sender, FilterEventArgs e)
        {
         SlotValue slot = e.Item as SlotValue;

         if (slot == null) return;

         if (! slot.IsDefault)
           { e.Accepted = true; }
         else if (displayDefaultsCheckBox.IsChecked == true)
           { e.Accepted = true; }
         else 
           { e.Accepted = false; }
        }

      /***********************************/ 
      /* DisplayDefaultedValuesUnchecked */
      /***********************************/ 
      private void DisplayDefaultedValuesUnchecked(object sender, RoutedEventArgs e)
        {
         if (slotSourceList == null) return;
         slotSourceList.Filter -= SlotFilter;
         slotSourceList.Filter += SlotFilter;
        }

      /*********************************/ 
      /* DisplayDefaultedValuesChecked */
      /*********************************/ 
      private void DisplayDefaultedValuesChecked(object sender, RoutedEventArgs e)
        {
         if (slotSourceList == null) return;
         slotSourceList.Filter -= SlotFilter;
         slotSourceList.Filter += SlotFilter;
        }
        
      /**************************/
      /* ReselectEntityAndSlots */
      /**************************/
      private void ReselectEntityAndSlots()
        {
         if (entityDataGridView.Items.Count != 0)
           {
            FactInstance theEntity;

            theEntity = entityDataGridView.Items[0] as FactInstance;

            entityDataGridView.SelectedItem = theEntity;

            CreateSlotListSource(theEntity);
            if (slotDataGridView.Items.Count != 0)
              {
               SlotValue theSV = slotDataGridView.Items[0] as SlotValue;
               slotDataGridView.SelectedItem = theSV;
              }
           }
         else
           {
            slotDataGridView.ItemsSource = null;
            slotSourceList = null;
            slotCollection = null;
           }
        }

      /********************/ 
      /* SearchFieldKeyUp */
      /********************/ 
      private void SearchFieldKeyUp(object sender, KeyEventArgs e)
        {
         if ((e.Key == System.Windows.Input.Key.Return) ||
             (e.Key == System.Windows.Input.Key.Enter))
           {
            if (entitySourceList != null)
              {
               entitySourceList.Filter -= EntityFilter;
               entitySourceList.Filter += EntityFilter;

               ReselectEntityAndSlots();
              }
           }
        }
     } 
  }
