using System.Text.RegularExpressions;
using Common.Exceptions;

namespace Common.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateStringLength(string input, int maxLength = 50)
        {
            if (input.Length > maxLength)
            {
                throw new InvalidInputException($"String cannot be longer than {maxLength} symbols");
            }
        }

        public static void ValidateRowsAmount(int numberOfSeats, int rows)
        {
            if (numberOfSeats % rows != 0)
            {
                throw new InvalidInputException("Invalid number of rows for this amount of seats");
            }
        }

        public static void ValidateMaxValue(int input, int maxValue = 250)
        {
            if (input > maxValue)
            {
                throw new InvalidInputException($"Value cannot be greater than {maxValue}");
            }
            if (input == 0)
            {
                throw new InvalidInputException($"Value cannot be zero");
            }
        }

        public static void ValidateRatingValue(int rating)
        {
            if (rating <= 0 || rating >10)
            {
                throw new InvalidInputException("Value must be between 1 and 10");

            }
        }

        public static void ValidateIfNegative(int input)
        {
            if (input < 0)
            {
                throw new InvalidInputException("Value cannot be less than zero.");
            }
        }
        
        public static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidInputException("Email is null or whitespace.");
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern);

            if (!regex.IsMatch(email))
            {
                throw new InvalidInputException("Invalid email format.");
            }
        }
        
        
        
        public static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidInputException("Password is null or whitespace.");
            }

            string pattern = @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$";
            Regex regex = new Regex(pattern);

            if (!regex.IsMatch(password))
            {
                throw new InvalidInputException("Password does not meet complexity requirements.");
            }
        }
        
        public static void ValidateSignUpData(string username, string password, string email)
        {
            ValidateStringLength(username, maxLength: 30);
            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidInputException("Username is null or whitespace.");
            }
            ValidatePassword(password);
            ValidateEmail(email);
            
        }
        
        public static void EnsureEntityFound<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new NotFoundException("Not found.");
            }
        }
    }
}