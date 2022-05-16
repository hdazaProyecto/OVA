using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Funciones
{
    public static class Helper
    {
        public static MvcHtmlString Menu()
        {
            string html = "<nav class=\"navbar navbar-light bg - light\">< span class=\"navbar-brand\" href=\"#\">@Html.ActionLink(\"Tema\", \"Index\", \"Admin\")</span></nav><nav class=\"navbar navbar-light bg-light\"><span class=\"navbar-brand\">@Html.ActionLink(\"Unidades\", \"ListarUnidades\", \"Admin\")</span></nav><nav class=\"navbar navbar-light bg-light\"><span class=\"navbar-brand\">@Html.ActionLink(\"Recursos\", \"listarRecursos\", \"Admin\")</span></nav>if (us.idRol == 1){<nav class=\"navbar navbar-light bg-light\"><span class=\"navbar-brand\">@Html.ActionLink(\"Email\", \"email\", \"Admin\")</span></nav>";
            return new MvcHtmlString(html);
        }
    }
}