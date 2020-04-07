using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using NorthwindSystem.DAL;
using NorthwindSystem.Entities;
using System.Data.SqlClient;    //SqlParameter()
using System.ComponentModel;    //ODS
#endregion

namespace NorthwindSystem.BLL
{
    [DataObject]
    public class ProductController
    {
        public List<Product> Products_List()
        {
            //ensure the sql connection is closed at the end
            //   of the query process
            using (var context = new NorthwindSystemContext())
            {
                //use an extension method of EntityFrame to get
                //all of the data from the sql table Region
                //use the property DbSet that maps the Region sql table to 
                //    the application
                //the dbset T type is Region which describes a single record
                //the actual collection type that is returned from
                //    EntityFramework is either IEnumerable or IQuerable
                //Change the collection type to a List<T> using .ToList()
                return context.Products.ToList();
            }
        }
        public Product Products_FindByID(int productid)
        {
            using (var context = new NorthwindSystemContext())
            {
                //the .Find() method does a primary key lookup of the
                //    sql table
                return context.Products.Find(productid);
            }
        }

        //this query will use a NON_PRIMARY key field off the Product
        //  record to lookup data from the database table
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Product> Products_FindByCategory(int categoryid)
        {
            using (var context = new NorthwindSystemContext())
            {
                //call to an SQL Procedure
                //the call returns a datatype of IEnumerable<T>
                //.SqlQuery<T>(execution string[, list of SqlParameter instances])
                //execution string >> "procedurename parameterlist"
                //a parameter is specified using SqlParameter("parametername", value)
                //SqlParameter() requires using System.Data.SqlClient; namespace
                IEnumerable<Product> results = context.Database.SqlQuery<Product>("Products_GetByCategories @CategoryID", 
                    new SqlParameter("CategoryID", categoryid));

                //convert the IEnumerable<T> to a List<T>
                return results.ToList();
            }
        }
        public List<Product> Products_GetByPartialProductName(string productname)
        {
            using (var context = new NorthwindSystemContext())
            {
                IEnumerable<Product> results = context.Database.SqlQuery<Product>("Products_GetByPartialProductName @PartialName",
                    new SqlParameter("PartialName", productname));
                return results.ToList();
            }
        }

        public int Products_Add(Product item)
        {
            //at some point in time you need to create an instance of your entity class
            //this can be done either on the webpage OR within this method
            //in this example the instance was created and loaded on the web page 

            using (var context = new NorthwindSystemContext())
            {
                //step One
                //Staging
                //during staging the entity will be loaded with the instance to add to the database
                //staging is done in current memory
                //record is NOT yet physically added to the database 
                context.Products.Add(item);

                //step Two
                //cause the physical addition of the staged record to the database
                //if this statement fails, any changed attempted against the database are ROLLEDBACK
                //  and an appropritiate error message is issued by the system
                //if this statement is succesful, THEN the NEW primary key will be available for you
                //  within the entity instance 
                
                //during this statement execution ANY validation in the entity class definition are
                //  executed
                context.SaveChanges();

                //optionally
                return item.ProductID;

            }
        }

        public int Products_Update(Product item)
        {
            using (var context = new NorthwindSystemContext())
            {
                //stage 
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;

                //commit and return of the number rows affected
                return context.SaveChanges();
            }
        }

        public int Products_Delete(int productid)
        {
            using (var context = new NorthwindSystemContext())
            {
                //two types of deletes

                ////physical: physical removal of the record from the database
                ////get the record from the database by the pkey
                //var exists = context.Products.Find(productid);
                ////stage the removal
                //context.Products.Remove(exists);
                ////commit
                //return context.SaveChanges();

                //logical: one that sets a property of the class to a specified value to indicate
                //         the record "logically" does not "exist" on the database anymore

                var exists = context.Products.Find(productid);
                //the setting of the property should be done within this method and NOT dependent
                //  on the user remembering to do the action
                exists.Discontinued = true;

                //stage
                context.Entry(exists).State = System.Data.Entity.EntityState.Modified;

                //commit and return of the number rows affected
                return context.SaveChanges();
            }
        }
    }
}
