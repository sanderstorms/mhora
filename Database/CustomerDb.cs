using SqlNado;
using System;
using SqlNado.Utilities;

namespace mhora.Database
{
    internal class CustomerDb
    {
        public CustomerDb()
        {
            using (var db = new SQLiteDatabase("customers.db"))
            {
                var customer = new Customer();
                customer.Email = "killroy@example.com";
                customer.Name = "Killroy";

                // updates or Insert (choice is made if there is already a primary key on the object).
                // by default, the Save operation synchronize the table schema.
                // if this is run for the first time, it will create the table using Customer type definition (properties).
                db.Save(customer);

                // dumps the customer list to the console
                db.LoadAll<Customer>().ToTableString(Console.Out);

                // dumps the sql query result to the console (should be the same as previous)
                db.LoadRows("SELECT * FROM Customer").ToTableString(Console.Out);

                // dumps the Customer table schema to the console
                TableStringExtensions.ToTableString(db.GetTable<Customer>(), Console.Out);

                // dumps the Customer table columns definitions to the console
                db.GetTable<Customer>().Columns.ToTableString(Console.Out);
            }

        }

        public class Customer
        {
            [SQLiteColumn(IsPrimaryKey = true)]
            public string Email { get; set; }
            public string Name { get; set; }
        }
    }
}
