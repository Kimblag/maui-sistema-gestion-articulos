using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoApp.UI.Helpers
{
    public static class StringHelper
    {
        public static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            texto = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var ch in texto)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
