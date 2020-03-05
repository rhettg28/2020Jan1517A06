using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.SamplePages
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //MessageLabel will be emptied on each event
            //Page_Load executes BEFORE any button events
            MessageLabel.Text = "";
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            string msg = "";
            msg += "Name: " + FullName.Text;
            msg += " Email: " + EmailAddress.Text;
            msg += " Phone: " + Phone.Text;
            msg += " Time: " + (FullOrPartTime.SelectedValue == "1" ? "Full Time" : "Part Time");

            //traverse the checkbox list, review one item at a time and add
            //  those items selected to the message. If no items were chosen
            //  then add a message stating that no items were chosen

            msg += " Jobs: ";

            bool found = false;

            foreach(ListItem jobrow in Jobs.Items)
            {
                if (jobrow.Selected)
                {
                    msg += jobrow.Text + " ";
                    found = true;
                }
            }

            if (!found)
            {
                msg += "You did not select a job. Application rejected.";
            }

            MessageLabel.Text = msg;
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            FullName.Text = "";
            EmailAddress.Text = "";
            Phone.Text = "";
            FullOrPartTime.SelectedIndex = -1; //manuall reset the radiobutton list
            Jobs.ClearSelection(); //a method which resets a list (CheckBoxList)
        }
    }
}