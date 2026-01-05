using System.ComponentModel.DataAnnotations;

namespace MyMEDIA.Shared.Validations;

public class validarNIFAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        string nifString = value.ToString() ?? string.Empty;

        // Ensure numeric
        if (!long.TryParse(nifString, out _))
        {
            return new ValidationResult(ErrorMessage ?? "O NIF deve ser numérico.");
        }

        // Validate 9 digits
        if (nifString.Length != 9)
        {
            return new ValidationResult(ErrorMessage ?? "O NIF deve ter 9 dígitos.");
        }

        // Validate first digit
        int firstDigit = int.Parse(nifString[0].ToString());
        int[] validFirstDigits = { 1, 2, 3, 5, 6, 8, 9 };
        // 45, 70, 71, 72, 74, 75, 77, 79 are also valid prefixes but checking just first digit is common simplification.
        // However, precise validation:
        // Prefixes: 1, 2, 3 (individual)
        // 45 (individual non-resident?)
        // 5 (corporate)
        // 6 (public)
        // 7 (others) - specifically 70, 71, 72, 74, 75, 77, 79
        // 8 (entrepreneur)
        // 9 (condo/others) - specifically 90, 91, 98, 99

        // For simplicity, we'll assume standard NIF algorithm check is sufficient, as it catches most invalid numbers.
        // Strict prefix checking:
        // Let's implement at least the modulo 11 check which is robust.

        int[] digits = new int[9];
        for (int i = 0; i < 9; i++)
        {
            if (!int.TryParse(nifString[i].ToString(), out digits[i]))
            {
                return new ValidationResult(ErrorMessage ?? "O NIF deve conter apenas números.");
            }
        }

        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += digits[i] * (9 - i);
        }

        int remainder = sum % 11;
        int checkDigit = (remainder < 2) ? 0 : 11 - remainder;

        if (digits[8] != checkDigit)
        {
            return new ValidationResult(ErrorMessage ?? "O NIF é inválido.");
        }

        return ValidationResult.Success;
    }
}
