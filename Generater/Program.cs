using FactoryGenEntity;
using System;

namespace Generater
{
    class Program
    {
        static void Main(string[] args)
        {
            var _nameSpaceModels = "Generater.Models";
            // Create file json
            var _pathOutput = @"e:\FileJson\";
            var _nameProject = "Organization.TenService";
            var handler1 = Handler.GetHandlerCreate(_nameSpaceModels, _pathOutput, _nameProject);
            handler1.Process();
            // Replace file
            //var _pathBuilder = @"D:\New folder\BookManager\src\BookManager.EntityFrameworkCore\EntityFrameworkCore\BookManagerDbContextModelCreatingExtensions.cs";
            //var _nameModule = "LoyaltyService"; //Ten Microservice module
            //var handler2 = Handler.GetHandlerReplace(_pathBuilder, _nameModule, _nameSpaceModels);
            //handler2.Process();
        }
    }
}
