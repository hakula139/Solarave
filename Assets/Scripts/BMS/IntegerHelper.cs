using System.Linq;
using UnityEngine;

namespace BMS {
  public class IntegerHelper {
    public static int ParseBase36(string number) {
      return number.ToLower().Aggregate(0, (acc, digit) => {
        int parsedDigit;
        if (char.IsNumber(digit)) {
          parsedDigit = digit - '0';
        } else if (char.IsLower(digit)) {
          parsedDigit = digit - 'a' + 10;
        } else {
          Debug.LogWarningFormat("failed to parse number, number=<{0}>", number);
          parsedDigit = 0;
        }
        return (acc * 36) + parsedDigit;
      });
    }

    public static bool InBounds(int index, object[] array) {
      return index >= 0 && index < array.Length;
    }

    public static bool IsInteger(string s) {
      return s.All(c => char.IsDigit(c));
    }
  }
}
