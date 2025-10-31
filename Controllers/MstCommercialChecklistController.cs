using GenericController.Controllers;
using GenericController.DTOs;
using GenericController.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GenericController.Controllers
{
    [Route("api/[controller]")]
    public class MstCommercialChecklistController : GenericMasterController<MstCommercialChecklist, MstCommercialChecklistDto>
    {
        public MstCommercialChecklistController(
            IMediator mediator,
            ILogger<MstCommercialChecklistController> logger)
            : base(mediator, logger)
        {
        }

        protected override object GetIdFromDto(MstCommercialChecklistDto dto)
        {
            return dto.ChkId;
        }

        protected override System.Linq.Expressions.Expression<System.Func<MstCommercialChecklist, bool>>? CreateFilterExpression(string filter)
        {
            // Example: filter by Particulars or CommercialPolicy
            return e => e.Particulars.Contains(filter) ||
                       (e.CommercialPolicy != null && e.CommercialPolicy.Contains(filter));
        }
    }
}