using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Components;
using RCLProdutos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RCLProdutos.Shared.ModalCard
{
    public partial class ModalCard
    {


        protected override async Task OnInitializedAsync()
        {

        }


        private string estado = "block"; 
        
        private string acao;

        private void Modal()
        {
            acao = estado;

            if (acao == "block")
                estado = "none";
            else estado = "block";

        }

    }




//    <Script>

//    // Get the modal
//    var modal = document.getElementById("myModal");

//        // Get the button that opens the modal
//        var btn = document.getElementById("myBtn");

//        // Get the <span> element that closes the modal
//        var span = document.getElementsByClassName("close")[0];

//        // When the user clicks the button, open the modal
//        btn.onclick = function()
//        {
//            modal.style.display = "block";
//        }

//        // When the user clicks on <span> (x), close the modal
//        span.onclick = function()
//        {
//            modal.style.display = "none";
//        }

//        // When the user clicks anywhere outside of the modal, close it
//        window.onclick = function(event)
//    {
//            if (event.target == modal) {
//            modal.style.display = "none";
//        }
//        }
//</script>

}

