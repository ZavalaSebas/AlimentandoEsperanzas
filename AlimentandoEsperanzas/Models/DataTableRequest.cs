using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlimentandoEsperanzas.Models
{
    public class DataTableRequest
    {
        // Propiedades para representar los datos de la tabla y los filtros
        public string ColumnName { get; set; }
        public string SearchValue { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

