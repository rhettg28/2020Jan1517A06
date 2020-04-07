using NorthwindSystem.BLL;
using NorthwindSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//these are necessary for the expand
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;

namespace WebApp.NorthwindPages
{
    public partial class ProductCRUD : System.Web.UI.Page
    {
        List<string> errormsgs = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            Message.DataSource = null;
            Message.DataBind();

            //execute when page is first displayed
            if (!Page.IsPostBack)
            {
                BindProductList();
            }

        }

        //use this method to discover the inner most error message.
        //this rotuing has been created by the user
        protected Exception GetInnerException(Exception ex)
        {
            //drill down to the inner most exception
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        //use this method to load a DataList with a variable
        //number of message lines.
        //each line is a string
        //the strings (lines) are passed to this routine in
        //   a List<string>
        //second parameter is the bootstrap cssclass
        protected void LoadMessageDisplay(List<string> errormsglist, string cssclass)
        {
            Message.CssClass = cssclass;
            Message.DataSource = errormsglist;
            Message.DataBind();
        }

        protected void BindProductList()
        {
            try
            {
                ProductController sysmgr = new ProductController();
                List<Product> info = sysmgr.Products_List();
                info.Sort((x, y) => x.ProductName.CompareTo(y.ProductName));
                ProductList.DataSource = info;
                ProductList.DataTextField = nameof(ProductName);
                ProductList.DataValueField = nameof(ProductID);
                ProductList.DataBind();
                ProductList.Items.Insert(0, new ListItem("select...", "0"));
            }
            catch (Exception ex)
            {
                errormsgs.Add(GetInnerException(ex).Message);
                LoadMessageDisplay(errormsgs, "alert alert-danger");
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            //standard lookup
            if (ProductList.SelectedIndex == 0)
            {
                errormsgs.Add("Select a product to lookup.");
                LoadMessageDisplay(errormsgs, "alert alert-info");
            }
            else
            {
                try
                {
                    ProductController sysmgr = new ProductController();
                    Product info = sysmgr.Products_FindByID(int.Parse(ProductList.SelectedValue));
                    if (info == null)
                    {
                        errormsgs.Add("Selected product no longer available. Select a different product.");
                        LoadMessageDisplay(errormsgs, "alert alert-info");
                        BindProductList();
                    }
                    else
                    {
                        //load the corresponding individual fields
                        ProductID.Text = info.ProductID.ToString();
                        ProductName.Text = info.ProductName;
                        if (!info.SupplierID.HasValue)
                        {
                            SupplierList.SelectedIndex = 0;
                        }
                        else
                        {
                            SupplierList.SelectedValue = info.SupplierID.ToString();
                        }
                        if (info.CategoryID == null)
                        {
                            CategoryList.SelectedIndex = 0;
                        }
                        else
                        {
                            CategoryList.SelectedValue = info.CategoryID.ToString();
                        }
                        QuantityPerUnit.Text = string.IsNullOrEmpty(info.QuantityPerUnit) ? "" : info.QuantityPerUnit;
                        UnitPrice.Text = info.UnitPrice == null ? "" : string.Format("{0:0.00}",info.UnitPrice);
                        UnitsInStock.Text = info.UnitsInStock == null ? "" : info.UnitsInStock.ToString();
                        UnitsOnOrder.Text = info.UnitsOnOrder == null ? "" : info.UnitsOnOrder.ToString();
                        ReorderLevel.Text = info.ReorderLevel == null ? "" : info.ReorderLevel.ToString();
                        Discontinued.Checked = info.Discontinued;
                    }
                }
                catch (Exception ex)
                {
                    errormsgs.Add(GetInnerException(ex).Message);
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
            }
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            ProductID.Text = "";
            ProductName.Text = "";
            QuantityPerUnit.Text = "";
            UnitPrice.Text = "";
            UnitsInStock.Text = "";
            UnitsOnOrder.Text = "";
            ReorderLevel.Text = "";
            Discontinued.Checked = false;
            SupplierList.ClearSelection();
            CategoryList.ClearSelection();
            //optionally
            ProductList.ClearSelection();
        }

        protected void AddProduct_Click(object sender, EventArgs e)
        {
            //check the page validation
            //only required if there is web page validation controls
            if ( Page.IsValid)
            {
                //page validation controls are good
                //
                //you oculd possible have additional validation within your code-behind
                //do that validation before calling your controller to do the addition of 
                //  the new database record
                //Assume for this example that a Supplier and Category are required
                if (CategoryList.SelectedIndex == 0)
                {
                    errormsgs.Add("Category is required");
                }
                if (SupplierList.SelectedIndex == 0)
                {
                    errormsgs.Add("Supplier is required");
                }
                //the following could have been done as a Validation control on the web form
                decimal price = 0.0m;
                if (!decimal.TryParse(UnitPrice.Text, out price))
                {
                    errormsgs.Add("UnitPrice needs to be a number");
                }
                else if (price < 0.0m)
                {
                    errormsgs.Add("Unit price must be 0.00 or greater");
                }

                //with the error handle example in this program one tests to see if the
                //  List<string> errormsgs has any content.
                //if so, display all the errors and stop processing for this event
                //otherwise, continue with the processing to add the new product to the 
                //  database
                if (errormsgs.Count > 0)
                {
                    LoadMessageDisplay(errormsgs, "alert alert-info");
                }
                else
                {
                    //all validation has passed
                    //assume data is good to this point
                    //the addition of the record to the database must be done using
                    //  user-friendly error handling
                    try
                    {
                        //standard add 
                        //create a new instance of the entity class that the data
                        //  will be added to
                        Product theItem = new Product();
                        //thing about your pkey; needed (non-identity) or not (identity)
                        //non-identity: you will need to load your value
                        //identity: you DO NOT need to load your value
                        //ProductID is an identity pkey
                        theItem.ProductName = ProductName.Text;
                        theItem.QuantityPerUnit = QuantityPerUnit.Text;
                        theItem.UnitPrice = string.IsNullOrEmpty(UnitPrice.Text) ? (decimal?)null: decimal.Parse(UnitPrice.Text);
                        theItem.UnitsInStock = string.IsNullOrEmpty(UnitsInStock.Text) ? (Int16?)null : Int16.Parse(UnitsInStock.Text);
                        theItem.UnitsOnOrder = string.IsNullOrEmpty(UnitsOnOrder.Text) ? (Int16?)null : Int16.Parse(UnitsOnOrder.Text);
                        theItem.ReorderLevel = string.IsNullOrEmpty(ReorderLevel.Text) ? (Int16?)null : Int16.Parse(ReorderLevel.Text);
                        if (SupplierList.SelectedIndex == 0)
                        {
                            theItem.SupplierID = null;
                        }
                        else
                        {
                            theItem.SupplierID = int.Parse(SupplierList.SelectedValue);
                        }
                        if (CategoryList.SelectedIndex == 0)
                        {
                            theItem.CategoryID = null;
                        }
                        else
                        {
                            theItem.CategoryID = int.Parse(CategoryList.SelectedValue);
                        }
                        //what about discontinued
                        //since this is an add, would you add a NEW product that is discontinued?
                        //in this case, you made decide to simply hard-code the value for Discontinued
                        theItem.Discontinued = false;

                        //connect to the controller class that handles adding a product to the database
                        ProductController sysmgr = new ProductController();

                        //call the appropriate method in the controller and receive any feedback 
                        //in this example, the controller method will return the new pkey value 
                        //  if the record was successfully added to the database
                        //if there is an error while adding, the catch() will handle the message
                        int newProductId = sysmgr.Products_Add(theItem);

                        //successful add
                        ProductID.Text = newProductId.ToString();
                        //refresh the dropdownlist for products
                        BindProductList();
                        //position in the list 
                        ProductList.SelectedValue = newProductId.ToString();

                    }
                    catch (DbUpdateException ex)
                    {
                        UpdateException updateException = (UpdateException)ex.InnerException;
                        if (updateException.InnerException != null)
                        {
                            errormsgs.Add(updateException.InnerException.Message.ToString());
                        }
                        else
                        {
                            errormsgs.Add(updateException.Message);
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                errormsgs.Add(validationError.ErrorMessage);
                            }
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (Exception ex)
                    {
                        //general, catch "all"
                        errormsgs.Add(GetInnerException(ex).ToString());
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                }
            }
        }

        protected void UpdateProduct_Click(object sender, EventArgs e)
        {
            //executes your web page validation controls
            if (Page.IsValid)
            {
                //any other required validation in code-behind that was not done
                //  with the web page validation controls

                //check to see if the pkey value is present
                if (string.IsNullOrEmpty(ProductID.Text))
                {
                    errormsgs.Add("Look up a product to update.");
                    LoadMessageDisplay(errormsgs, "alert alert-info");
                }
                else
                {
                    Product item = LoadProductInstance();

                    //in addition to the basic load of the Product instance for an update one also
                    //  must include the identity pKey value
                    item.ProductID = int.Parse(ProductID.Text);

                    //deal with a "Logical" delete property if one exists
                    item.Discontinued = Discontinued.Checked;

                    try
                    {
                        ProductController sysmgr = new ProductController();
                        int rowsaffected = sysmgr.Products_Update(item);
                        //check for success with or without actual changes to the database
                        if (rowsaffected == 0)
                        {
                            //no abort BUT no rows changed 
                            //this means that the record no longer is on the database
                            errormsgs.Add("Product has been removed from the file.");
                            LoadMessageDisplay(errormsgs, "alert alert-warning");
                            ProductID.Text = "";
                            BindProductList();  //make sure your selection list is up to date                           
                        }
                        else
                        {
                            //record was update
                            errormsgs.Add("Product has been updated.");
                            LoadMessageDisplay(errormsgs, "alert alert-info");
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        UpdateException updateException = (UpdateException)ex.InnerException;
                        if (updateException.InnerException != null)
                        {
                            errormsgs.Add(updateException.InnerException.Message.ToString());
                        }
                        else
                        {
                            errormsgs.Add(updateException.Message);
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                errormsgs.Add(validationError.ErrorMessage);
                            }
                        }
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                    catch (Exception ex)
                    {
                        //general, catch "all"
                        errormsgs.Add(GetInnerException(ex).ToString());
                        LoadMessageDisplay(errormsgs, "alert alert-danger");
                    }
                }
            }
        }

        protected void RemoveProduct_Click(object sender, EventArgs e)
        {
            //check to see if the pkey value is present
            if (string.IsNullOrEmpty(ProductID.Text))
            {
                errormsgs.Add("Look up a product to discontinue.");
                LoadMessageDisplay(errormsgs, "alert alert-info");
            }
            else
            {
                try
                {
                    ProductController sysmgr = new ProductController();
                    int rowsaffected = sysmgr.Products_Delete(int.Parse(ProductID.Text));
                    //check for success with or without actual changes to the database
                    if (rowsaffected == 0)
                    {
                        //no abort BUT no rows changed 
                        //this means that the record no longer is on the database
                        errormsgs.Add("Product has been removed from the file.");
                        LoadMessageDisplay(errormsgs, "alert alert-warning");
                        ProductID.Text = "";
                        BindProductList();  //make sure your selection list is up to date                           
                    }
                    else
                    {
                        //record was update
                        errormsgs.Add("Product has been discontinued.");
                        LoadMessageDisplay(errormsgs, "alert alert-info");
                        Discontinued.Checked = true;
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException.InnerException != null)
                    {
                        errormsgs.Add(updateException.InnerException.Message.ToString());
                    }
                    else
                    {
                        errormsgs.Add(updateException.Message);
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            errormsgs.Add(validationError.ErrorMessage);
                        }
                    }
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
                catch (Exception ex)
                {
                    //general, catch "all"
                    errormsgs.Add(GetInnerException(ex).ToString());
                    LoadMessageDisplay(errormsgs, "alert alert-danger");
                }
            }
        }

        protected Product LoadProductInstance()
        {
            Product item = new Product();
            item.ProductName = ProductName.Text;
            item.QuantityPerUnit = string.IsNullOrEmpty(QuantityPerUnit.Text) ? null : QuantityPerUnit.Text;
            item.SupplierID = SupplierList.SelectedIndex == 0 ? (int?)null : int.Parse(SupplierList.SelectedValue);
            item.CategoryID = CategoryList.SelectedIndex == 0 ? (int?)null : int.Parse(CategoryList.SelectedValue);
            item.UnitPrice = string.IsNullOrEmpty(UnitPrice.Text) ? (decimal?)null : decimal.Parse(UnitPrice.Text);
            item.UnitsInStock = string.IsNullOrEmpty(UnitsInStock.Text) ? (Int16?)null : Int16.Parse(UnitsInStock.Text);
            item.UnitsOnOrder = string.IsNullOrEmpty(UnitsOnOrder.Text) ? (Int16?)null : Int16.Parse(UnitsOnOrder.Text);
            item.ReorderLevel = string.IsNullOrEmpty(ReorderLevel.Text) ? (Int16?)null : Int16.Parse(ReorderLevel.Text);

            //optionally you could handle the Discontinue in this method
            return item;
        }
    }
}