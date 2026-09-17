namespace CustomerTestApp1.Helpers;

public static class OTPGenerator {
  public static string Generate() {
    string[] letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "S"];
    int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 0];

    var randomLetter = Random.Shared.GetItems(letters, 2);
    var randomNumber = Random.Shared.GetItems(numbers, 2);

    return $"{randomLetter[0]}{randomNumber[0]}{randomLetter[1]}{randomNumber[1]}";
  }
}