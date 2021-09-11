using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using EmployeeManagement.Model.SalaryModel;
using System.Data;

namespace EmployeeManagement
{
    public class Salary
    {
        private static SqlConnection ConnectionSetup()
        {
            return new SqlConnection(@"Data Source=(LocalDb)\localdb;Initial Catalog=payroll_service;Integrated Security=True");
        }

        public int UpdateEmployeeSalary(SalaryUpdateModel salaryUpdateModel)
        {
            SqlConnection SalaryConnection = ConnectionSetup();
            int salary = 0;
            try
            {
                using(SalaryConnection)
                {
                    SalaryDetailModel displayModel = new SalaryDetailModel();

                    SqlCommand command = new SqlCommand("spUpdateEmployeeSalary", SalaryConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", salaryUpdateModel.SalaryId);
                    command.Parameters.AddWithValue("@month", salaryUpdateModel.Month);
                    command.Parameters.AddWithValue("@salary", salaryUpdateModel.EmployeeSalary);
                    command.Parameters.AddWithValue("@EmpId", salaryUpdateModel.EmployeeId);

                    SalaryConnection.Open();

                    SqlDataReader dr = command.ExecuteReader();

                    if(dr.HasRows)
                    {
                        while(dr.Read())
                        {
                            displayModel.EmployeeId = Convert.ToInt32(dr["EmpId"]);
                            displayModel.EmployeeName = dr["ENAME"].ToString();
                            displayModel.JobDescription = dr["JOB"].ToString();
                            displayModel.EmployeeSalary = Convert.ToInt32(dr["EMPSAL"]);
                            displayModel.Month = dr["SAL_MONTH"].ToString();
                            displayModel.SalaryId = Convert.ToInt32(dr["SALARYId"]);

                            Console.WriteLine("{0},{1},{2}", displayModel.EmployeeName, displayModel.EmployeeSalary, displayModel.Month);
                            Console.WriteLine("\n");
                            salary = displayModel.EmployeeSalary;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data Found");
                    }
                    dr.Close();

                    SalaryConnection.Close();
                }

            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                SalaryConnection.Close();
            }
            return salary;
        }
    }
}
