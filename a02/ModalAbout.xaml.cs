/*
 * FILE : ModalAbout.xaml.cs
 * PROJECT : PROG2121 - Assignment #2
 * PROGRAMMER : Chris Lemon
 * FIRST VERSION : 2020 - 09 - 20
 * LAST REVISION : 2020 - 09 - 25
 * DESCRIPTION : This file holds the class which outlines the functionality of the modal About window.  It displays the author and version number of the application
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
using System.Windows.Shapes;

namespace a02
{
    /*
    * NAME : MainWindow
    * PURPOSE : This class defines an about window to show details about the application.
    */
    public partial class ModalAbout : Window
    {

        /*
         * FUNCTION : ModalAbout()
         *
         * DESCRIPTION : This is the constructor method for the ModalAbout class. 
         *
         * PARAMETERS : None
         *
         * RETURNS : Nothing
         */
        public ModalAbout()
        {
            InitializeComponent();
        }

      /*
       * FUNCTION : Close_Window()
       *
       * DESCRIPTION : This method triggers whenever the 'OK' Button is clicked.  It closes the window
       *
       * PARAMETERS : sender - the object that raised the event
       *              e - contains the relevant event data
       *
       * RETURNS : Nothing
       */
        private void Close_Window(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
