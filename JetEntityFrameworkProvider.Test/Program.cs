﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;

using JetEntityFrameworkProvider;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using JetEntityFrameworkProvider.Test.CodeFirst;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace JetEntityFrameworkProvider.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.SetWindowSize(210, 80);

            JetEntityFrameworkProvider.JetCommand.ShowSqlStatements = true;

            JetConnection jetConnection = Helpers.GetJetConnection();
            Context context = new Context(jetConnection);


            Console.WriteLine("DB First ======================================================================");

            /*
            //EntityConnection ec = GetSqlServerEntityConnection();
            EntityConnection ec = JetEntityFrameworkProvider.Test.DbFirst.SetUpDbFirst.GetJetEntityConnection();
            //EntityConnection ec = GetOleDbEntityConnection();

            // Use the Entity SQL to implement the Between operation
            IEnumerable<Course> courses = GetCoursesByEntitySQL(ec);
            ShowCourses("Get the Courses by Entity SQL", courses);
            Console.WriteLine();

            // Use the Entity SQL to implement the Between operation
            courses = GetCoursesByEntityEscapedLikeSQL(ec);
            ShowCourses("Get the Courses by Entity Escaped Like % SQL", courses);
            Console.WriteLine();

            // Use the extension method to implement the Between operation
            courses = GetCoursesByExtension(ec);
            ShowCourses("Get the Courses by extension method", courses);
            Console.WriteLine();

            //School school = new School(ec);
            //Course course = school.Courses.First(c => (c.DepartmentID & 3) == 3);


            Console.WriteLine("Code First ======================================================================");

            Student student;
            student = new Student() { StudentName = "New Student 1" };
            context.Students.Add(student);
            student = new Student() { StudentName = "New Student 2" };
            context.Students.Add(student);
            context.SaveChanges();

            // Add a student to update
            student = new Student() { StudentName = "Student to update" };
            context.Students.Add(student);
            context.SaveChanges();
            int studentId = student.StudentID;

            // Retrieve the student
            student = context.Students.Where(s => s.StudentID == studentId).First();

            // Update the student
            student.StudentName = "Student updated";
            context.SaveChanges();

            // Retrieve the student and check that is the right student
            student = context.Students.Where(s => s.StudentName == "Student updated").First();
            if (student.StudentID != studentId)
                Console.WriteLine("Student save or retrieve error");

            // Delete the student
            context.Students.Remove(student);
            context.SaveChanges();

            // Try to retrieve the student
            student = context.Students.Where(s => s.StudentName == "Student updated" || s.StudentID == studentId).FirstOrDefault();
            if (student != null)
                Console.WriteLine("Student not deleted");
            */

            Console.WriteLine("Function test ======================================================================");

            TableWithSeveralFieldsType table = new TableWithSeveralFieldsType()
            {
                MyInt = 10,
                MyString = " My current string with leading and trailing spaces ",
                MyDateTime = new DateTime(1969, 09, 15, 20, 03, 19)
            };

            context.TableWithSeveralFieldsTypes.Add(table);
            context.SaveChanges();



            Console.WriteLine(context.TableWithSeveralFieldsTypes.Select(c => new { c.MyDateTime.Day }).First().Day);
            Console.WriteLine(context.TableWithSeveralFieldsTypes.Select(c => new { Date = EntityFunctions.AddDays(c.MyDateTime, 4) }).First());
            Console.WriteLine(context.TableWithSeveralFieldsTypes.Select(c => new { ElapsedDays = EntityFunctions.DiffDays(c.MyDateTime, c.MyDateTime) }).First().ElapsedDays.Value);

            Console.WriteLine(context.TableWithSeveralFieldsTypes.Select(c => c.MyString.IndexOf(CanonicalFunctionsTest.MYSTRINGVALUE.Substring(5, 4))).First());


            Console.WriteLine(context.TableWithSeveralFieldsTypes.Where(c => c.MyString.Contains(CanonicalFunctionsTest.MYSTRINGVALUE.Substring(3, 5))).First());
            Console.WriteLine(context.TableWithSeveralFieldsTypes.Where(c => c.MyString.StartsWith(CanonicalFunctionsTest.MYSTRINGVALUE.Substring(3, 5))).FirstOrDefault());
            Console.WriteLine(context.TableWithSeveralFieldsTypes.Where(c => c.MyString.StartsWith(CanonicalFunctionsTest.MYSTRINGVALUE.Substring(0, 5))).First());
            string stringEnd = CanonicalFunctionsTest.MYSTRINGVALUE.Substring(CanonicalFunctionsTest.MYSTRINGVALUE.Length - 5, 5);
            //Console.WriteLine(context.TableWithSeveralFieldsTypes.Where(c => c.MyString.EndsWith(CanonicalFunctionsTest.MYSTRINGVALUE.Substring(CanonicalFunctionsTest.MYSTRINGVALUE.Length - 5, 5))).First());
            Console.WriteLine(context.TableWithSeveralFieldsTypes.Where(c => c.MyString.EndsWith(stringEnd)).First());

            context.Students.Where(s => !(new int[] { 1, 2, 3, 4 }).Contains(s.StudentID)).FirstOrDefault();

            /*
            Console.WriteLine("Schema test ======================================================================");

            Helpers.ShowDataReaderContent(jetConnection, "show tables");
            Helpers.ShowDataReaderContent(jetConnection, "show tablecolumns");
            Helpers.ShowDataReaderContent(jetConnection, "show views");
            Helpers.ShowDataReaderContent(jetConnection, "show viewcolumns");
            Helpers.ShowDataReaderContent(jetConnection, "show constraints");
            Helpers.ShowDataReaderContent(jetConnection, "show checkconstraints");
            Helpers.ShowDataReaderContent(jetConnection, "show constraintcolumns");
            Helpers.ShowDataReaderContent(jetConnection, "show foreignKeyconstraints");
            Helpers.ShowDataReaderContent(jetConnection, "show foreignKeys");
            Helpers.ShowDataReaderContent(jetConnection, "show viewconstraints");
            Helpers.ShowDataReaderContent(jetConnection, "show viewconstraintcolumns");
            Helpers.ShowDataReaderContent(jetConnection, "show viewforeignkeys");
            */

            Console.WriteLine("Schema test with where and order by ==============================================");

            Helpers.ShowDataReaderContent(jetConnection, "show tablecolumns where ParentId = 'Students'");
            Helpers.ShowDataReaderContent(jetConnection, "show indexcolumns where index like 'PK*' order by Index, Ordinal");

            /*           
            // Retrieve some oledb schema infos
            jetConnection.Open();

            //Console.SetOut(new StreamWriter("C:\\Temp\\Tables.txt"));

            DataTable schemaTable = ((OleDbConnection)jetConnection).GetOleDbSchemaTable(
              System.Data.OleDb.OleDbSchemaGuid.Tables,
              new object[] { null, null, null, null });
            JetProviderFactory.JetStoreSchemaDefinitionRetrieveTest.ShowDataTableContent(schemaTable);


            //Console.SetOut(new StreamWriter("C:\\Temp\\Columns.txt"));
            
            schemaTable = ((OleDbConnection)jetConnection).GetOleDbSchemaTable(
              System.Data.OleDb.OleDbSchemaGuid.Columns,
              new object[] { null, null, null, null });
            JetProviderFactory.JetStoreSchemaDefinitionRetrieveTest.ShowDataTableContent(schemaTable);
            */


            Console.WriteLine("Boolean materialization ===========================================================");
            Console.WriteLine(context.Students.Select(c => new { MyNewProperty = (bool)true }).ToList().Count);
            Console.WriteLine(context.TableWithSeveralFieldsTypes.Select(c => new { MyNewProperty = (bool)true }).ToList().Count);



            context.Dispose();


            Console.WriteLine("Press any key to exit.....");
            Console.ReadKey();
        }



        private static void ShowCourses(String showString, IEnumerable<Course> courses)
        {
            Console.WriteLine(showString);
            foreach (Course course in courses)
            {
                Console.WriteLine("CourseID:{0,-10} CourseTitle:{1,-15} Department:{2,-10}",
                    course.CourseID, course.Title, course.DepartmentID);
            }
        }

        /// <summary>
        /// Use the Entity SQL to implement the Between operation
        /// </summary>
        /// <returns></returns>
        private static List<Course> GetCoursesByEntitySQL(EntityConnection ec)
        {
            School school = new School(ec);
            return school.Courses.Where(c => c.CourseID.StartsWith("C2")).ToList();
        }

        /// <summary>
        /// Use the Entity SQL to implement the Between operation
        /// </summary>
        /// <returns></returns>
        private static List<Course> GetCoursesByEntityEscapedLikeSQL(EntityConnection ec)
        {
            School school = new School(ec);
            return school.Courses.Where(c => c.CourseID.StartsWith("%")).ToList();
        }

        /// <summary>
        /// Use the extension method to implement the Between operation
        /// </summary>
        /// <returns></returns>
        private static List<Course> GetCoursesByExtension(EntityConnection ec)
        {
            School school = new School(ec);
            return school.Courses.Between(c => c.CourseID, "C1050", "C3141").ToList();
        }


    }
}