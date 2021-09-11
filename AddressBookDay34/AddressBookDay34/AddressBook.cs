using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AddressBookDay34
{
    public class AddressBook
    {
        string connectionString = @"Data Source=(LocalDb)\localdb;Initial Catalog=AddressBookSystem;Integrated Security=True";

        AddressBookModel address = new AddressBookModel();
        public int GetPersonDetailsfromDatabase()
        {
            int Count = 0;
            try
            {
                AddressBookModel addressBookModel = new AddressBookModel();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string retrieveQuery = @"select p.person_id,p.firstname,p.lastname,p.phone,p.email,ab.book_name,ab.book_id,ab.book_type,p.zip ,a.city,a.state from person p inner join address a on p.zip=a.zip inner join Person_addressbook pa on pa.person_id=p.person_id inner join addressbook ab on ab.book_id=pa.book_id";
                    ;
                    SqlCommand sqlCommand = new SqlCommand(retrieveQuery, connection);
                    connection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        Console.WriteLine("Address Book Services Database has following Contact details right now");
                        while (sqlDataReader.Read())
                        {
                            addressBookModel.person_id = sqlDataReader.GetInt32(0);
                            addressBookModel.firstname = sqlDataReader.GetString(1);
                            addressBookModel.lastname = sqlDataReader.GetString(2);
                            addressBookModel.phone = sqlDataReader.GetString(3);
                            addressBookModel.email = sqlDataReader.GetString(4);
                            addressBookModel.book_id = sqlDataReader.GetInt32(6);
                            addressBookModel.city = sqlDataReader.GetString(9);
                            addressBookModel.zip = sqlDataReader.GetInt32(8);
                            addressBookModel.state = sqlDataReader.GetString(10);
                            addressBookModel.book_name = sqlDataReader.GetString(5);
                            addressBookModel.book_type = sqlDataReader.GetString(7);
                            Count++;
                            Console.WriteLine("{0}, {1}, {2}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", addressBookModel.person_id, addressBookModel.firstname, addressBookModel.lastname,
                                addressBookModel.phone, addressBookModel.email, addressBookModel.book_id, addressBookModel.city, addressBookModel.zip, addressBookModel.state, addressBookModel.book_name, addressBookModel.book_type);
                        }
                        Console.WriteLine("New Contact Added Successfully");
                        sqlDataReader.Close();
                    }
                    connection.Close();
                }
                return Count;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool UpdateContact(AddressBookModel model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("SpUpdateContact", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@first_name", model.firstname);
                    command.Parameters.AddWithValue("@last_name", model.lastname);
                    command.Parameters.AddWithValue("@phone_no", model.phone);
                    command.Parameters.AddWithValue("@email1", model.email);
                    command.Parameters.AddWithValue("@zip1", model.zip);


                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    Console.WriteLine("Contact Updated Successfully !");
                    connection.Close();
                    if (result == 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool RetriveContactInParticularPeriod()
        {

            try
            {
                AddressBookModel addressBookModel = new AddressBookModel();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string retrieveQuery = @"select p.person_id,p.firstname,p.lastname,p.date_added,p.phone,p.email,ab.book_name,ab.book_id,ab.book_type   ,p.zip ,a.city,a.state from person p inner join address a on p.zip=a.zip inner join
                                            Person_addressbook pa on pa.person_id=p.person_id inner join addressbook ab on ab.book_id=pa.book_id where p.date_added between '2019-03-12' and getdate();";

                    SqlCommand sqlCommand = new SqlCommand(retrieveQuery, connection);
                    connection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        Console.WriteLine("Address Book Services Database has following Contact details right now");
                        while (sqlDataReader.Read())
                        {

                            addressBookModel.person_id = sqlDataReader.GetInt32(0);
                            addressBookModel.firstname = sqlDataReader.GetString(1);
                            addressBookModel.lastname = sqlDataReader.GetString(2);
                            addressBookModel.phone = sqlDataReader.GetString(4);
                            addressBookModel.email = sqlDataReader.GetString(5);
                            addressBookModel.book_id = sqlDataReader.GetInt32(7);
                            addressBookModel.city = sqlDataReader.GetString(10);
                            addressBookModel.zip = sqlDataReader.GetInt32(9);
                            addressBookModel.state = sqlDataReader.GetString(11);
                            addressBookModel.book_name = sqlDataReader.GetString(6);
                            addressBookModel.book_type = sqlDataReader.GetString(8);
                            addressBookModel.date_added = sqlDataReader.GetDateTime(3);

                            Console.WriteLine("{0}, {1}, {2},{3}, {4}, {5}, {6}, {7}, {8}, {9}, {10} {11}", addressBookModel.person_id, addressBookModel.firstname, addressBookModel.lastname, addressBookModel.date_added,
                                addressBookModel.phone, addressBookModel.email, addressBookModel.book_name, addressBookModel.book_id, addressBookModel.book_type, addressBookModel.zip, addressBookModel.city, addressBookModel.state);
                        }
                        Console.WriteLine("New Contact Added Successfully");
                        sqlDataReader.Close();
                        return true;
                    }
                    connection.Close();
                    return false;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int RetriveContactByCityOrState(AddressBookModel addressBookModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SpRetriveContactByCityOrState", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@state_name", addressBookModel.state);
                connection.Open();
                var Count = (int)command.ExecuteScalar();
                SqlDataReader sqlDataReader = command.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        addressBookModel.person_id = sqlDataReader.GetInt32(0);
                        Console.WriteLine("Number of Conctacts beloning to entered City Or State {0}", addressBookModel.person_id);
                    }
                }
                return Count;
            }
        }
        public bool AddNewContactWithoutThread(List<AddressBookDetail> model1)
        {
            var result = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    foreach (AddressBookDetail model in model1)
                    {

                        SqlCommand command = new SqlCommand("SpAddNewRecord", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@first_name", model.firstname);
                        command.Parameters.AddWithValue("@last_name", model.lastname);
                        command.Parameters.AddWithValue("@email", model.email);
                        command.Parameters.AddWithValue("@phone_number", model.phone);
                        command.Parameters.AddWithValue("@zip", model.zip);
                        command.Parameters.AddWithValue("@date_added", model.date_added);
                        command.Parameters.AddWithValue("@person_id", model.person_id);
                        command.Parameters.AddWithValue("@book_id", model.book_id);
                        connection.Open();
                        result = command.ExecuteNonQuery();
                        Console.WriteLine("New Contact Added Successfully");
                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (result != 0)
                return true;
            return false;
        }
        public int AddNewContactWithThread(List<AddressBookDetail> model1)
        {
            List<AddressBookDetail> addressBookModelslist = new List<AddressBookDetail>();
            var result = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    foreach (AddressBookDetail model in model1)
                    {
                        Task thread = new Task(() =>
                        {
                            SqlCommand command = new SqlCommand("SpAddNewRecord", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@first_name", model.firstname);
                            command.Parameters.AddWithValue("@last_name", model.lastname);
                            command.Parameters.AddWithValue("@email", model.email);
                            command.Parameters.AddWithValue("@phone_number", model.phone);
                            command.Parameters.AddWithValue("@zip", model.zip);
                            command.Parameters.AddWithValue("@date_added", model.date_added);
                            command.Parameters.AddWithValue("@person_id", model.person_id);
                            command.Parameters.AddWithValue("@book_id", model.book_id);
                            connection.Open();
                            result = command.ExecuteNonQuery();
                            addressBookModelslist.Add(model);
                            Console.WriteLine("New Contact Added Successfully");
                            connection.Close();
                        });
                        thread.Start();
                    }
                    Console.WriteLine(model1.Count);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return model1.Count;
        }
    }
}
