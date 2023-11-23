using System.ComponentModel;

namespace WoofAdopciones.Shared.Enums
{
    public enum RequestStatusVolunteering
    {

        [Description("Enviada")]
        Sent,

        [Description("En revisión")]
        Review,

        [Description("Rechazada")]
        Rejected,

        [Description("Aprobada")]
        Approved,
    }
}