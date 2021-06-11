using System;
using Xunit;
using WebAPI.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace WebPI_TestProject
{
    public class RandomPasswordTests
    {
        [Fact]
        public void GenerateRandomPassword_MustBeValidLength()
        {
            bool mustBeTrue = false;
            string randomPassword = RandomPassword.GenerateRandomPassword();

            if (randomPassword.Length == 5) { mustBeTrue = true; }

            Assert.True(mustBeTrue);
        }

        [Fact]
        public void GenerateRandomPassword_MustContainNumber()
        {
            bool isContainNumber = false;
            string randomPassword = RandomPassword.GenerateRandomPassword();
           
            isContainNumber = randomPassword.ToCharArray().Any(char.IsDigit);

            Assert.True(isContainNumber);
        }

        private static bool IsTextContainNumberManual(string text)
        {
            List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            foreach (char c in text)
            {
                foreach (int number in numbers)
                {
                    if (int.Parse(c.ToString()) == number)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}