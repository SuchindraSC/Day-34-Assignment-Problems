using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBookDay34
{
    public class AddressBookModel
    {
        public int person_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string book_type { get; set; }
        public int book_id { get; set; }
        public string book_name { get; set; }
        public string city { get; set; }
        public int zip { get; set; }
        public string state { get; set; }
        public DateTime date_added { get; set; }
        public void AddressBookDetail(string firstname, string lastname, string phone, string email, string city, int book_id, int person_id, int zip, DateTime date_added)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.phone = phone;
            this.email = email;
            this.city = city;
            this.book_id = book_id;
            this.person_id = person_id;
            this.zip = zip;
            this.date_added = date_added;
        }

    }
}