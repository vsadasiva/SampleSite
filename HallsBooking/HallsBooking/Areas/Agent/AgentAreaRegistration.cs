using System.Web.Mvc;

namespace HallsBooking.Areas.Agent
{
    public class AgentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Agent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Agent_default",
                "Agent/{controller}/{action}/{id}",
                new { action = "HallReg", id = UrlParameter.Optional },
                namespaces: new[] {"HallsBooking.Areas.Agent.Controllers"}
            );
        }
    }
}
