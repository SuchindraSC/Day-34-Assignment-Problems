using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeeManagement;
using EmployeeManagement.Model.SalaryModel;

namespace EmployeeManagementTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GivenSalaryDetails_AbleToUpdateSalaryDetails()
        {
            Salary salary = new Salary();
            SalaryUpdateModel updateModel = new SalaryUpdateModel()
            {
                SalaryId = 2,
                Month = "Jan",
                EmployeeSalary = 1300,
                EmployeeId = 5
            };

            int EmpSalary = salary.UpdateEmployeeSalary(updateModel);

            Assert.AreEqual(updateModel.EmployeeSalary, EmpSalary);
        }
    }
}
