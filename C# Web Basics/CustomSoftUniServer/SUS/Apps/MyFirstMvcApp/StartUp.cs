using Microsoft.EntityFrameworkCore;
using MyFirstMvcApp.Controllers;
using MyFirstMvcApp.Data;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMvcApp
{
    public class StartUp : IMvcApplication
    {
        public void ConfigureServices()
        {

        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }

       
    }
}
