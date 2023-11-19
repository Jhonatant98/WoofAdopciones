using System.ComponentModel;

namespace WoofAdopciones.Shared.Enums
{
    public enum AdoptionStatus
    {
        [Description("En revisión")]
        InReview,

        [Description("Visita al hogar")]
        GoHome,

        [Description("Aprobado")]
        Approved,

        [Description("Rechazado")]
        Rejected,
    }
}
