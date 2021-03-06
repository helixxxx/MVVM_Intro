﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using MVVM_Intro.Command;
using MVVM_Intro.FileHandler;
using MVVM_Intro.Helpers;
using MVVM_Intro.Model;

namespace MVVM_Intro.ViewModel
{

  /**
   * 
   * This class holds information about the viewmodels and controls of the mainwindow
   * 
   **/

  public class MainWindowViewModel : BaseViewModel
  {


    public CustomersViewModel CustomersViewModel { get; set; }
    public TextBoxViewModel TextBoxViewModel { get; set; }

    public ActionCommand LoadCommand { get; set; }
    public ActionCommand SaveCommand { get; set; }
    public ActionCommand EditCommand { get; set; }



    private bool _canEditControl;
    public bool CanEditControl
    {
      get { return _canEditControl; }
      set
      {
        if(value != _canEditControl)
        {
          _canEditControl = value;
          OnPropertyChanged(nameof(CanEditControl));

        }
      }
    }


    private bool _editIsChecked;
    public bool EditIsChecked
    {
      get { return _editIsChecked; }
      set
      {
        if (value != _editIsChecked)
        {
          _editIsChecked = value;
          OnPropertyChanged(nameof(EditIsChecked));

        }
      }
    }


    private bool _canLoadControl;
    public bool CanLoadControl
    {
      get { return _canLoadControl; }
      set
      {
        if (value != _canLoadControl)
        {
          _canLoadControl = value;
          OnPropertyChanged(nameof(CanLoadControl));
        }
      }
    }

    /* If any changes have been made to any of the UI fields */
    private bool _changesDetected;
    public bool ChangesDetected
    {
      get { return _changesDetected; }
      set
      {
        if(value != _changesDetected)
        {
          _changesDetected = value;
          OnPropertyChanged(nameof(ChangesDetected));
        }
      }
    }



    // Mandatory constructor
    public MainWindowViewModel() { }

    private MainModel mm;
    public MainWindowViewModel(MainModel mm)
    {

      this.mm = mm;

      CustomersViewModel = new CustomersViewModel(mm.Customers);
      CustomersViewModel.PropertyChanged += CustomersViewModel_PropertyChanged;

      TextBoxViewModel = new TextBoxViewModel(mm.TextBoxContent);
      TextBoxViewModel.PropertyChanged += TextBoxViewModel_PropertyChanged;

      

     // Per default one cannot press the buttons
      CanLoadControl = false;
      ChangesDetected = true;
      CanEditControl = true;
     

      /* Register whenever notifypropertychanged is called, RaiseCanExecuteChanged will be called on the all registered commands aswell*/
      RegisterCommand(LoadCommand = new ActionCommand(Load, CanLoad));
      RegisterCommand(SaveCommand = new ActionCommand(Save, CanSave));
      RegisterCommand(EditCommand = new ActionCommand(EditMode, CanEdit));
      

    }



    private void CustomersViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {


      //MessageBox.Show(e.PropertyName);

      // Avoid multiple raise
      if (!ChangesDetected)
      {
        ChangesDetected = true;
      }

    }

    private void TextBoxViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      ChangesDetected = true;
    }

    public void LoadValues(MainModel mm)
    {
      CustomersViewModel.LoadValues(mm.Customers);
      TextBoxViewModel.LoadValues(mm.TextBoxContent);

      ChangesDetected = false;
    }

    public void Load()
    {

      MainModel mainModel = new MainModel(StaticResources.DBPath);

      LoadValues(mainModel);

      MessageBox.Show("Values loaded:" );

      CanLoadControl = false;
   
    }


    public void EditMode()
    {
      // Enter edit mode
      CustomersViewModel.EditMode(EditIsChecked);
      // Do not activate save button when edit mode is entered
      ChangesDetected = false;
    }

    public void Save()
    {
      ///* Xml Serilizer to write data to an existing txt file */
      //XmlSerializer x = new XmlSerializer(typeof(MainModel));
      //if (!string.IsNullOrWhiteSpace(StaticResources.DBPath) && File.Exists(StaticResources.DBPath))
      //{
      //  using (TextWriter tw = new StreamWriter(StaticResources.DBPath))
      //  {
      //    // Update main model object with new values from textboxes
      //    mm.Customers = CustomersViewModel.SaveValues();
      //    mm.TextBoxContent = TextBoxViewModel.TextBox; 

      //    x.Serialize(tw, mm);
      //  }
      //  MessageBox.Show("Values saved");
      //}

      //EditIsChecked = false;
      //CanLoadControl = false;
      //ChangesDetected = false;


      
    }

    // Properties used to check wether a button is active or not
    public bool CanLoad() { return CanLoadControl; }
    public bool CanSave() { return ChangesDetected; }
    public bool CanEdit() { return CanEditControl; }



  }
}
