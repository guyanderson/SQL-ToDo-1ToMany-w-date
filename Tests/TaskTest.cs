using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDo
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Task.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Task firstTask = new Task("Mow the lawn", 1);
      Task secondTask = new Task("Mow the lawn", 1);

      //Assert
      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1);

      //Act
      testTask.Save();
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1);

      //Act
      testTask.Save();
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      Task testTask = new Task("Do the dishes", 1);
      testTask.Save();

      Task foundTask = Task.Find(testTask.GetId());

      Console.WriteLine($"description: {testTask.GetDescription()}, ID: {testTask.GetId()}, catId: {testTask.GetCategoryId()}");
      Console.WriteLine($"description: {foundTask.GetDescription()}, ID: {foundTask.GetId()}, catId: {testTask.GetCategoryId()}");
      Assert.Equal(testTask, foundTask);
    }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
      Console.WriteLine(Task.GetAll().Count);
      Console.WriteLine(Category.GetAll().Count);
    }
  }
}
