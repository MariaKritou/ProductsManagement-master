using ProductsManagement.Models;
using SQW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Repositories
{
  public class UserRepository
  {
    private readonly ISQWWorker worker;

    public UserRepository(ISQWWorker worker)
    {
      this.worker = worker;
    }

    List<User> users = new List<User>();

    //public UserRepository()
    //{
    //  var user1 = new User { username = "maria", password = "maria", firstName = "Maria", lastName = "Kritou", age = 23, isAdmin = true };
    //  var user2 = new User { username = "dimitris", password = "dimitris", firstName = "Dimitris", lastName = "Maragkos", age = 25, isAdmin = true };
    //  var user3 = new User { username = "vasilis", password = "vasilis", firstName = "Vasilis", lastName = "Papoglou", age = 26, isAdmin = false };

    //  users.Add(user1);
    //  users.Add(user2);
    //  users.Add(user3);
    //}


    //public User login(string username, string password)
    //{

    //  foreach (var user in users)
    //  {
    //    if (user.username == username && user.password == password)
    //    {
    //      return user;
    //    }

    //  }
    //  return null;
    //}


    public User login(string username, string password)
    {
      //var user = users.FirstOrDefault(p => p.username == username && p.password == password);
      User user = null;
      try
      {
        worker.run(context =>
        {
          user = context.createSpCommand("MARIADEMO.MAIN.LOGIN")
          .addCursorOutParam("RET_VAL")
          .addStringInParam("A_USERNAME", username)
          .addStringInParam("A_PASSWORD", password)
          .first<User>();
        });

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
     
      return user;
    }


    //KANE TH ME ID
    public User getUserByUsername(string username)
    {
      //var user = users.FirstOrDefault(p => p.username == username);
      User user = null;

      worker.run(context =>
      {
        user = context.createSpCommand("MARIADEMO.MAIN.GET_USER_BY_USERNAME")
        .addCursorOutParam("RET_VAL")
        .addStringInParam("A_USERNAME", username)
        .first<User>();
      });

      return user;
    }

    public User getUserById(int id)
    {
      User user = null;

      worker.run(context =>
      {
        user = context
        .createSpCommand("MARIADEMO.MAIN.GET_USER_BY_ID")
        .addCursorOutParam("RET_VAL")
        .addNumericInParam("A_USER_ID", id)
        .first<User>();

      });

      return user;
    }

    public void editUser(User user)
    {
      //var olduser = getUserByUsername(user.username);
      //olduser.username = user.username;
      //olduser.password = user.password;
      //olduser.firstName = user.firstName;
      //olduser.lastName = user.lastName;
      //olduser.age = user.age;

      worker.run(context =>
      {
        context.createSpCommand("MARIADEMO.MAIN.EDIT_USER")
        .addStringInParam("A_FIRSTNAME", user.firstName)
        .addStringInParam("A_LASTNAME", user.lastName)
        .addNumericInParam("A_AGE", user.age)
        .addNumericInParam("A_USER_ID", user.id)
        .execute();
      });
    }

    public void createUser(User user)
    {
      worker.run(context =>
      {
        context.createSpCommand("MARIADEMO.MAIN.CREATE_USER")
        .addStringInParam("A_USERNAME", user.username)
        .addStringInParam("A_PASSWORD", user.password)
        .addStringInParam("A_FIRSTNAME", user.firstName)
        .addStringInParam("A_LASTNAME", user.lastName)
        .addNumericInParam("A_AGE", user.age)
        .execute();
      });
    }
  }
}

