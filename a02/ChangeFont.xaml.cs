/*
 * FILE : ChangeFont.xaml.cs
 * PROJECT : PROG2121 - Assignment #2
 * PROGRAMMER : Chris Lemon
 * FIRST VERSION : 2020 - 09 - 20
 * LAST REVISION : 2020 - 09 - 25
 * DESCRIPTION : This file holds the class which outlines the functionality of the change font window.
 *               It allows the user to change the font type and size
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
    * NAME : ChangeFont
    * PURPOSE : This class defines a ChangeFont window which contains two combo boxes that have settings the
    *           user can change to alter the appearance of the text in the input box.
    */
    public partial class ChangeFont : Window
    {
        // class attributes
        private const int maxFontSize = 108;
        public FontFamily currentFont { get; set; }
        public String fontToChangeTo { get; set; }
        public int fontSizeToChange { get; set; }
       /*
        * FUNCTION : ChangeFont()
        *
        * DESCRIPTION : This is the constructor method for the ChangeFont class.  Sets inital values for the properties
        *               and calls methods to fill the combo boxes on the window
        *
        * PARAMETERS : None
        *
        * RETURNS : Nothing
        */
        public ChangeFont(FontFamily userFont, int userFontSize)
        {
            currentFont = userFont;
            fontSizeToChange = userFontSize;
            InitializeComponent();
            FillFontComboBox(FontList);
            FillFontSizeComboBox(FontSizeList);
        }
        /*
         * FUNCTION : FillFontComboBox()
         *
         * DESCRIPTION : Loops through the fonts available to the system to fill the combobox. This code was taken from
         *               https://docs.microsoft.com/en-us/dotnet/desktop/wpf/advanced/how-to-enumerate-system-fonts?view=netframeworkdesktop-4.8
         *               on September 22nd, 2020
         *
         * PARAMETERS : comboBoxFonts - the combo box to fill
         *
         * RETURNS : Nothing
         */
        public void FillFontComboBox(ComboBox comboBoxFonts)
        {
            // Enumerate the current set of system fonts,
            // and fill the combo box with the names of the fonts.
            int defaultItemSelect = 0;
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies) //loop through all fonts
            {
                // FontFamily.Source contains the font family name.
                comboBoxFonts.Items.Add(fontFamily.Source); //add them to combo box
                if (fontFamily.Source == currentFont.Source) //check to see if it's the current font
                {
                    defaultItemSelect = comboBoxFonts.Items.Count;
                }
            }
            comboBoxFonts.SelectedIndex = defaultItemSelect - 1; //set the font to be initially displayed in box
        }

      /*
       * FUNCTION : FillFontSizeComboBox()
       *
       * DESCRIPTION : Loops through font sizes and places them in a combo box
       *
       * PARAMETERS : comboBoxFontSize - the combo box to fill
       *
       * RETURNS : Nothing
       */
        public void FillFontSizeComboBox(ComboBox comboBoxFontSize)
        {
            int defaultItemSelect = 0;
            if (fontSizeToChange %2 != 0)//check to see if the current font size is an odd number
            {
                fontSizeToChange -= 1;
            }
            for (int i = 2; i <= maxFontSize; i += 2)//increment by 2
            {
                comboBoxFontSize.Items.Add(i); // add i into combo box
                if (i == fontSizeToChange) //check to see if i is the current font size
                {
                    defaultItemSelect = comboBoxFontSize.Items.Count;
                }
            }
            comboBoxFontSize.SelectedIndex = defaultItemSelect - 1;//set default value in combo box
        }

        /*
         * FUNCTION : changeFontAndClose()
         *
         * DESCRIPTION : changes the current font style and size and closes the window
         *
         * PARAMETERS : sender - the object that raised the event
         *              e - contains the relevant event data
         *
         * RETURNS : Nothing
         */
        private void changeFontAndClose(object sender, RoutedEventArgs e)
        {
            if (FontList.SelectedItem != null)
            {
                fontToChangeTo = FontList.SelectedItem.ToString();
            }
            if (FontSizeList.SelectedItem != null)
            {
                fontSizeToChange = (int)FontSizeList.SelectedItem;
            }
            this.Close();
        }
    }
}
