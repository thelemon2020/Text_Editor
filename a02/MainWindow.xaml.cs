/*
 * FILE : MainWindow.xaml.cs
 * PROJECT : PROG2121 - Assignment #2
 * PROGRAMMER : Chris Lemon
 * FIRST VERSION : 2020 - 09 - 20
 * LAST REVISION : 2020 - 09 - 25
 * DESCRIPTION : This file holds the class which outlines the functionality of the main window, including user input, opening, saving and starting new files.  
 *               It features a text box that the user can interact with, by typing in text.  The font can be changed in another window
 */
using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using System.Configuration;
using System.ComponentModel;
using System.IO;

namespace a02
{



    /*
    * NAME : MainWindow
    * PURPOSE : This class copies the functionality of a text editor, with a textbox for the user to interact with. 
    *            It containing the code to open, save and create files with command bindings.  
    *            It also keeps track of whether the file needs to be saved and the number of characters in the text box
    */
    public partial class MainWindow : Window
    {
        //class properties
        public bool isSaved { get; set; }
        public string openedFile { get; set; }

       /*
        * FUNCTION : MainWindow()
        *
        * DESCRIPTION : This is the constructor method for the MainWindow class.  Sets inital values for the properties
        *
        * PARAMETERS : None
        *
        * RETURNS : Nothing
        */
        public MainWindow()
        {
            isSaved = true;
            openedFile = null;
            InitializeComponent();
        }

        /*
       * FUNCTION : OnKeyUpHandler()
       *
       * DESCRIPTION : This method triggers whenever a key is depressed.  It indicates the file has changed, 
       *               meaning a save is valid now.  It also records the amount of characters typed and displays them.
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void OnKeyUpHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int charactersTyped = userInput.Text.Length;
            wordCount.Text = "Character Count: " + charactersTyped;
            isSaved = false;
        }


      /*
       * FUNCTION : openAbout_click()
       *
       * DESCRIPTION : This method triggers whenever the 'About' menu button is clicked.  It creates and shows a ModalAbout Window
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void openAbout_click(object sender, RoutedEventArgs e)
        {
            ModalAbout about = new ModalAbout();
            about.Owner = this;
            about.ShowDialog();
        }


        /*
         * FUNCTION : CommandBinding_CanExecute_New()
         *
         * DESCRIPTION : This method looks to see if the New command can be executed.  It always can be executed
         *
         * PARAMETERS : sender - the object that raised the event
         *              e - contains the relevant event data
         *
         * RETURNS : Nothing
         */
        private void CommandBinding_CanExecute_New(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        /*
         * FUNCTION : CommandBinding_Executed_New()
         *
         * DESCRIPTION : This method triggers when the user clicks the 'New' menu button.  It contains the logic to start a new file
         * 
         * PARAMETERS : sender - the object that raised the event
         *              e - contains the relevant event data
         *
         * RETURNS : Nothing
         */
        private void CommandBinding_Executed_New(object sender, ExecutedRoutedEventArgs e)
        {
            if (isSaved == false)
            {
                MessageBoxResult didSave = System.Windows.MessageBox.Show("Do you want to save your work", "Save Work", MessageBoxButton.YesNoCancel);
                if (didSave == MessageBoxResult.Yes) //starts the save process
                {
                    CommandBinding_Executed_SaveAs(sender, e); //calls the command binding for SaveAs
                    ResetState();//starts a new file by clearing the input field
                }
                else if (didSave == MessageBoxResult.No)//doesn't save
                {
                    ResetState();//starts a new file by clearing the input field
                }
            }
            else //file already saved
            {
                ResetState(); //starts a new file by clearing the input field
            }
        }


        /*
        * FUNCTION : ResetState()
        *
        * DESCRIPTION : This method resets the state of the main window back to what it was when it started
        * 
        * PARAMETERS : None
        *
        * RETURNS : Nothing
        */
        private void ResetState()
        {
            wordCount.Text = "Character Count: 0";
            userInput.Text = "";
            isSaved = true;
            openedFile = "";
        }

        /*
         * FUNCTION : CommandBinding_CanExecute_Open()
         *
         * DESCRIPTION : This method looks to see if the Open command can be executed.  It always can be executed
         *
         * PARAMETERS : sender - the object that raised the event
         *              e - contains the relevant event data
         *
         * RETURNS : Nothing
         */
        private void CommandBinding_CanExecute_Open(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

      /*
       * FUNCTION : CommandBinding_Executed_Open()
       *
       * DESCRIPTION : This method executes the command binding for Open.  It checks to see if the file needs to 
       *               be saved and reacts accordingly
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void CommandBinding_Executed_Open(object sender, ExecutedRoutedEventArgs e)
        {
            if (isSaved == true) //if the file doesn't need a save
            {
                OpenWorkDialog();
            }
            else
            {
                MessageBoxResult didSave = System.Windows.MessageBox.Show("Do you want to save your work", "Save Work", MessageBoxButton.YesNoCancel);
                if (didSave == MessageBoxResult.Yes) //if the user wants to save
                {
                    if ((openedFile != null) && (openedFile != ""))
                    {
                        SaveFile();
                    }
                    else
                    {
                        SaveWorkDialog();
                    }
                    OpenWorkDialog();
                }
                else if (didSave == MessageBoxResult.No) //if the user doesn't want to save
                {
                    OpenWorkDialog();
                }
            }
        }

      /*
       * FUNCTION : OpenWorkDialog()
       *
       * DESCRIPTION : This method contains the logic to open a file
       *
       * PARAMETERS : None
       *
       * RETURNS : Nothing
       */
        public void OpenWorkDialog()
        {
            try
            {
                OpenFileDialog UserFileOpen = new OpenFileDialog();
                UserFileOpen.FileName = "*.txt";
                UserFileOpen.Filter = "Text File (*.txt )| *.txt";
                DialogResult opendlg = UserFileOpen.ShowDialog();
                if (opendlg.ToString() != "Cancel")
                {
                    openedFile = UserFileOpen.FileName;
                    userInput.Text = System.IO.File.ReadAllText(openedFile);
                    wordCount.Text = "Character Count: " + userInput.Text.Length;
                    isSaved = true;
                }
            }
            catch(IOException e)
            {
                Console.WriteLine("{0:S}: Please Try Again", e.Message);            
            }
        }

      /*
       * FUNCTION : CommandBinding_CanExecute_SaveAs()
       *
       * DESCRIPTION : This method looks to see if the SaveAs command can be executed.  It can only be executed if the 
       *               file has been saved recently
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void CommandBinding_CanExecute_SaveAs(object sender, CanExecuteRoutedEventArgs e)
        {
            if (isSaved == false)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

      /*
       * FUNCTION : CommandBinding_Executed_SaveAs()
       *
       * DESCRIPTION : This method triggers when the SaveAs command is executed.  
       *               It allows the user to save their text to a file.  It contains logic to determine a default file name.
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void CommandBinding_Executed_SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            SaveWorkDialog();

        }
        public void SaveWorkDialog()
        {
            try
            {
                SaveFileDialog SaveAs = new SaveFileDialog(); // create new SaveDialogWindow
                string tempFileName = openedFile; //save the file name in case the user cancels
                if (openedFile != null && openedFile != "")  //skips some logic if user has already saved a file
                {
                    SaveAs.InitialDirectory = System.IO.Path.GetFullPath(openedFile);
                    SaveAs.FileName = System.IO.Path.GetFileName(openedFile);
                    string fileExtension = System.IO.Path.GetExtension(openedFile);
                    SaveAs.Filter = "(*" + fileExtension + ")|*" + fileExtension;
                }
                else //if this is the first file saved in this session
                {
                    int i = 1;
                    SaveAs.InitialDirectory = "C:\\Users\\Chris\\Documents";
                    string fileExistPath = SaveAs.InitialDirectory + "\\Document" + i + ".txt";
                    while (System.IO.File.Exists(fileExistPath) == true) //loops while the file name exists
                    {
                        i++;
                        fileExistPath = SaveAs.InitialDirectory + "\\Document" + i + ".txt";
                    }
                    string defaultFileName = String.Format("Document{0:N}", i); //sets default file name to something that doesn't exist
                    SaveAs.FileName = defaultFileName;
                    SaveAs.Filter = "Text File(*.txt )| *.txt";
                }
                DialogResult didSave = SaveAs.ShowDialog();

                if (didSave.ToString() == "OK") //if user saves
                {
                    openedFile = SaveAs.FileName;
                    System.IO.File.WriteAllText(openedFile, userInput.Text);
                    isSaved = true;
                }
                else if (didSave.ToString() == "Cancel") //if user changes mind
                {
                    openedFile = tempFileName;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("{0:S}: Please Try Again", e.Message);
            }
        }
        /*
       * FUNCTION : CommandBinding_CanExecute_Close()
       *
       * DESCRIPTION : This method looks to see if the Close command can be executed.  It can only be executed if the 
       *               file has been saved recently
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void CommandBinding_CanExecute_Close(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /*
       * FUNCTION : CommandBinding_Executed_Close()
       *
       * DESCRIPTION : This method triggers when the Close command is triggered.  It attempts to close the window
       *               which calls the MainWindow_Closing function
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        /*
       * FUNCTION : toggleWordWrap()
       *
       * DESCRIPTION : This method triggers when the user clicks the TextWrap option in the menu.  
       *               Switches it between wrapping and no wrapping
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void toggleWordWrap(object sender, RoutedEventArgs e)
        {
            if (textWrap.IsChecked == true)
            {
                userInput.TextWrapping = TextWrapping.Wrap;
            }
            else
            {
                userInput.TextWrapping = TextWrapping.NoWrap;
            }
        }

      /*
       * FUNCTION : openChangeFont_click()
       *
       * DESCRIPTION : This method triggers when the user clicks the 'Font' button in the menu.  Opens a ChangeFont Window
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void openChangeFont_click(object sender, RoutedEventArgs e)
        {
            ChangeFont userChangeFont = new ChangeFont(userInput.FontFamily, (int)userInput.FontSize);
            userChangeFont.Owner = this;
            userChangeFont.ShowDialog();
            userInput.FontFamily = new FontFamily (userChangeFont.fontToChangeTo);
            userInput.FontSize = userChangeFont.fontSizeToChange;

        }
     /*
      * FUNCTION : MainWindow_Closing()
      *
      * DESCRIPTION : This method triggers when the some part of the application tries to close the window.m 
      *               It asks the user to save the file if they haven't already and then closes if the user wishes
      *
      * PARAMETERS : sender - the object that raised the event
      *              e - contains the relevant event data
      *
      * RETURNS : Nothing
      */
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult saveAndClose = System.Windows.MessageBox.Show("Do you want to save your work before closing?", "Save Work", MessageBoxButton.YesNoCancel);
            if (saveAndClose == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (saveAndClose == MessageBoxResult.Yes)
            {
                if ((openedFile != null) && (openedFile != ""))
                {
                    SaveFile();
                }
                else
                {
                    SaveWorkDialog();
                    if (isSaved == false)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

      /* FUNCTION : CommandBinding_CanExecute_Close()
       *
       * DESCRIPTION : This method looks to see if the Save command can be executed.  It can only be executed if an
       *               existing file has already been opened or saved
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void CommandBinding_CanExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((isSaved == false) && ((openedFile != null) && (openedFile != "")))
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

    /*
     * FUNCTION : CommandBinding_Executed_Save()
     *
     * DESCRIPTION : This method triggers when the "Save" menu item is clicked.  It saves the file to the already open file path
     *
     * PARAMETERS : sender - the object that raised the event
     *              e - contains the relevant event data
     *
     * RETURNS : Nothing
     */
        private void CommandBinding_Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFile();
        }


        /*
         * FUNCTION : SaveFile()
         *
         * DESCRIPTION : Contains the logic for saving a file if it already exists
         *
         * PARAMETERS : None
         *
         * RETURNS : Nothing
         */
        private void SaveFile()
        {
            try
            {
                System.IO.File.WriteAllText(openedFile, userInput.Text);
                isSaved = true;
            }
            catch (IOException e)
            {
                Console.WriteLine("{0:S}: Please Try Again", e.Message);
            }
        }
    }

}
