using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMEDIA.Shared.DTO;

public class EncomendaDTO
{
    public MoradaDTO Morada { get; set; }
    public List<EncomendaItemDTO> Itens { get; set; }
}
