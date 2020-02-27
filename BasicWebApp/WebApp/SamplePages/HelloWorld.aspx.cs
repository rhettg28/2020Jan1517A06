using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.SamplePages
{
    public partial class HelloWorld : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void PressMe_Click(object sender, EventArgs e)
        {
            //when the PressMe button is pressed, this code will be executed
            //this is called event-driven logic
            //the event was the pressing of the button
            //see the Onclick property of your control on the web page.

            //The ID name of a control acts as the variable name in your code
            //Since, each control is a class instance, access to the contents
            //  of a class is by the class properties
            //Just like Razor, 95% of all data content is returned as a string
            if(string.IsNullOrEmpty(NameArg.Text))
            {
                OutputArea.Text = "Enter your name, please";
            }
            else
            {
                OutputArea.Text = "Hello " + NameArg.Text;
                OutputArea.ForeColor = System.Drawing.Color.PaleVioletRed;
                OutputArea.Font.Size = 100;
            }
        }
    }
}