using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.SamplePages
{
    public partial class CommonControls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //every time the page is processed (submitted to the web server)
            //  this method is the FIRST event that is processed that you
            //  can easily see and interact with.

            //this is a great place to do common code that is required on
            //  each process of the page
            //Example: empty out old messages

            //Every control on your web form is a class instance
            //Every control has an ID property
            //Every control must have a unique name
            //The ID value is used to reference the control in your code behind
            //Since controls are instances of a class, all rules of OOP apply
            MessageLabel.Text = "";

            //The web page has a flag that can be checked to see if the web
            //  page is posting back
            if (!Page.IsPostBack)
            {
                //If the page is not PostBack, it means that this is the first
                //  time the page has been displayed
                //You can do page initialization by testing the IsPostBack
                //Create a List<T> where T is a class that has 2 columns: a
                //  value and a text display
                List<DDLData> DataCollection = new List<DDLData>();
                DataCollection.Add(new DDLData(1, "COMP1008"));
                DataCollection.Add(new DDLData(3, "DMIT1508"));
                DataCollection.Add(new DDLData(4, "DMIT2018"));
                DataCollection.Add(new DDLData(2, "CPSC1517"));

                //Sorting a List<T>
                // (x,y) are placeholders for 2 records at any given time in the sort
                // => (lamda symbol) is part of the delegate syntax, read as "do the following"
                //Comparing x to y is ascending
                //Comparing y to x is descending
                DataCollection.Sort((x, y) => x.displayField.CompareTo(y.displayField));

                //Place the data into the dropdownlist control
                //a) assign the data collection to the "list" control
                CollectionList.DataSource = DataCollection;

                //b) this step is required for specific "list" controls
                //Indicate which data value is to be assigned to the Value field and the Display
                //  text field there are different styles in assigning this information
                CollectionList.DataValueField = "valueField";
                CollectionList.DataTextField = nameof(DDLData.displayField); //don's prefered method

                //c) bind your data to the actual control
                CollectionList.DataBind();

                //d) OPTIONALLY you can place a prompt line on your control
                CollectionList.Items.Insert(0, new ListItem("select...", "0"));
            }
        }

        protected void SubmitButtonChoice_Click(object sender, EventArgs e)
        {
            int numberchoice = 0;
            //This event will execute ONLY if you press the submit button on your form
            //This event CAN be called from any method within your code behind
            //This event will execute AFTER the Page_Load event
            if (string.IsNullOrEmpty(TextBoxNumberChoice.Text))
            {
                MessageLabel.Text = "Enter a number between 1 and 4.";
            }
            else if (!int.TryParse(TextBoxNumberChoice.Text, out numberchoice))
            {
                MessageLabel.Text = "Invalid number. Enter a number between 1 and 4.";
            }
            else if (numberchoice < 1 || numberchoice > 4)
            {
                MessageLabel.Text = "Number is out of range. Enter a number between 1 and 4.";
            }
            else
            {
                //assign a value to the RadioButton list to indicate the list item selected
                //this can be done using either .SelectedValue or .SelectedIndex
                //.SelectedValue will match the control item value to the argument
                //.SelectedIndex selects the control item based on it's physical index position
                //The best policy is to use .SelectedValue
                RadioButtonListChoice.SelectedValue = numberchoice.ToString();

                //Set the checkbox
                if (numberchoice == 2 || numberchoice == 4)
                {
                    CheckBoxChoice.Checked = true;
                }
                else
                {
                    CheckBoxChoice.Checked = false;
                }

                //Position the dropdownlist
                //this can be done using either .SelectedValue or .SelectedIndex
                //.SelectedValue will match the control item value to the argument
                //.SelectedIndex selects the control item based on it's physical index position
                //The best policy is to use .SelectedValue
                CollectionList.SelectedValue = numberchoice.ToString();

                //Access data from your dropdownlist
                //This can be done using either .SelectedValue or .SelectedIndex or .SelectedItem
                //.SelectedValue will return the value portion of your selected line of the control
                //.SelectedIndex will return the selected line of the control physical index position
                //.SelectedItem will return the display text of your selected line of the control
                DisplayReadOnly.Text = CollectionList.SelectedItem.Text +
                                    " at index " + CollectionList.SelectedIndex +
                                    " having a value of " + CollectionList.SelectedValue;
            }
        }

        protected void LinkButtonCollecetion_Click(object sender, EventArgs e)
        {

        }
    }
}