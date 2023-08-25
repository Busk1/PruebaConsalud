using System.Text.RegularExpressions;

namespace PruebaConsalud.Validators
{
    public static class RutValidator
    {
        public static bool MustBeValid(string value)
        {
            var val = value.AsSpan();
            if (val.Length > 1 && val.Length < 10)
            {
                var rut = val[0..^1];
                var dv = val[^1];

                Regex expresion = new Regex("^([0-9]+[0-9Kk])$");
                if (!expresion.IsMatch(val.ToString()))
                    return false;

                if (dv.ToString().ToUpper() != Digito(int.Parse(rut)))
                    return false;

                return true;

                static string Digito(int rut)
                {
                    int suma = 0;
                    int multiplicador = 1;
                    while (rut != 0)
                    {
                        multiplicador++;
                        if (multiplicador == 8)
                            multiplicador = 2;
                        suma += (rut % 10) * multiplicador;
                        rut /= 10;
                    }
                    suma = 11 - (suma % 11);
                    if (suma == 11)
                        return "0";
                    else if (suma == 10)
                        return "K";
                    else
                        return suma.ToString();
                }
            }
            return false;
        }
    }
}
