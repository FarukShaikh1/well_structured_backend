using System.Security.Cryptography;
using System.Text;

namespace FMS_Collection.Application.Services
{
  public class RandomGeneratorService
  {
    public static string GenerateNumericOtp(int length)
    {
      const string digits = "0123456789";
      var data = new byte[length];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(data);
      var sb = new StringBuilder(length);
      for (int i = 0; i < length; i++)
      {
        sb.Append(digits[data[i] % digits.Length]);
      }
      return sb.ToString();
    }

    public static string GeneratePassword(int length, bool includeSpecialChars = false)
    {
      const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      const string lower = "abcdefghijklmnopqrstuvwxyz";
      const string digits = "0123456789";
      const string special = "!@#$%^&*()-_=+<>?";

      string chars = upper + lower + digits;
      if (includeSpecialChars)
        chars += special;

      var data = new byte[length];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(data);

      var sb = new StringBuilder(length);
      for (int i = 0; i < length; i++)
      {
        sb.Append(chars[data[i] % chars.Length]);
      }

      return sb.ToString();
    }
  }
}


